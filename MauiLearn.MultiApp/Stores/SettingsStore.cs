using System;
using System.Collections.Generic;
using System.Text;

namespace MauiLearn.MultiApp.Stores
{
    /*just for "seeing" with stores right now, 
     * not for actual use*/
    public class SettingsStore
    {
        public string Theme
        {
            get => Preferences.Get(nameof(Theme), "Light");
            set => Preferences.Set(nameof(Theme), value);
        }

        public bool IsFirstRun
        {
            get => Preferences.Get(nameof(IsFirstRun), true);
            set => Preferences.Set(nameof(IsFirstRun), value);
        }
    }

}
