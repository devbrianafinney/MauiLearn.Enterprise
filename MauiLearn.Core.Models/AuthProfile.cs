using MauiLearn.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MauiLearn.Core.Models
{
   public class AuthProfile : BaseEntity<Guid>
    {
        public required string PasswordHash{ get; set; } // later: want to have the hash of the password, not the actual password, and want to have a salt as well for security
        //public required string Password { get; set; }
        public required string Username { get; set; }
        public string AuthRegistratorAppLocation { get; set; } = "MauiLearn.MultiApp";
        public bool IsLoggedIn { get; set; } = false; //time currently logged in
        public bool IsDeleted { get; set; } = false;
        public DateTime LoggedOn { get; set; } //needed for amount of time logged in, and for "last logged in" information LATER
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        /**** more later ****/
        public string? DisplayName { get; set; }
        public string? IntroductionMessage { get; set; }
        public string? Email { get; set; }

    }
}
