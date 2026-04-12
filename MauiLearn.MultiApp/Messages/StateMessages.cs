using CommunityToolkit.Mvvm.Messaging.Messages;
using MauiLearn.Core.Models;
using MauiLearn.Core.Services.Interfaces;
using MauiLearn.MultiApp.Models;

namespace MauiLearn.MultiApp.StateMessages
{
    #region Copilot learning with Messages
    /* these are the "types", your "shapes of communication"
     * -simple, immutable, and descriptive as perfect for event-style communication
     * "thinks" of them as records that describe what happened or what needs to happen
     * */
    public sealed record UserLoggedInMessage(int UserId) : IMessage;
    public sealed record RefreshRequestedMessage(string Target) : IMessage;

    //would be great for CancellationToken, but let's try first new today Copilot
    /*  using MauiLearn.Core.Models; == ItemModel
        using MauiLearn.Core.Services.Interfaces; == IMessage && IMessageService
        using MauiLearn.Core.Services.Messages == MessageToken
    */
    //this is MauiLearn.MultiApp.Messages for ItemModelStateChangedMessage
    public sealed record ItemModelStateChangedMessage(ItemModel? Item = null) : IMessage; /*, CancellationToken token = default) : IMessage;*/
    public sealed record AuthProfileStateChangedMessage(AuthProfile? AuthProfile = null) : IMessage;
    public sealed record AppMenuCurrentViewItemChangedMessage(AppMenuItem AppMenuCurrentView) : IMessage; //works NOT with a list - just current item
    /*still wanting to bring in from prior when able to.... */

    #endregion Copilot learning with Messages

    #region Message Tokens
    public partial class MessageTokens
    {
        public static readonly string AuthState = "AuthStateToken";
        public static readonly string AppMenuCurrentView = "AppCurrentMenuToken";
        public static CancellationToken NavigationState = default;
    }
    #endregion


    //#region ChangedMessages
    //public class AuthProfileStateChangedMessage : ValueChangedMessage<AuthProfile?>
    //{
    //    public AuthProfileStateChangedMessage(AuthProfile? value) : base(value) { }
    //}

   
    /*** to SEND ****/
    #region Test Learning
    public class SomethingChangedMessage : ValueChangedMessage<object>
    {
        public SomethingChangedMessage(object value) : base(value) { }
    }

    // Define a message class
    public class LoggedInUserChangedMessage : ValueChangedMessage<string>
    {
        public LoggedInUserChangedMessage(string user) : base(user) { }
    }
    #endregion

}


