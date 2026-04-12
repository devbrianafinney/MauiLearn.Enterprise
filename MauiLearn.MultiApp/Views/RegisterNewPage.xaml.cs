using MauiLearn.MultiApp.ViewModels;

namespace MauiLearn.MultiApp.Pages;

public partial class RegisterNewView : ContentPage
{
	public RegisterNewView(RegisterNewViewModel pm)
	{
		InitializeComponent();
		BindingContext = pm;
    }
}