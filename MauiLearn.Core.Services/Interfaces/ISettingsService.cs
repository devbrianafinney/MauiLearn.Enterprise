using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace MauiLearn.Core.Services.Interfaces
{
    /* 03/04/2026: This interface defines the contract for a settings service that manages application settings 
     * such as authentication tokens, mock data usage, and location settings.
     * From ideas Enterprise-Application-Patterns-Using-.NET-MAUI -> CHAPTER 8 | Application settings management  
     */
    public interface ISettingsServices
    {
        string? IdentityEndpointBase { get; set; }
        string? AccessToken { get; set; }
        string? IdUseMocks { get; set; }
        string? AuthAccessToken { get; set; }
        bool UseMocks { get; set; }
        string? AuthIdToken { get; set; }
        bool UseFakeLocation { get; set; }
        string? Latitude { get; set; }
        string? Longitude { get; set; }
        bool AllowGpsLocation { get; set; }
    }
}
