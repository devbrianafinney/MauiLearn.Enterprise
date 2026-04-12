using CommunityToolkit.Maui.Converters;
using MauiLearn.Core.Models;
using MauiLearn.Core.Services;
using MauiLearn.Core.Services.Interfaces;
using MauiLearn.Core.Services.Messages;
using MauiLearn.Core.Services.Repository;
using MauiLearn.CoreEF;
using MauiLearn.CoreEF.Data;
using MauiLearn.CoreEF.Repository;
using MauiLearn.MultiApp.Converters;
using MauiLearn.MultiApp.Interfaces;
using MauiLearn.MultiApp.PageModels;
using MauiLearn.MultiApp.Pages;
using MauiLearn.MultiApp.Services;
using MauiLearn.MultiApp.Stores;
using MauiLearn.MultiApp.ViewModels;
using MauiLearn.MultiApp.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Hosting;
using MauiDbLocation = Microsoft.Maui.Storage.FileSystem;


/**********************************************************************************
 *   Multi-platform app, the IAppEnvironmentService, IDialogService , 
 *   INavigationService, and ISettingsService interfaces need to be 
 *   'resolved' before it can instantiate a <name>ViewModel[.cs] object.
 *   
 *   This involves the container performing the following actions from 
 *   MainProgram.Extensions.cs to MauiProgram.cs (just to keep it cleaner), 
 *   but is still exactly the WHY w/ MainProgram -- Maui: 
 *      • Deciding how to instantiate an object that implements the interface. 
 *          This is known as --->>  registration. 
 *      • Instantiating the object that implements the required interface and 
 *          the <TName>ViewModel[.cs] object. This is known as --->> resolution.
 * ********************************************************************************/



namespace MauiLearn.MultiApp
{
    /* 2/3/2026: Registration Services
     * MainProgram.CreateMauiApp() -- adds extensions
     * each required: return services;                          <----- REGISTER
     * 
     * builder.RegisterAppServices
     * builder.RegisterHttpAPIs
     * builder.RegisterPages
     * builder.RegisterPageModel
     * builder.RegisterViews
     * builder.RegisterPageModels
     * builder.AddRouterExtensions
     * builder.RegisterAppCoreDb
     * builder.AddCreateDbContext
     * 
     */
    public static partial class ServiceCollectionExtensions
    {
       public static MauiAppBuilder RegisterAppServicesBeforeBuild(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IConfigurationSource>(new MemoryConfigurationSource());
            builder.Services.AddSingleton<ICoreDictionaryService, CoreDictionaryService>();
            //Only register from CoreEf.Data.CoreDbContext
            builder.Services.AddDbContextFactory<CoreDbContext>(); //before builder.Build();
            //remember for namespaces and WHY for this one....later
            //builder.Services.AddSingleton<StringExtensions>();
            /* Default: use EF 
             *      + repository for all TModel/TKey
             */
            builder.Services.AddScoped(typeof(IEfRepository<,>), typeof(EfRepository<,>));
            /*This is the only registration you need.*/
            builder.Services.AddScoped<IAuthService, AuthService>(); /*only for EF */
            /*AddSingleton for entire app */
            builder.Services.AddSingleton<INavigationService, ShellNavigationService>();
            builder.Services.AddSingleton<IAuthStateService, AuthStateService>(); /*only for This app, that will use EF for us not "need those" EF here*/
            /*3/16/26: from Copilot helping with MessageTokens and MessageServices */
            //MauiLearn.Core.Services.Messages
            builder.Services.AddSingleton<IMessageService, MessageService>();

            return builder;
        }

