using MauiLearn.MultiApp.PageModels;

namespace MauiLearn.MultiApp.Pages;

public partial class BaseCore : ContentPage
{
	public BaseCore(BaseCoreModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}