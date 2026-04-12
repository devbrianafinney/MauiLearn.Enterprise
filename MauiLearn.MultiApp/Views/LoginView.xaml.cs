using MauiLearn.MultiApp.ViewModels;

namespace MauiLearn.MultiApp.Views;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;		
    }
}