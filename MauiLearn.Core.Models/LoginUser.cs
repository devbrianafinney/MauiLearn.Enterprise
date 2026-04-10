using System;
using System.Collections.Generic;
using System.Text;

namespace MauiLearn.Core.Models
{
    public class LoggingInUser
    {
        public required string Username { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
     
    }

    public class LoggedInUser
    {
        private string _username;
        private string _password;
        private bool _isLogging = false;
        public required string Username { get; set; } = string.Empty;

        public required string PasswordHash { get; set; } = string.Empty;

        public LoggedInUser(string Username, string PasswordHash, bool isLogging = false)
        {
            _username = Username;
            _password = PasswordHash;
            _isLogging = isLogging;
        }
    }
}
