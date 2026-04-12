using CommunityToolkit.Mvvm.ComponentModel;
using MauiLearn.Core.Models;
using MauiLearn.Core.Services.Interfaces;
using MauiLearn.Core.Services.Messages;
using MauiLearn.Core.Services.Repository;
using MauiLearn.MultiApp.Interfaces;
using MauiLearn.MultiApp.Models;
using MauiLearn.MultiApp.Pages;
using MauiLearn.MultiApp.Services.Helpers;
using MauiLearn.MultiApp.StateMessages;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using Font = Microsoft.Maui.Font;

namespace MauiLearn.MultiApp
{
    /*
     * 3/28/2026: ONLY TO KNOW WHAT NAVIGATION IS GOING TO BE MOVING ON...
     * AUTHENTICATED - OR IS NOT.
     * 
     * This file defines the AppShell class, which is used to define visual hierarchy of the app.
     */
    public partial class AppShell : Shell
    {
        /*02/16/2026 - app.Build() --> Navigation should be? YES ....but, .....(damn):
        It is safe, it works, it is the correct pattern for login-gate navigation, 
        it will not break shell, it will not cause a navigation loop as long as you
        check the target route: This is clean & correct: await _nav.GoToAsync("LoginView");
        ****** The KEY RULE: you MUST call args.Cancel() BEFORE navigating
        *IF YOU DON'T CANCEL - coulda, or a silent failure, too. 
        * 
         2/8/26: Register non?Shell routes at startup
         Create an extension to register routes for pages you might navigate to that are
         not declared in Shell (detail pages, modal pages, etc.).
         2/17/26:  KNOW WHY WHERE --> Shell pages belong in XAML.
                                  --> Conditional pages belong in AppShell.
                                  --> Non‑Shell pages belong in Extensions.
                                  --> Just don’t register the same route twice.
         */
        #region Interfaces
        private readonly IAuthStateService? _authStateService;
        private readonly INavigationService _navService = null;
        private readonly IMessageService? _messagesSubscribedService = null;
        private readonly IBaseShellModel _baseShellModel = null;
        #endregion
        #region Publishes/Subscribes
        private MessageToken? _globalPublishedToken = null;
        private MessageToken? _tabBarPublishedToken = null;
        private MessageToken? _flyoutPublishedToken = null;
        private MessageToken? _authenticationPublishedToken = null;
        #endregion
        /* navigation is a list of ITEMS from the model that will to determine and utilize when/where based on after shell base 
        private Dictionary<Type, Type> m_tabBarList = new();
        private Dictionary<Type, Type> m_flyoutList = new();*/

        private AuthProfile m_authProfile = null; //initially empty, but will be updated by messages and other options later, like BaseCoreModel
        private AppMenuItem m_appMenuCurrentView = null; //initially empty, but will be updated by messages and other options later, like BaseCoreModel
        private Dictionary<Type, Type> m_currentAuthMapList; //enter list same list with dictionary and can be utilized several, choosing, edit, create with auth as determined
        private List<AppMenuItem> m_menuItemsList; //enter list from ViewModelToPageHelper
        private TabBar m_baseTabBar { get; set; } = new TabBar();
        private FlyoutItem m_baseFlyoutItem { get; set; } = new FlyoutItem();
        public static TaskCompletionSource<bool> ShellReady { get; } = new();

        public AppShell(INavigationService navService,
                                IAuthStateService? authStateService,
                                IMessageService? messagesService) 
                                //, IBaseShellModel baseShellModel)
        {
            InitializeComponent();

            _authStateService = authStateService;
            _messagesSubscribedService = messagesService;
            _navService = navService;
            Navigating += OnShellNavigating;
            m_authProfile = new AuthProfile() { PasswordHash = "", Username = "Guest",  IsLoggedIn = false, DisplayName = "Guest" }; //initially empty, but will be updated by messages and other options later, like BaseCoreModel
            /* 
             * // Subscribe to collection change events
             * // m_ViewModelToPageHelper.CollectionChanged += OnCollectionChanged;
             * // Navigating += OnShellNavigating; need auth just not here...
             * // InitializeComponent();
             * public event EventHandler<ShellNavigatedEventArgs> Navigated;
             * public event EventHandler<ShellNavigatingEventArgs> Navigating;
             */

            //CreateAppShellLinks(null); //just for initial, but will be modified by messages and other options later, like BaseCoreModel
            #region Publish/Subscribe

            //authentications for publish and subscribe
            _messagesSubscribedService?.Publish(new AuthProfileStateChangedMessage(m_authProfile));
            _authenticationPublishedToken = _messagesSubscribedService?.Subscribe<AuthProfileStateChangedMessage>(OnAuthProfileStateChangedMessage);

            //authentications for publish and subscribe
            _messagesSubscribedService?.Publish(new AppMenuCurrentViewItemChangedMessage(m_appMenuCurrentView));
            _authenticationPublishedToken = _messagesSubscribedService?.Subscribe<AppMenuCurrentViewItemChangedMessage>(OnAppMenuCurrentViewItemChangedMessage);
            #endregion

            #region App based for menu items + types
            /*Menu Tab Bar Items-  Entire list exist - now decide is with auth or other properties
                  - A bottom tab bar (on mobile) or top tab bar (on desktop) with a single tab labeled "Base Page".
                  - Selecting the tab will display the content of the BaseCoreModel.
                  - Add a Tab with a ShellContent pointing to BaseCoreModel
            */
            m_currentAuthMapList = ViewModelToPageHelper.GetViewModelToPageMappingList().ToDictionary(x => x.Key, x => x.Value);
            m_menuItemsList = ViewModelToPageHelper.GetAppMenuItems().ToList();

            CreateAppBaseShell();
            #endregion
        }

