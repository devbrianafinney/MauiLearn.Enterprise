using System;
using System.Collections.Generic;
using System.Text;
using MauiLearn.MultiApp.Interfaces;



/* 02/16/2026:  The clean architecture version does NOT use ServiceHelper at all.
Instead, AppShell receives the service through constructor injection:
keeping this class, jic, and should see the comments on the AppShell.xaml.cs
until determine to do and actually remove this one.....
 */
namespace MauiLearn.MultiApp.Services.Helpers
{
    /// <summary>
    /// Helper class to access services from the DI container
    /// </summary>
    public static class ServiceHelper
    {
        /// <summary>
        /// Gets the Navigation Service from the DI container
        /// </summary>
        public static INavigationService? GetNavigationService()
        {
            return ServiceLocator.GetService<INavigationService>();
        }
    }

    /// <summary>
    /// Service Locator for accessing registered services
    /// </summary>
    public static class ServiceLocator
    {
        /// <summary>
        /// Gets a service from the MauiApp service provider
        /// </summary>
        public static T? GetService<T>() where T : class
        {
            var mainPage = Application.Current?.Windows[0].Page;
            if (mainPage != null)
            {
                var handler = mainPage.Handler;
                if (handler != null && handler.MauiContext != null)
                {
                    var services = handler.MauiContext.Services;
                    return services.GetService(typeof(T)) as T;
                }
            }

            return null;
        }
    }
}
