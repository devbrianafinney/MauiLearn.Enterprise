using MauiLearn.Core.Services.Interfaces;

/*********************************************************************************
 *                           ACTIONS 
 * !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
     Summary:
        Encapsulates a method that has no parameters and does not return a value.
**********************************************************************************/

namespace MauiLearn.Core.Services.Messages
{
    /* Tokens are how you unsubscribe cleanly.
    * They’re disposable handles that represent a subscription.
    * - deterministic cleanup
    * - no memory leaks 
    * - no dangline event handlers
    * - no "ghost receivers" firing after a page in gone
    */
public sealed class MessageToken : IDisposable
    {
        private readonly Action _unsubscribe;
        private bool _isDisposed;

        public MessageToken(Action unsubscribe)
        {
            _unsubscribe = unsubscribe;
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
            _unsubscribe();
        }
    }


    /*Responsibilities for:
     *  - storing subscriptions
     *  - routing messages
     *  - delivering them safely
     *  - returning tokens for clearnup
     */

    public sealed class MessageService : IMessageService
    {
        private readonly Dictionary<Type, List<Delegate>> _handlers = new();
     
        public MessageToken Subscribe<TMessage>(Action<TMessage> handler)
            where TMessage : IMessage
        {
            var type = typeof(TMessage);
            var x = handler.Target.ToString();
            if (!_handlers.TryGetValue(type, out var list))
            {
                list = new List<Delegate>();
                _handlers[type] = list;
            }
           
            list.Add(handler);
            return new MessageToken(() => list.Remove(handler));
        }

        public void Publish<TMessage>(TMessage message)
            where TMessage : IMessage
        {
            var type = typeof(TMessage);

            if (!_handlers.TryGetValue(type, out var list))
                return;

            foreach (var handler in list.OfType<Action<TMessage>>())
                handler(message);
        }
    }
}
