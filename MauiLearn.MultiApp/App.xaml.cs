using MauiLearn.Core.Services.Interfaces;
using MauiLearn.Core.Services.Messages;
using MauiLearn.MultiApp.Interfaces;
using MauiLearn.MultiApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
/*
 * The App.xaml file contains app-wide XAML resources, such as colors, 
 * styles, or templates. The App.xaml.cs file generally contains code 
 * that instantiates the Shell application. In this project, it points 
 * to the AppShell class.
 */
namespace MauiLearn.MultiApp
{
    public partial class App : Application
    {
        private readonly IAuthStateService? _authStateService = null;
        private readonly INavigationService _navService;
        private readonly IMessageService? _messagesSubscribedService = null;
        public App(INavigationService navService)
        {
            InitializeComponent();
            _navService = navService;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell(_navService, _authStateService, _messagesSubscribedService));
        }
        //public App(INavigationService navService,
        //                        IAuthStateService? authStateService,
        //                        IMessageService? messagesService,
        //                        IGlobalBaseService? globalBaseService)
        //{
        //    InitializeComponent();
        //    _globalBaseService = globalBaseService;
        //    _authStateService = authStateService;
        //    _messagesSubscribedService = messagesService;
        //    _navService = navService;
        //}

        //protected override Window CreateWindow(IActivationState? activationState)
        //{
        //    return new Window(new AppShell(_navService, _authStateService, _messagesSubscribedService, _globalBaseService));
        //}
    }
}