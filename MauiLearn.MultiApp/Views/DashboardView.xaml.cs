using MauiLearn.MultiApp.ViewModels;

namespace MauiLearn.MultiApp.Views;

public partial class DashboardView : ContentView
{
    public DashboardView(DashboardViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}