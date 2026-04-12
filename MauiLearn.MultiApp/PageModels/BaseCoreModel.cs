namespace MauiLearn.MultiApp.PageModels;

public class BaseCoreModel : ContentPage
{
	public BaseCoreModel()
	{
		Content = new VerticalStackLayout
		{
			Children = {
				new Label { 
					HorizontalOptions = LayoutOptions.Center, 
					VerticalOptions = LayoutOptions.Center, 
					Text = "Welcome to .NET MAUI!"
				}
			}
		};
	}
}