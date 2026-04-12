using CommunityToolkit.Mvvm.ComponentModel;
using MauiLearn.Core.Models;
using MauiLearn.Core.Services.Interfaces;
using MauiLearn.Core.Services.Messages;
using MauiLearn.MultiApp.Interfaces;
using MauiLearn.MultiApp.StateMessages;
using MauiLearn.MultiApp.Services.Helpers;
using System.ComponentModel.Design;
using MauiLearn.MultiApp.PageModels;
using MauiLearn.MultiApp.ViewModels;


/*2/9/2026: This is now: (from Copilot written)
    - type?safe
    - async?correct
    - Shell?friendly
    - future?proof
*/
/*Name	            Value	Description
Unknown	            0	    The navigation source is unknown.
Push	            1	    Navigation was initiated by pushing a page onto the stack.
Pop	                2	    Navigation was initiated by popping a page from the stack.
PopToRoot           3       Navigation was initiated by popping to the root page.
Insert	            4	    Navigation was initiated by inserting a page into the stack.
Remove	            5	    Navigation was initiated by removing a page from the stack.
ShellItemChanged	6	    Navigation was initiated by changing the active ShellItem.
ShellSectionChanged	7	    Navigation was initiated by changing the active ShellSection.
ShellContentChanged	8	    Navigation was initiated by changing the active ShellContent.

        /* Every navigation - ONLY to a Shell page - should go through this service. 
         * This is the only place where you should call Shell.Current.GoToAsync() 
         * directly. This ensures that all navigation goes through a single, 
         * testable point in your app, and allows you to easily add any additional
         * logic (such as authentication checks) that you want to run on every 
         * navigation.*/
/*var profile = await _auth.GetCurrentAsync();
    authProfile - if Id -> 0 then never created
    if (profile == null || !profile.IsLoggedIn)
    {
        await _nav.GoToAsync<MainPage>(
            new Dictionary<string, object?>
            //if you want to pass parameters to the page, you can do it here
            {
                ["Id"] = profile.Id
            });
    }
 
 * examples: 
* await _nav.GoToAsync<LoginView>();  
  await _nav.GoToAsync("MainPage/SettingsMoreView"); //and can with options too
  await _nav.GoToAsync<SettingsMoreView>(new Dictionary<string, object?>
   {
       ["Mode"] = "Advanced",
       ["UserId"] = 42
   });

   [QueryProperty(nameof(OrderNumber), "OrderNumber")] 
   public class OrderDetailViewModel : ViewModelBase 
   { 
       public int OrderNumber { get; set; } 
   } 
   await _nav.GoToAsync("OrderDetail", new Dictionary{ 
       { "OrderNumber", order.OrderNumber } 
   });
*/

namespace MauiLearn.MultiApp.Services
{
    /****************************************************
     *     IMPORTANT
     * - You DO NOT navigate to a View
     * - You DOT NOT use Shell to switch Views
     * ----any 'moving' inside the view should be the 
     *     modulating from inside the 'page' inside the 'shell'
     * The QueryProperty attribute allows us to provide a parameter for a 
     * property to map values to and a key to find values from the 
     * query parameters dictionary.
     * 
     * 3/8/2026: - INavigationService is NOT used to "multiple" in with ObservableValidator!!
     * 
     * /*************************************************/

    public class ShellNavigationService : ObservableValidator, INavigationService
    {
        /*3/30/26 */
        private readonly IServiceProvider _serviceProvider;
        private readonly Shell _shell;


        /* 3/13/2026: ******************************************
         * Should be ready for AuthService integration, 
         * but not yet fully implemented 
         * *****************************************************/
        //[ObservableProperty] bool IsSpecificAuthRequired { get; set; }  = false;
        public bool _isLoggedIn = false;
        private readonly IAuthStateService _authService;
        private readonly IMessageService _messages;
        //private List<ViewModelToPageMapping> _navListModels = new List<ViewModelToPageMapping>();
        private ViewModelToPageHelper _navStateModel;
        public ShellNavigatedEventArgs _navStateEventArgs { get; private set; } = default;
        private CancellationToken _cancellationToken;
        private MessageToken? _messageToken;
        private AuthProfile _authProfile = null;

        private readonly Dictionary<Type, List<Delegate>> _authHandlers = new();
        private string? _goToNavigationRoute;

        /*3/30/2026 - this is where we will want to add to the list of navigation states, and then publish that list to the app shell, 
         *   so that it can update the UI with the new navigation state. 
         *   - all others with priors will be placed lower and comments out
         *   - also, this is where we will want to add any additional logic that we want to run on every navigation, such as authentication checks.
         *     - for example, if we want to check if the user is logged in before allowing them to navigate to a certain page, we can do that here.
         *     - if (route == "SomePage" && !IsLoggedIn) { GoToAsync<LoginView>(); return; }
         *     - or, if we want to check if the user has a certain role before allowing them to navigate to a certain page, we can do that here as well.
         *     - if (route == "AdminPage" && !User.IsInRole("Admin")) { GoToAsync<UnauthorizedPage>(); return; }
         */
       
