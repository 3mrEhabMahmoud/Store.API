using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Exceptions
{
    public class UserNotFoundException(string email) : NotFoundException($"user with email {email} was not found !!")
    {
    }
}
