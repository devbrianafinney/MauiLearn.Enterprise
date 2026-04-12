using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
/*/3/8/2026 - GO AWAY for bringing more with CommunityToolkit.Mvvm *?*/
namespace MauiLearn.MultiApp.Stores
{
    public partial class ObjectStore : INotifyPropertyChanged
    {
        private bool _isButtonVisible = true;
        private bool _isButtonEnabled = true;

        public bool IsButtonVisible
        {
            get => _isButtonVisible;
            set { _isButtonVisible = value; OnPropertyChanged(); }
        }

        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set { _isButtonEnabled = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

