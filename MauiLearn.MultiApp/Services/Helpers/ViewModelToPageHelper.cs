using MauiLearn.MultiApp.PageModels;
using MauiLearn.MultiApp.Pages;
using MauiLearn.MultiApp.ViewModels;
using MauiLearn.MultiApp.Views;
using MauiLearn.MultiApp.Models;

using System.Collections.ObjectModel;
using Microsoft.Maui.ApplicationModel;

namespace MauiLearn.MultiApp.Services.Helpers
{
    public class ViewModelToPageHelper
    {
        public readonly static Dictionary<Type, Type> _map = new Dictionary<Type, Type>();
        //private Dictionary<Type, Type> _map = new();

        private readonly static List<AppMenuItem> _appMenuItems = new List<AppMenuItem>() {
            /** Pages **/
            new AppMenuItem { Name = typeof(BaseCoreModel), NameModel = typeof(BaseCoreModel), IsViewAuthOnly = false, IsViewTabOn = false },

            new AppMenuItem { Name = typeof(RegisterNewView), NameModel = typeof(RegisterNewViewModel), IsViewAuthOnly = false, IsViewFlyoutOn = true, IsViewTabOn = true },
            new AppMenuItem { Name = typeof(LoginView), NameModel = typeof(LoginViewModel), IsViewAuthOnly = false, IsViewTabOn = true },
           
            /** Views **/
            new AppMenuItem { Name = typeof(DashboardView), NameModel = typeof(DashboardViewModel), IsViewAuthOnly = false },
            new AppMenuItem { Name = typeof(HomeView), NameModel = typeof(HomeViewModel), IsViewAuthOnly = false, IsViewTabOn = false },
            new AppMenuItem { Name = typeof(UserProfileView), NameModel = typeof(UserProfileViewModel), IsViewAuthOnly = true, IsViewTabOn=true, IsViewFlyoutOn = true }
        };

        public ViewModelToPageHelper(){}

        public static List<AppMenuItem> GetAppMenuItems()
        {
            return _appMenuItems;
        }

        public static Type GetPageType(Type viewModelType) => _map.TryGetValue(viewModelType, out var pageType) ? pageType : null;

        public static Dictionary<Type, Type> GetViewModelToPageMappingList()
        {
            var items = _appMenuItems;

            foreach (var item in items)
            {
                _map[item.NameModel] = item.Name; // Add or overwrite
            }

            return _map.Where(x => x.Key != null && x.Value != null).ToDictionary(x => x.Key, x => x.Value);
        }
   
        //public required string Title { get; set; } = "BaseCoreModel";
        //public NavigationType? ShellNavigationType { get; set; }
        //public Type ShellType { get; set; } = typeof(Type);
        //public required string RouteStore { get; set; } = "//Views/BaseCoreModel";
        //public ShellNavigatedEventArgs? EventArgs { get; set; }
        //public CancellationToken? CancellationToken { get; set; } = default(CancellationToken?);
        //public DateTime? DateVisited { get; set; } = DateTime.Now; //just for hierarchy of list possibilities

        //[SetsRequiredMembers]
        //public ViewModelToPageMapping(string route) => (RouteStore) = route;   public bool IsAuthOnly { get; set; } = false; /* for now, but could be more specific with enum of "scopes" or something */


        //TryGetValue(viewModelType, out var pageType) ? pageType : null;
        //private static readonly Dictionary<Type, Type> _map = new()
        //{
        //    { typeof(BaseCoreModel), typeof(BaseCoreModel) }, /* ?? not sure -- yet if need here at all */
        //    { typeof(RegisterPageModel), typeof(RegisterPage) },
        //    { typeof(LoginViewModel), typeof(LoginView) },
        //    { typeof(HomeViewModel), typeof(HomeView) },
        //    { typeof(DashboardViewModel), typeof(DashboardView) },
        //    { typeof(EditItemViewModel), typeof(EditItemView) },
        //    { typeof(ItemsListViewModel), typeof(ItemsListView) },
        //    { typeof(LearnViewModel), typeof(LearnView) },
        //    { typeof(SettingsViewModel), typeof(SettingsView) },
        //    { typeof(UserProfileViewModel), typeof(UserProfileView) }
        //};

        //public static ObservableCollection<Dictionary<Type, Type>> GetViewModelToPageMappingList()
        //{
        //    var items = new ObservableCollection<Dictionary<Type, Type>>();
        //    items.Add(_map);
        //    return items;
        //}


        //public static Type GetPageType(Type viewModelType) => _map.TryGetValue(viewModelType, out var pageType) ? pageType : null;

    }
}
