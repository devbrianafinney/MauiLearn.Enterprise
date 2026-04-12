using System;
using MauiLearn.Core.Models;
using MauiLearn.Core.Services.Validations;



/*02/16/2026: Ok, hoping attempt to unify with these damn authentication!
 * Copilot did write a couple "ideas about it" early a couple paragraphs
 * already written absolutely beautiful. 
 * Anyhow --- this is what we are going to try and 'unify' proper now...
 */

namespace MauiLearn.Core.Services.Interfaces 
{
    public interface IAuthService
    {
        Task<AuthProfile> GetCurrentAsync();
        Task<AuthProfile> LoginAsync(string username, string password);
        Task<AuthProfile> RegisterAsync(string username, string password);

        Task LogoutAsync();
    }
}


