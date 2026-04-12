using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiLearn.Core.Models;
using MauiLearn.Core.Services.Interfaces;
using MauiLearn.Core.Services.Messages;
using MauiLearn.MultiApp.Converters;
using MauiLearn.MultiApp.Interfaces;
using MauiLearn.MultiApp.Pages;
using MauiLearn.MultiApp.StateMessages;
using MauiLearn.MultiApp.Views;

//using static MauiLearn.MultiApp.Services.AuthStateService;
//MauiLearn.MultiApp.PageModels.MainPageModel
namespace MauiLearn.MultiApp.ViewModels
{
    /* 3/9/2026: PageModel pure UI logig.
     * -no fetching, no services, no async...just REACTING
     */
    public partial class MainViewModel : ObservableValidator
    {
        private readonly INavigationService _nav;
		private readonly IAuthStateService _authState;
        private readonly IMessageService _messages;
        private MessageToken? _token;
        [ObservableProperty] private AuthProfile? m_authProfile = null;
        [ObservableProperty] private bool isBtnLoginVisible;
        [ObservableProperty] private bool isBtnSettingsVisible;
        [ObservableProperty] private bool isBtnLogoutVisible;
        [ObservableProperty] private bool isBtnUserProfileSettingsVisible;
        [ObservableProperty] private string mainPageTitle;
		[ObservableProperty] private string profileUsername;

        InverseBoolConverter boolConverter = new InverseBoolConverter();

        bool m_IsLoggedIn = false;
        
        //only wants one element/control to decide with each ask to want to be either visibled, enabled
        public bool IsWantVisibleElement()
        {
           if (m_authProfile != null && !m_authProfile.IsDeleted)
            {
                return m_authProfile.IsLoggedIn; // will determine bool same as auth logged in
            }
            return true; //if not logged in to determine, it will be true regardless
        }
        public bool IsWantEnabledElement()
        {
            if (m_authProfile != null && !m_authProfile.IsDeleted)
            {
                return !m_authProfile.IsLoggedIn; //will opposite of a logged in user
            }
            return true; //else will true regardless
        }

        public MainViewModel(INavigationService nav, IAuthStateService? authState, IMessageService messages)
        {
            _nav = nav; //this is about to be notified with Messengers....
            _authState = authState;
            _messages = messages;

            //3/9/26: LISTENS for AuthState -- IsLoggedIn? IsLogout?
            /*LISTENS for UI visibility changes (recipent, message)
			//Registers a recipient for a given type of message.
			// recipient: The recipient that will receive the messages.
			//IMessenger.Register<AuthProfileStateChangedMessage>(
			//          object recipient,
			//          MessageHandler<object, AuthStageChangeMessage> handler)
            */

            /* "recipients" ->> recipientsMap  <<<----  TO NOW --> FROM "there"
             * -- _authService - (AuthStateService) is "set" for "EntityType: AuthProfile" */
            //WeakReferenceMessenger.Default.Register<AuthProfileStateChangedMessage>(
            //   this,(r, m) => { _authProfile = m.Value; });
            /* 3/16/26 - removed WeakReferenceMessager to Core.IMessage */
            _messages.Publish(new AuthProfileStateChangedMessage(null));
            _token = _messages.Subscribe<AuthProfileStateChangedMessage>(OnAuthProfileStateChangedMessage);

            UpdateAuthState();
        }

       

        private void OnAuthProfileStateChangedMessage(AuthProfileStateChangedMessage msg)
        {
            
            m_authProfile = msg.AuthProfile;
            m_IsLoggedIn = m_authProfile.IsLoggedIn;
            IsLoggedIn();
        }
        public bool IsLoggedIn()
        {
            return m_IsLoggedIn;
        }

        [RelayCommand]
		private void UpdateAuthState()
        {
            if (m_authProfile == null) 
            {
                IsBtnLoginVisible = true;
                isBtnUserProfileSettingsVisible = false;
                IsBtnSettingsVisible = false;
                ProfileUsername = "Do you want to Login?";
            }else
            {
                IsBtnLoginVisible = !m_authProfile.IsLoggedIn;
                isBtnUserProfileSettingsVisible = !m_authProfile.IsLoggedIn;
                IsBtnSettingsVisible = m_authProfile.IsLoggedIn;
                IsBtnLogoutVisible = m_authProfile.IsLoggedIn;
                ProfileUsername = m_authProfile.Username;
            }
        }

        /*** Do some buttons...or navigatings... etc */
        [RelayCommand]
        private async Task GoToLogin(AuthProfileStateChangedMessage msg)
        {
            if (msg == null || msg.AuthProfile == null)
            {
                //await _nav.NavigateToAsync<LoginView>(default).GetType;
                //await _nav.GoToAsync<LoginView>(default); //MOVE TO Page for Login 
            }
        }

        //[RelayCommand]
        //private async Task GoToIsBtnUserProfileSettingsCommand(AuthProfileStateChangedMessage msg)
        //{
        //    //CurrentUser here...or "just know" with Navigating/Service....Seems but, not there yet...
        //    if (msg.AuthProfile.IsLoggedIn)
        //        await _nav.GoToAsync<UserProfileView>();//IF Logged In, can go to Settings for CurrentUser
        //}

        //[RelayCommand]
        //private async Task GoToSettings(AuthProfileStateChangedMessage msg)
        //{
        //    //CurrentUser here...or "just know" with Navigating/Service....Seems but, not there yet...
        //    if (msg.AuthProfile.IsLoggedIn)
        //        await _nav.GoToAsync<SettingsView>(null);//IF Logged In, can go to Settings for CurrentUser
        //}

		[RelayCommand]
		public void GoToLogout()
        {
            _authState.Logout();
        }
    }
}
