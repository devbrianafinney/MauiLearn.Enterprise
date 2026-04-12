using MauiLearn.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiLearn.MultiApp.Interfaces
{
    public interface IAuthStateService
    {

            AuthProfile? CurrentProfile { get; }
            Task InitializeAsync();
            Task LoginAsync(string user, string pass);
            Task RegisterAsync(string user, string pass);
            void Logout();
    }
}
