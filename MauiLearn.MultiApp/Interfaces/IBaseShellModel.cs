using MauiLearn.Core.Models;
using MauiLearn.MultiApp.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace MauiLearn.MultiApp.Interfaces
{
    /*
     * 2/16/2026:
     * NOT MODELS -> UI Binding Logic! 
     *  -> Domain models do NOT implement UI interfaces
     *  -> UI models do NOT inherit from domain models
     *      
     *    Core.Models
     *        ↓ (pure data)
     *    CoreEF
     *        ↓ (repositories, DbContext)
     *    Core.Services
     *        ↓ (interfaces)
     *    Transfer
     *        ↓ (ViewModels, UI binding)
     *    IBaseShellModel
     */
    /** I think that ObservableObject takes over with all 
     * of this and with CommunityToolkit...
     * make sure and can remove all 
     */
   // [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IBaseShellModel//<T> where T : struct
    {
        //private Dictionary<Type, Type> OnCurrentAuthMapList; //enter list same list with dictionary and can be utilized several, choosing, edit, create with auth as determined
        //private List<AppMenuItem> OnMenuItemsList; //enter list from ViewModelToPageHelper
        //private TabBar OnBaseTabBar;
        //private FlyoutItem OnBaseFlyoutItem;



        [EditorBrowsable(EditorBrowsableState.Never)]
        event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "");
        bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "");
    }
}