        #region Publish/Subscribe Methods
        private void OnAuthProfileStateChangedMessage(AuthProfileStateChangedMessage msg)
        {
           m_authProfile = msg.AuthProfile;
        }

        private void OnAppMenuCurrentViewItemChangedMessage(AppMenuCurrentViewItemChangedMessage  msg)
        {
            m_appMenuCurrentView = msg.AppMenuCurrentView;
        }
        #endregion
        //m_ViewModelToPageMapping.CollectionChanged += (s, e) =>
        //private async void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        //{
        //    //authentication, etc. changes to the collection, or just log it for now
        //    //if can, need to get out of navigation OnShellNavigating
        //  //whatever you want to do when the collection changes, for now just logging
        //    Console.WriteLine($"Dictionary-like collection changed: {e.Action}");
        //}



        //initial global visual options idea
        public void CreateAppBaseShell()
        {
            /*3/10/26 - from nada for learning orig written 
            var currentTheme = Application.Current!.RequestedTheme;
            currentTheme == AppTheme.Light ? 0 : 1;*/
            var currentTheme = Application.Current!.RequestedTheme;
            Application.Current!.UserAppTheme = currentTheme == 0 ? AppTheme.Light : AppTheme.Dark;
            m_baseTabBar.Title = "Hey, I'm a Tab Bar"!;
            m_baseFlyoutItem.Title = "Hey, I'm a Flyout Item"!;
            //m_baseFlyoutItem.Items.Add(new ShellContent { ContentTemplate = new DataTemplate(typeof(BaseCoreModel)) });

            GetCurrentMenuList();
        }

        /*Menu Items:
         *  Dictionary => m_CurrentAuthMapList
         *  List<AppMenuItem> => m_MenuItemsList
         */
        private void GetCurrentMenuList()
        {
            //m_currentAuthMapList - already loaded
            //m_menuItemsList - already loaded
            //if user is not logged in, want to see the tabs to see for tabs with Login/Register ....
            //if user is not logged in, all see flyout except settings/profiles
            int i = 0;
            foreach (var currentItem in m_menuItemsList)
            { 
                Type nowItem = ViewModelToPageHelper.GetPageType(currentItem.NameModel);
                i++;
                
                var now = nowItem.Name;//just to get the page type for the content template, but could be used for more later, like route store, etc.
                //for tabs IF only is user is not logged in AND only Tab and NOT on Flyout {both}
                if (!m_authProfile.IsLoggedIn && currentItem.IsViewAuthOnly == false && currentItem.IsViewTabOn == true)
                {
                    m_baseTabBar.Items.Add(new ShellContent { ContentTemplate = new DataTemplate(nowItem) });
                }
                //the user is logged in -- what to see on Tab and Settings BOTH
                if (m_authProfile.IsLoggedIn && currentItem.IsViewAuthOnly == true && currentItem.IsViewTabOn == true)
                {
                    m_baseTabBar.Title = $"Hello, {m_authProfile.DisplayName}";
                    m_baseTabBar.Items.Add(new ShellContent { ContentTemplate = new DataTemplate(nowItem) });
                }
                //maybe more thinking with logged in more need not just flyout avail (login, register, profiles, etc....)
                //home page - see on flyout regardless, UNLESS, 
                //i.e., "EditItemView" --> later need to add to ensure this with Page/ViewModel properties for helper
                if (currentItem.IsViewFlyoutOn = true) {
                    if (m_authProfile.IsLoggedIn && currentItem.IsViewAuthOnly == true)
                    {
                        m_baseFlyoutItem.Items.Add(new ShellContent { ContentTemplate = new DataTemplate(nowItem) });
                    } else if (currentItem.IsViewAuthOnly == false)//any user
                    {
                        m_baseFlyoutItem.Items.Add(new ShellContent { ContentTemplate = new DataTemplate(nowItem) });
                    }
                } 
            }

            m_baseFlyoutItem.Title = $"Hey, I'm a Flyout Item with {i} items in it!";


        }

        /* Normally, async void is dangerous. But Shell’s override is intentionally 
           * defined as MAUI forces you into async void. This is one of the few places
           * where async void is acceptable and expected.*/
        private async void OnShellNavigating(object sender, ShellNavigatingEventArgs e)
        {

        //    var BaseCoreModel = m_currentAuthMapList.Where(x => x.Value == typeof(BaseCoreModel)).Select(x => x.Key.Name).FirstOrDefault();
        //    if (e.Current == null && e.Target.Location.OriginalString != BaseCoreModel)
        //    {
        //        e.Cancel();
        //        // Get a deferral so we can run async code before continuing
        //        var deferral = e.GetDeferral();
        //        try
        //        {
        //            // Perform async work before navigation
        //            bool canNavigate = await ConfirmNavigationAsync();
        //            if (canNavigate) //IF, for other reason -- but 'now' not needed here
        //            {
        //                await _navService.GoToAsync<BaseCoreModel>(default);
        //            }
        //        }
        //        finally
        //        {
        //            deferral.Complete(); // Always complete the deferral
        //        }
        //    }
        }
        /*Initiate to simply get going somewhere, still working on Objects, before all services, data's, etc*/
        private async Task OnShellLoadedAsync()
        {
            ShellReady.TrySetResult(true);
        }

        //first nav method
        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);
        }

        /*Making sure that Shell is all ready to keep on going from out Services...*/
        private async Task<bool> ConfirmNavigationAsync()
        {  
            bool answer = await AppShell.ShellReady.Task;
            return answer;
        }
    }
}
