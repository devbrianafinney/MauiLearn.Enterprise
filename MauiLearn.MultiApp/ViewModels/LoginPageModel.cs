using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MauiLearn.Core.Models;
using MauiLearn.Core.Services.Interfaces;
using MauiLearn.Core.Services.Messages;
using MauiLearn.Core.Services.Repository;
using MauiLearn.CoreEF;
using MauiLearn.MultiApp.Interfaces;
using MauiLearn.MultiApp.Pages;
using MauiLearn.MultiApp.StateMessages;
using MauiLearn.MultiApp.Views;
using System.Threading.Tasks;

namespace MauiLearn.MultiApp.ViewModels;

public partial class LoginViewModel : ObservableValidator
{
    private readonly INavigationService _navService;
    private readonly IAuthStateService _authService;
    private readonly IEfRepository<ItemModel, Guid> _db;
    private readonly IMessageService _messages;
    private MessageToken? _token;
    bool _isLoggedIn = false;
    [ObservableProperty] private AuthProfile? m_authProfile;
    [ObservableProperty] private string? m_Username = null;
    [ObservableProperty] private string? m_Password = null;
    [ObservableProperty] private string loginHeadline = "Log In";

    public LoginViewModel (INavigationService navService, 
        IAuthStateService authService,
        IMessageService messages)
    {
        _navService = navService;
        _authService = authService;
        _messages = messages;
        //_token = _messages.Subscribe<AuthProfileStateChangedMessage>(OnAuthProfileStateChangedMessage);
    }

    //private async void OnAuthProfileStateChangedMessage(AuthProfileStateChangedMessage message)
    //{
    //    _isLoggedIn = message.AuthProfile.IsLoggedIn;
    //    //of course, do not only just going for user profile view...this will go back to elements 
    //    if (message.AuthProfile.IsLoggedIn)
    //        await _navService.GoBackAsync();//IF Logged In, can go to Settings for CurrentUser

    //}

    [RelayCommand]
    private async Task Save()
    {
        var user = _authService.LoginAsync(m_Username ?? string.Empty, m_Password ?? string.Empty);
        
        if (user != null)
        {
            m_authProfile = _authService.CurrentProfile;
            LoginHeadline = "Button clicked!";
            //DO MORE LATER: maybe navigate to a "home" page or update the UI based on login success/failure
            //bool isGood = _validationRule.Check(_authProfile); //just cuz too...
            if (m_authProfile != null && m_authProfile.IsLoggedIn)
            {
                m_authProfile.LoggedOn = DateTime.Now;
                //broadcast this current authProfile
                _messages.Publish(new AuthProfileStateChangedMessage(m_authProfile));
                _navService.GoBackAsync();
            }
        }
        else
        {
            LoginHeadline = "Oops! We should go back to animations away for a while...";
        }
    }
}