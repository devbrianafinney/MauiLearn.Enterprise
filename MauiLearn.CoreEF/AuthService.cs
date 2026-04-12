using MauiLearn.Core.Models;
using MauiLearn.Core.Services.Interfaces;
using MauiLearn.Core.Services.Repository;
using MauiLearn.Core.Services.Validations;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiLearn.CoreEF
{
    /*maybe change folders later after bring in more need/want classes. 
     * for now........ its here
     */
    public class AuthService : IAuthService
    {
        private readonly IEfRepository<AuthProfile, Guid> _repo;
        private string PasswordHash { get; set; } = string.Empty; // Store hash, not plain text
         
        public AuthService(IEfRepository<AuthProfile, Guid> repo)
        {
            _repo = repo;
        }

        // Helper method to hash password
        public string SetPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password.Trim()));
            PasswordHash = Convert.ToBase64String(bytes);
            Console.WriteLine("SetPassword : PasswordHash in SetPassword: " + PasswordHash); // Debugging line to check the generated password hash)
            return PasswordHash;
        }

        public bool VerifyPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password.Trim()));
            PasswordHash = Convert.ToBase64String(bytes);
            Console.WriteLine("VerifyPassword(" + password + ") and PasswordHash: " + PasswordHash);
            return PasswordHash == Convert.ToBase64String(bytes);
        }

        public async Task<AuthProfile?> LoginAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(PasswordHash)){
                //until make things prettier, just register to get first time login working.
                var user = await RegisterAsync(username, password);
                return user;
            }
            else {
                var user = (await _repo.GetAllAsync())
                    .FirstOrDefault(x => x.Username == username && x.PasswordHash == PasswordHash);

                if (user == null)
                    return null;

                /* AMOUNT OF TIME LATER */

                user.IsLoggedIn = true;
                await _repo.UpdateAsync(user);

                return user;
            }
        }

        public async Task<AuthProfile> RegisterAsync(string username, string password)
        {
            if ((await _repo.GetAllAsync()).Any(x => x.Username == username))
            {
                throw new Exception("Username already exists.");
            }

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Username and password cannot be empty.");
            }

            if (password.Length < 6)
            {
                throw new Exception("Password must be at least 6 characters long.");
            }

            if (password.Length > 100)
            {
                throw new Exception("Password cannot be longer than 100 characters.");
            }

            if (username.Length < 3)
            {
                throw new Exception("Username must be at least 3 characters long.");
            }

            if (username.Length > 50)
            {
                throw new Exception("Username cannot be longer than 50 characters.");
            }

            string guidPassword = SetPassword(password);
            Console.WriteLine("guidPassword: " + guidPassword); // Debugging line to check the generated password hash)
            Console.WriteLine("PasswordHash: " + PasswordHash); // Debugging line to check the stored password hash))
            var profile = new AuthProfile
            {
                Username = username,
                PasswordHash = guidPassword,
                IsLoggedIn = true
            };

            await _repo.AddAsync(profile);
            return profile;
        }

        public async Task LogoutAsync()
        {
            var current = await GetCurrentAsync();
            if (current != null)
            {
                current.IsLoggedIn = false;
                await _repo.UpdateAsync(current);
            }
        }
        //3/5/2026 - will need to add settings for user + app for "time" while staying logged in. for now... just "logout" when app is closed.
        public async Task<AuthProfile?> GetCurrentAsync()
        {
            return (await _repo.GetAllAsync())
                .FirstOrDefault(x => x.IsLoggedIn); 
        }
    }
}
