using Store.Shard.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Abstractions.Auth
{
    public interface IAuthService
    {
        Task<UserResponse?> LoginAsync(LoginRequest request);
        Task<UserResponse?> RegisterAsync(RegisterRequest request);

        //Check Email Exists
        Task<bool> CheckEmailExistsAsync(string email);
        //Get Current User
        Task<UserResponse?> GetCurrentUserAsync(string email);
        //Get Current User Address
        Task<AddressDto?> GetCurrentUserAddressAsync(string email);
        //Update Current User Address
        Task<AddressDto?> UpdateCurrentUserAddressAsync(AddressDto request,string email);
    }
}
