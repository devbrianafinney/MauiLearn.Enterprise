using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiLearn.Core.Models;
using MauiLearn.Core.Services.Interfaces;
using MauiLearn.Core.Services.Messages;
using MauiLearn.MultiApp.Interfaces;
using MauiLearn.MultiApp.StateMessages;
using System.Threading.Tasks;

namespace MauiLearn.MultiApp.ViewModels
{
    public partial class UserProfileViewModel : ObservableValidator //, IBaseShellModel
    {
        private readonly INavigationService _nav;
        private readonly IAuthStateService _authService;
        private readonly IMessageService _messages;
        private MessageToken? _token;
        [ObservableProperty] private AuthProfile m_authProfile = null;
        [ObservableProperty] private string m_displayInfo = null;
        [ObservableProperty] private string m_username = null;
        [ObservableProperty] private string m_displayName = "Who Areyou";
        [ObservableProperty] private string m_email = "no_user_loggedin@mauilearn.com";
        [ObservableProperty]private string m_introductionMessage = "We just want to start and beginning with all of this brand new (or at least seems like it's been a long enough time)!" ;


        public UserProfileViewModel(INavigationService nav, IAuthStateService authService, IMessageService messageService)
        {
            _authService = authService;
            _nav = nav;
            _messages = messageService;

            _token = _messages.Subscribe<AuthProfileStateChangedMessage>(OnAuthProfileStateChangedMessage);
            
        }

        private void OnAuthProfileStateChangedMessage(AuthProfileStateChangedMessage msg)
        {
            m_authProfile = msg.AuthProfile;
        }

        [RelayCommand]
        private void UpdateAuthState(AuthProfileStateChangedMessage msg)
        {
            if (msg == null)
            {
                //login - cuz you don't allowed to be here!
            }
            else
            {
                //current user is here
                m_authProfile = msg.AuthProfile;
                //do some updates for a UI info
                _messages.Publish(new AuthProfileStateChangedMessage(m_authProfile));
            }
        }

        [RelayCommand] private async Task GoToSaveProfile()
        {
            await _nav.GoBackAsync(); //3/10/26: absolutely nothing with this page. 
        }

        [RelayCommand] private async Task GoToCancel()
        {
            await _nav.GoBackAsync(); //3/10/26: absolutely nothing with this page. 
        }

        //public string DisplayName
        //{
        //    get => _displayName;
        //    set => SetProperty(ref _displayName, value);
        //}

        // IBaseShellModel implementation - *THINKING* why we
        // can use the Toolkit's ObservableObject/ObservableValidator instead of implementing
        // INotifyPropertyChanged ourselves
        // public event PropertyChangedEventHandler PropertyChanged;

        //public void OnPropertyChanged(string propertyName = "")
        //    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        //public bool SetProperty<T>(ref T backingStore, T value, string propertyName = "")
        //{
        //    if (EqualityComparer<T>.Default.Equals(backingStore, value))
        //        return false;

        //    backingStore = value;
        //    OnPropertyChanged(propertyName);
        //    return true;
        //}
    }
}
