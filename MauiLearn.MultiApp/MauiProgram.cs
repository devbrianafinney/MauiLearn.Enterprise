using CommunityToolkit.Maui;
using MauiLearn.MultiApp.Services;
using MauiLearn.MultiApp.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
//using Syncfusion.Maui.Core;
using System.Collections.Generic;
using System.Linq; //nada no @now


/* AppShell - decides whice one to show 
 *      MainPage -> host Views for desktop
 *      Pages host isolated screens for desktop (when chose devices & why)
 *  Navigation ? Page ? ViewModel ? Repository ? DbContext ? Database
 *  And DI wires the chain:
 *      Page (via DI)
 *          ?
 *      ViewModel (via DI)
 *          ?
 *      IEfRepository<T,TKey> (via DI)
 *          ?
 *      EfRepository<T,TKey> (via DI)
 *          ?
 *      CoreDbContext (via DI)
 *  
 * extensions:
 * builder.RegisterAppServicesBeforeBuild
 * builder.RegisterHttpAPIs
 * builder.RegisterPages
 * builder.RegisterPageModel
 * builder.RegisterViews
 * builder.RegisterPageModels
 * builder.AddRouterExtensions
 * builder.RegisterAppCoreDb
 * builder.AddCreateDbContext
 * 
 * 
 * ----> MainPage.xaml and MainPage.xaml.cs

    This is the startup page displayed by the app. The MainPage.xaml file defines the 
    UI (user interface) of the page. MainPage.xaml.cs contains the code-behind for the 
    XAML, like code for a button click event.
 */
namespace MauiLearn.MultiApp
{
    /* jic -> xmlns:toolkit="http://schemas.microsoft.com/dotnet/2022/maui/toolkit"
     * builder.UserMauiCommunityToolkit() //for the toolkit controls and behaviors
     * 
     * iOS simulator: use Finder ? ~/Library/Developer/CoreSimulator/Devices/... 
     * or use the simulator’s container tools.
     * for android and windows: FileSystem.AppDataDirectory 
     * #if WINDOWS10_0_17763_0_OR_GREATER */
    public static partial class MauiProgram
    {
       public static MauiApp CreateMauiApp()
        {
          
            var builder = MauiApp.CreateBuilder();
            builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureMauiHandlers(handlers =>
                {
//#if IOS || MACCATALYST
//    				handlers.AddHandler<Microsoft.Maui.Controls.CollectionView, Microsoft.Maui.Controls.Handlers.Items2.CollectionViewHandler2>();
//#endif
                })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
            builder.Logging.AddDebug();
            builder.Services.AddLogging(configure => configure.AddDebug());
#endif

            #region Global Properties
            /******************************************************
            * Global Properties and with from 'record' class (declaring properties and a constructor)
            ******************************************************/
            var initialProps = InitializePropertiesProvider.GetInitialProperties();
            // Example: register the list in DI so other services can consume it
            builder.Services.AddSingleton<List<IInitializeProperties>>(initialProps);
            // returns the Value of the first item whose Key == "ConnectionString"
            var connectionString = initialProps
                .Where(x => x.Key == "ConnectionString")
                .Select(x => x.Value)
                .FirstOrDefault(); // string?

            // build a string dictionary for configuration
            var dict = new Dictionary<string, string?>();

            foreach (var prop in initialProps)
            {
                // use prop directly — no LINQ Select here
                dict[prop.Key] = prop.Value;
            }

            builder.Configuration.AddInMemoryCollection(dict);
            #endregion

            /******************************************************
            * ? Services
               (Repositories, API clients, helpers)
           * ****************************************************/
            builder.RegisterAppServicesBeforeBuild();
            builder.RegisterAppConvertersBeforeBuild(); //3/15/2026

            //builder.RegisterHttpAPIs(); <!--not using yet
            builder.RegisterStores();
            builder.RegisterPages();
            builder.RegisterPageModels();
            builder.RegisterViews();
            builder.RegisterViewModels();
            builder.AddRouterExtensions(); //make the routers
            builder.RegisterAppCoreDb(connectionString);
            
            /* Once Build has been called, the DI container is immutable and can no longer be updated or modified. 
             * ensure the app will still have its lifetime after this has build
             */
            builder.Services.AddSingleton<AuthStateService>();
            var app = builder.Build(); //does not yet "exist"
            // read configuration after build
            var config = app.Services.GetRequiredService<IConfiguration>();

            /*---Prepare for with EF after Build - AND RootDictionary for the lifetime entire */
            app.AddCreateDbContext();
            //ServiceCollectionExtensions.AddCreateDbContext(app);
            
            return app;
        }
    }
}
