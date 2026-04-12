using MauiLearn.MultiApp.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiLearn.MultiApp.Models
{
    /* 216/2026 ::: NO GOOD! ALL DONE ON THIS...
     *  
     * 2/10/26: This isn't 'perfect' or 'already' what it is - but --- right now it 
     * is simply to create to know that this model, is for a user, that is knowing it
     * is from an app -- some device -- from this app. 
     * So, knowing that this EF is going to be basing on it, regardless of any 'to think is'
     * this app of 'apps' will be wanting data, and that, in the future, this 'user', may
     * be actually an employee, or whatever other type of 'user' so that is a base of on the 
     * Models project, just like needing with CoreEF project - so, just to ensuring to 
     * remember exactly 'why' to then thinking of actually properties to be determined as this
     * app even begins to have a reason of 'why'. Right now, it is just a solid of base to
     * begin growing from the base and the future 
    public class AppUserProfile : IBaseShellModel<UserProfile>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            throw new NotImplementedException();
        }

        public bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            throw new NotImplementedException();
        }
    }*/
}
