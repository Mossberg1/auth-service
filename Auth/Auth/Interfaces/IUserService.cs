using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Models;

namespace Auth.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateAsync(string email, string password);
        Task<bool> UserExistsAsync(string email);
    }
}