        //_token = _messages.Subscribe<AuthProfileStateChangedMessage>(OnAuthProfileStateChangedMessage);
        //_messageService.Publish(new AuthProfileStateChangedMessage(CurrentProfile));
        //private void OnAuthProfileStateChangedMessage(AuthProfileStateChangedMessage msg)
        //{
        //    m_authProfile = msg.AuthProfile;
        //}

        public ShellNavigationService(IAuthStateService? authService, 
                                    IMessageService messageService,
                                    IServiceProvider serviceProvider)
                                    //,Shell shell)
        {

            _authService = authService;
            _messages = messageService;
            //3/13/2026: Prepare sure to remove all authProfile info here; but, until..........
            _messageToken = _messages.Subscribe<AuthProfileStateChangedMessage>(OnAuthProfileStateChangedMessage);
            //looks better with _messageToken AFTER Logged in...

            //3/30/2026: - this is where we will want to add to the list of navigation states,
            //and then publish that list to the app shell,*/
            _serviceProvider = serviceProvider;
            //_shell = shell;
        }

        private void OnAuthProfileStateChangedMessage(AuthProfileStateChangedMessage msg)
        {
            //if the user is here...we can do ... want....if loggedin
            _authProfile = msg.AuthProfile;
            if (_authProfile != null) { _isLoggedIn = _authProfile.IsLoggedIn; }
            ; //determine is each other item can determine, if the user is LoggedIn?
        }

        /*3/30/26 - as of with Copilot and current to transistion
         * from existing already NavigationService and now
         * working to bring in with TypeSafe, routing dictionaries
         */
        public async Task NavigateToAsync<TViewModel>() where TViewModel : BaseCoreModel
        {
            var pageType = ViewModelToPageHelper.GetPageType(typeof(TViewModel));
            if (pageType == null) throw new InvalidOperationException("No page mapped for this ViewModel.");

            var page = (Page)_serviceProvider.GetService(pageType);
            if (page == null) throw new InvalidOperationException("Page not registered in DI container.");
            //4/1/2026: er
            //await _shell.GoToAsync(page.GetType().FullName);
            await Shell.Current.GoToAsync(page.GetType().FullName);
        }

        public async Task GoBackAsync(CancellationToken? cancellationToken = default) => await Shell.Current.GoToAsync("..", true);

        public async Task GoToAsync<TPage>(CancellationToken? token = null) where TPage : Page
        {
            await Shell.Current.GoToAsync(typeof(TPage).FullName, true); //true, bool animate
        }
    }
}



/* * 3/30/2026: - this is where we will want to add to the list of navigation states, and then publish that list to the app shell, 
 *   so that it can update the UI with the new navigation state. 
 *   - all others with priors will be placed lower and comments out
 *   
 *   
     //private void Broadcast(NavigationStateChangedMessage msg)
//{
//    if (msg == null)
//    {
//        _navListModels = new List<ViewModelToPageMapping>();
//    }
//}

public async Task GoToAsync(string route, IDictionary<string, object?>? parameters = default, CancellationToken? cancellationToken = default)
{
    await Shell.Current.GoToAsync(route, true, parameters);
}
*/
//        //private void OnNavigationStateChangedMessage(NavigationStateChangedMessage msg)
//        //{
//        //    //if the user is here...we can do ... want....if loggedin
//        //    if (msg != null)
//        //    {
//        //        string route = "something";
//        //        ViewModelToPageMapping _navStateModel = new ViewModelToPageMapping(route); 
//        //        msg.navModel.Add(_navStateModel);

//        //        _navStateModel.Route = "MainPage", //if ((msg.Route == null || msg.Route = string.IsNullOrEmpty()), msg.Route, "MainPage"),
//        //        _navStateModel.EventArgs = msg.EventArgs,
//        //        _navStateModel.CancellationToken = msg.CancellationToken,
//        //        _navStateModel.DateVisited = DateTime.Now;


//        //    _navListModels.Add(_navStateModel);
//        //    _messages.Publish(new NavigationStateChangedMessage(_navListModels));
//        //    }
//        //}

//        public async Task GoToAsync(string route, CancellationToken? cancellationToken = default)
//{
//    //just checking to learn some more here....
//    _navStateModel = new ViewModelToPageMapping(route);
//    _navStateModel.CancellationToken = cancellationToken;
//    var frame = Shell.Current.Frame;
//    var x = Shell.NavigationProperty.PropertyName;
//    var y = Shell.Current.CurrentState.Location.Authority;
//    var source = _navStateEventArgs.Source;
//    var guidId = Shell.Current.Id;
//    //ShellNavigatingEventArgs shellNavigating = Shell.Current;



//    await Shell.Current.GoToAsync(route, true); //assuming from all other classes to nav will decide, until . . . 
//}

//        public async Task GoToAsync<TPage>(CancellationToken? cancellationToken = default) where TPage : Page
//        {
//            var route = typeof(TPage).Name;
//            await Shell.Current.GoToAsync(route, true); //true, bool animate 
//        }

//        public async Task GoBackAsync(CancellationToken? cancellationToken = default)
//        {
//            await Shell.Current.GoToAsync("..", true); 
//        }

//    }
//}
