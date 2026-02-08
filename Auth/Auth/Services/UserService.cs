using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Data;
using Auth.Exceptions;
using Auth.Interfaces;
using Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHashService _hashService;

        public UserService(ApplicationDbContext dbContext, IHashService hashService)
        {
            _dbContext = dbContext;
            _hashService = hashService;
        }

        public async Task<User> CreateAsync(string email, string password)
        {
            var hashedPassword = _hashService.Hash(password);

            var user = new User(email, hashedPassword);

            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetByEmailAsync(string email) 
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user ?? throw new NotFoundException();
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }
    }
}