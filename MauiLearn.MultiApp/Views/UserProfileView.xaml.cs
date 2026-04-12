using MauiLearn.MultiApp.ViewModels;
using Microsoft.Maui.Controls;

namespace MauiLearn.MultiApp.Views;

public partial class UserProfileView : ContentView
{
	public UserProfileView(UserProfileViewModel viewModels)
	{
		InitializeComponent();
		BindingContext = viewModels;
    }
}