        public static MauiAppBuilder RegisterAppConvertersBeforeBuild(this MauiAppBuilder builder)
        {

            builder.Services.AddScoped<ICommunityToolkitMultiValueConverter, InverseBoolConverter>(); /*only for This app, that will use EF for us not "need those" EF here*/
            return builder;
        }
        /* 
        *  Routing: ONLY if they actually need to physically 'moving' 
        *  like https://localhost:7133/Views/TestView  with creating 
        *  the "name" (string route, Type type) -->
        *  ----  the 'kind of x's types' being worth 'moving' typesI
        */
        //public static IServiceCollection RegisterHttpAPIs(this MauiApp app)
        //{
        //    /* 
        //     * https://learn.microsoft.com/en-us/ef/core/performance/advanced-performance-topics?tabs=with-di%2Cexpression-api-with-constant#dbcontext-pooling
        //     *
        //    <summary>
        //           This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        //           the same compatibility standards as public APIs. It may be changed or removed without notice in
        //           any release. You should only use it directly in your code with extreme caution and knowing that
        //           doing so can result in application failures when updating to a new Entity Framework Core release.
        //      </summary> 
        //      Register AuthService via IAuthService

        //    builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
        //    {
        //        client.BaseAddress = new Uri("https://localhost:7133/");
        //    });


        //    return services;*/
        //}
        public static MauiAppBuilder RegisterStores(this MauiAppBuilder builder)
        {
            //Stores/
            builder.Services.AddSingleton<SettingsStore>();
            builder.Services.AddSingleton<ObjectStore>();
            builder.Services.AddSingleton<SessionStore>();
            return builder;
        }

        public static MauiAppBuilder RegisterPages(this MauiAppBuilder builder)
        {
            //make sure remember why MainPage is not on this...thinking it is because on very beginning...just make sure know "why"!
            builder.Services.AddTransient<LoginView>();
            builder.Services.AddTransient<RegisterNewView>();
            return builder;
        }

      
        /*The [pages] do not inherit from an interface, so they only need their concrete type provided to the 
        * AddSingleton<T> and AddTransient<T> methods.
        *  ---------------------------------------------
        * AddSingletion<T>   Will create a single instance of the object which will be remain for the lifeime of the app
        * AddTransient<T>    Will create a new instance of the object when requested during resolution. Transient objects 
        *                    do not have a pre-defined lifetime, but will typically follow the lifetime of thier host.
        */
        public static MauiAppBuilder RegisterPageModels(this MauiAppBuilder builder)
        {
            /*? maybe better later with this one... */
            builder.Services.AddSingleton<AppShell>();
            /*not from any expectations with CoreEF */
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegisterNewViewModel>();
            return builder;
        }
        public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
        {
            /*not from any expectations with CoreEF */
            builder.Services.AddTransient<BaseCore>();
            builder.Services.AddTransient<HomeView>();
            builder.Services.AddTransient<DashboardView>();
            builder.Services.AddTransient<UserProfileView>();
            
            return builder;
        }
        /*
        * The [view models] do not inherit from an interface, so they only need their concrete type provided to the 
        * AddSingleton<T> and AddTransient<T> methods.
        *  ---------------------------------------------
        * AddSingletion<T>   Will create a single instance of the object which will be remain for the lifeime of the app
        * AddTransient<T>    Will create a new instance of the object when requested during resolution. Transient objects 
        *                    do not have a pre-defined lifetime, but will typically follow the lifetime of thier host.
        */
        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
        {
            /*not from any expectations with CoreEF */
            builder.Services.AddTransient<BaseCoreModel>();
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<UserProfileViewModel>();

            return builder;
        }

