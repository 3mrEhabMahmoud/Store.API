using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Store.Domain.Entities.Identity;
using Store.Domain.Exceptions;
using Store.Services.Abstractions.Auth;
using Store.Shard;
using Store.Shard.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Auth
{
    public class AuthService(UserManager<AppUser> _userManager, IOptions<JwtOptions> options, IMapper _mapper) : IAuthService
    {
        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        public async Task<AddressDto?> GetCurrentUserAddressAsync(string email)
        {
            //_userManager.FindByEmailAsync(email); //this Function Dont Load the Navigational Property

            var user = await _userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user is null) throw new UserNotFoundException(email);
            return _mapper.Map<AddressDto>(user.Address);

        }

        public async Task<AddressDto?> UpdateCurrentUserAddressAsync(AddressDto request, string email)
        {
            var user = await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user is null) throw new UserNotFoundException(email);
            if(user.Address is null)
            {
                //Creat new Address
                user.Address = _mapper.Map<Address>(request);
            }
            else
            {
                //Update the Old Address
                user.Address.FirstName = request.FirstName;
                user.Address.LasttName = request.LastName;
                user.Address.City = request.City;
                user.Address.Street = request.Street;
                user.Address.Country = request.Country;

            }
            await _userManager.UpdateAsync(user);
            return _mapper.Map<AddressDto>(user.Address);
        }
        public async Task<UserResponse?> GetCurrentUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) throw new UserNotFoundException(email);
            return new UserResponse()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateJwtTokenAsync(user)
            };
        }

        public async Task<UserResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null) throw new UserNotFoundException(request.Email);

            var flag = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!flag) throw new UnauthorizedException();

            return new UserResponse()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateJwtTokenAsync(user)
            };
        }

        public async Task<UserResponse?> RegisterAsync(RegisterRequest request)
        {
            var user = new AppUser()
            {
                UserName = request.UserName,
                Email = request.Email,
                DisplayName = request.DisplayName,
                PhoneNumber = request.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) throw new RegistrationBadRequestException(result.Errors.Select(E => E.Description).ToList());

            return new UserResponse()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateJwtTokenAsync(user)

            };

        }


        private async Task<string> GenerateJwtTokenAsync(AppUser user)
        {
            //TOKEN :
            //1. Header    (type,Algo)
            //2. Payload   (Claims)
            //3. Signature   (Key)
            var authClaims = new List<Claim>()
          { new Claim(ClaimTypes.GivenName,user.DisplayName),
          new Claim(ClaimTypes.Email,user.Email),
          new Claim(ClaimTypes.MobilePhone,user.PhoneNumber)
          };


            var roles = await _userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var jwtOptions = options.Value;

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey));

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims : authClaims,
                expires : DateTime.Now.AddDays(jwtOptions.DurtioninDays),
                signingCredentials : new SigningCredentials(Key, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
