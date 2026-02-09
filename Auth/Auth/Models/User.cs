using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User : BaseModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public User()
        {
            Email = string.Empty;
            Password = string.Empty;
        }

        public User(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}