        /* come back on this! 
         2/8/26: Register non?Shell routes at startup
         Create an extension to register routes for pages you might navigate to that are 
         not declared in Shell (detail pages, modal pages, etc.).
         2/17/26:  KNOW WHY WHERE --> Shell pages belong in XAML.
                                  --> Conditional pages belong in AppShell.
                                  --> Non?Shell pages belong in Extensions.
                                  --> Just don’t register the same route twice.
         */
        public static MauiAppBuilder AddRouterExtensions(this MauiAppBuilder builder)
        {
           // Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
            Routing.RegisterRoute(nameof(UserProfileView), typeof(UserProfileView));
            Routing.RegisterRoute(nameof(HomeView), typeof(HomeView));
            Routing.RegisterRoute(nameof(DashboardView), typeof(DashboardView));
            Routing.RegisterRoute(nameof(RegisterNewView), typeof(RegisterNewView));
            return builder;
        }
        /* ensure BEFORE app.Build() */
        public static MauiAppBuilder RegisterAppCoreDb(this MauiAppBuilder builder, string? connectionString = null)
        {
            var AppCoreDbName = builder.Configuration["AppCoreDb"]; //AppCoreDb - mauilearn.db3
            var connString = builder.Configuration["ConnectionString"];
            // Recommended: AddDbContextFactory for MAUI (thread-safe, safe for viewmodels/background work)
            builder.Services.AddDbContextFactory<CoreDbContext>(options =>
                options.UseSqlite($"Data Source={connectionString}"));

//#if DEBUG//mauilearn.db3-shm & mauilearn.db3-wal --- 3/16/2026 : JUST KEEPING FROM DELETING EVERY TIME RIGHT NOW
//            var dbPath = Path.Combine(MauiDbLocation.AppDataDirectory, "mauilearn.db3");
//            var wal = Path.Combine(MauiDbLocation.AppDataDirectory, connString?.Replace(".db3", ".db3-wal"));
//            var shm = Path.Combine(MauiDbLocation.AppDataDirectory, connString.Replace(".db3", ".db3-shm"));
//            File.Delete(dbPath); //if need 
//            File.Delete(wal);
//            File.Delete(shm);
//#endif

            return builder;
        }
        /* Good'ens w/ Debugs, dotnet, CoreEF, databases info: 
         * https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=vs
         * dotnet ef migrations add InitialCreate           / Add-Migration InitialCreate
         * dotnet ef database update                        / Update-Database
         * dotnet ef migrations add AddBlogCreatedTimestamp / Add-Migration AddBlogCreatedTimestamp
         * dotnet ef migrations script                                                        / begin to "now"
         * dotnet ef migrations script --idempotent         / Script-Migration -Idempotent    / only scripts
         * dotnet ef migrations bundle                      / Bundle-Migration
         */

        /*
         *  File.Delete(dbPath);
            File.Delete(dbPath + "-wal");
            File.Delete(dbPath + "-shm");
        REMEMBER THIS BEFORE Build() when debug is going on #if:
        #if DEBUG
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "mauilearn.db3");
        File.Delete(dbPath);
        File.Delete(dbPath + "-wal");
        File.Delete(dbPath + "-shm");
        #endif

        THIS HAS BEEN WORKING AS EXACTLY HAS BEEN WORKING FOR MANY WEEKS PRIOR, Finally decided going on and moving on
        feeling 'far, far, away' with this...of course, until now. this pc and of course, the location for connectionString 
        has obviously not the same location it has all before this 'newest' pc anyhow. What's fucking day any mroe these 
        last years? Ya, fuck off and deal with it; so.now............ just comment it out, go back to the original exactly
        WHY that not deleted because of how much gd trouble it becomes just for that. So, let's just go up somewhere and 
        delete on purpose 'just in case' (UGH!!!!!!!!!!!!!!)
        */
        public static MauiApp AddCreateDbContext(this MauiApp app)
        {
            using var scope = app.Services.CreateScope();
            var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<CoreDbContext>>();

            using var db = factory.CreateDbContext();
        #if DEBUG   
           // db.Database.EnsureDeleted(); //does db3, but not delete with -wal and -shm
            db.Database.EnsureCreated(); // dev convenience; switch to Migrate() for production//File.Delete(dbPath);/File.Delete(dbPath + ".wal");/File.Delete(dbPath + ".shm");
        #else
                db.Database.Migrate();  // switch to Migrate() for production KNOW WHEN WHAT for sure
        #endif
            db.Database.GenerateCreateScript();
            return app;
        }
    }
}
