using MauiLearn.Core.Services.Interfaces;

namespace MauiLearn.MultiApp.Services
{
    public sealed class SettingsServices : ISettingsServices
    {
        public string UseMocksDefault
        {
            get => "use_mocks";
        }
        public string IdUseMocks
        {
            get => string.Empty;
            set => Preferences.Set("use_mocks", value);
        }
        public bool UseMocks
        {
            get => Preferences.Get(IdUseMocks, UseMocksDefault) == "true";
            set => Preferences.Set(IdUseMocks, value);
        }
        public string IdIdentityBase
        {
            get => string.Empty;
            set => Preferences.Set("url_base", value);
        }

        public string AccessToken
        {
            get => string.Empty;
            set => Preferences.Set("access_token", value);
        }

        public string? AccessTokenDefault
        {
            get => string.Empty;
        }

        public string? AuthAccessToken
        {
            get => Preferences.Get(AccessToken, AccessTokenDefault);
            set => Preferences.Set(AccessToken, value);
        }
       

        public string? IdentityEndpointBase
        {
            get => Preferences.Get(IdIdentityBase, IdIdentityBase);
            set => Preferences.Set(IdIdentityBase, value);
        }
        public string? AuthIdToken { get; set; }
        public bool UseFakeLocation { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public bool AllowGpsLocation { get; set; }

    }
}
