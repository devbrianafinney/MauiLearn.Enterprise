using CommunityToolkit.Mvvm.Messaging;
using MauiLearn.Core.Models;
using MauiLearn.Core.Services.Interfaces;
using MauiLearn.Core.Services.Messages;
using MauiLearn.MultiApp.Interfaces;
using MauiLearn.MultiApp.StateMessages;

/*3/9/26: This is placing on app - maybe later with core
 * services, but only stays here until later as learning*/
namespace MauiLearn.MultiApp.Services
{
    public class AuthStateService : IAuthStateService
    {
        //just for EF .... 
        private readonly IAuthService _authService;
        IMessageService _messageService;
        public AuthProfile? CurrentProfile { get; private set; }

        public AuthStateService(IAuthService authService, IMessageService messageService)
        {
            _authService = authService;
            _messageService = messageService;
            //start to broadcast this AuthProfile
            InitializeAsync();
        }

        private void Broadcast()
        {
            _messageService.Publish(new AuthProfileStateChangedMessage(CurrentProfile));
        }

        public async Task InitializeAsync()
        {
            CurrentProfile = await _authService.GetCurrentAsync();
            Broadcast(); /*Not sure yet, but immediately WANTING FINALLY! :-) */
        }

        public async Task LoginAsync(string user, string pass)
        {
            CurrentProfile = await _authService.LoginAsync(user, pass);
            Broadcast();
        }

        public async Task RegisterAsync(string user, string pass)
        {
            CurrentProfile = await _authService.LoginAsync(user, pass);
            Broadcast();
        }

        public void Logout()
        {
            CurrentProfile = null;
            Broadcast();
        }
    }
}
