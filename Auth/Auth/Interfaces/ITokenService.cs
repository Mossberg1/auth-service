using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Auth.Models;

namespace Auth.Interfaces
{
    public interface ITokenService
    {
        string Generate(User user);
    }
}