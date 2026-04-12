using MauiLearn.MultiApp.Models;
using MauiLearn.MultiApp.Pages;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MauiLearn.MultiApp.PageModels;
using MauiLearn.MultiApp.ViewModels;

namespace MauiLearn.MultiApp.Interfaces
{
    public interface INavigationService
    {
        /* 3/30/2026 - Type-Safe Navigation with TViewModel */
        Task NavigateToAsync<TViewModel>() where TViewModel : BaseCoreModel;
        Task GoBackAsync(CancellationToken? token = default);
        /* Prior */
       //Task GoToAsync(string route, CancellationToken? token = default);
        //Task GoToAsync(string route, IDictionary<string, object?> parameters = default, CancellationToken? token = default);
        Task GoToAsync<TPage>(CancellationToken? token = default) where TPage : Page;
        //Task GoToAsync<TPage>(IDictionary<string, object?> parameters = default, CancellationToken? token = default) where TPage : Page;
    }


}
