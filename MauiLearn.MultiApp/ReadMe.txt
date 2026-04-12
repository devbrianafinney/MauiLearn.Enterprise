# MauiLearn.MultiApp
-- .xaml styles written on with App.xaml --
MauiLearn.MultiApp
This project should contain:
    - Your MAUI UI
    - Your DI registration
    - Your startup logic
    - Your ViewModels

? What Transfer should NOT contain
    - DbContext
    - DbSets
    - Repository implementations
    - EF Core logic

? What Transfer should do
Register everything:

builder.Services.AddDbContextFactory<CoreDbContext>(options =>
    options.UseSqlite($"Data Source={conn}"));

builder.Services.AddScoped(typeof(IEfRepository<,>), typeof(EfRepository<,>));

??
Then after app.Build():
using var scope = app.Services.CreateScope();
var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<CoreDbContext>>();
using var db = factory.CreateDbContext();
db.Database.EnsureCreated();

/*******Below Prior ReadMe1.txt *********************************/
TO KNOW FROM THE MauiProgram, App:
It is NOT between Build() and Run().
It cannot run there.
Because:
- Shell.Current does not exist until the Shell is instantiated.
- The Shell is instantiated after Build() and during Run().
- Your MauiPage doesn’t exist until after the Shell loads the first route
-!! Shell.Current does not exist until the Shell is instantiated.
-!! The Shell is instantiated after Build() and during Run().
-!! Your MauiPage doesn’t exist until after the Shell loads the first route


Think of it like this:
- Build() = “assemble the app’s parts”
- Run() = “turn the engine on”
- Shell + Pages = “the car actually starts moving”
- GoToAsync = “you can now steer the car”
You cannot steer before the engine is running.


******************************************************************
On MauiProgram.cs:
MauiProgram.CreateMauiApp()
    ↓
builder.Build()  ← creates the DI container + registers Shell + pages + viewmodels
    ↓
new App()        ← your App.xaml.cs constructor runs
    ↓
AppShell created ← AppShell.xaml + AppShell.xaml.cs constructor runs
    ↓
app.Run()        ← framework starts the UI loop
    ↓
First page loads ← Shell loads the initial route
    ↓
Your MauiPage appears
    ↓
Shell.Current.GoToAsync(...) becomes valid   <!-----------------------!!!!!!

!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
1. DI builds services
2. DI constructs AppShell (your constructor runs)
3. DI constructs NavigationService
4. App() constructor runs
5. AppShell is assigned to MainPage
6. Shell.Current becomes non-null
7. Shell visual tree loads
8. Loaded event fires
*************************************************************
CoreEF (EntityFramework)
-Create DbContext, Tracking ENTITY INSTANCES by the Context
-Entities become tracking:
    -returned from a query
    -being added or attached to the context 
-SaveChanges or SaveChangesAsync is called. 
-EF Core detects the changes made and writes them to the database.
-The DbContext instance is disposed

MauiProgram:
builder.Services.AddDbContextFactory<CoreDbContext>(options => {

/data/AppDbContext:  <<-- Keeping this to be named 'outside' only proj with AppDbContext (until determination to change name)
public class CoreDbContext : DbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)

//FIND MORE!! //just a reason to be 'seeing' inside and thinking when sending to CoreEF like GetDates() and knowing namespaces and Entities...just in case
        /*
        options.Extensions.ToFrozenDictionary
        typeof(CoreDbContext) 
        options.ContextType.GetConstructor()
        options.LoadFromXaml
        options.BuildOptionsFragment!!!!!!!!!!!
        */
*************************************************************


/******************BEGIN OLD _readme.txt prior************************/ MauiLearn.MultiApp
This project should contain:
    - Your MAUI UI
    - Your DI registration
    - Your startup logic
    - Your ViewModels

? What Transfer should NOT contain
    - DbContext
    - DbSets
    - Repository implementations
    - EF Core logic

? What Transfer should do
Register everything:

builder.Services.AddDbContextFactory<CoreDbContext>(options =>
    options.UseSqlite($"Data Source={conn}"));

builder.Services.AddScoped(typeof(IEfRepository<,>), typeof(EfRepository<,>));

??
Then after app.Build():
using var scope = app.Services.CreateScope();
var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<CoreDbContext>>();
using var db = factory.CreateDbContext();
db.Database.EnsureCreated();

/*******************END OLD _readme.txt prior************************/

