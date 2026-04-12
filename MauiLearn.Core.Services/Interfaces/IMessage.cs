using System;
using System.Collections.Generic;
using System.Text;
using MauiLearn.Core.Services.Messages;

namespace MauiLearn.Core.Services.Interfaces
{
    public interface IMessage { } //So your MessageService can work with a single abstraction: IMessage

    /*Responsibilities for:
      *  - storing subscriptions
      *  - routing messages
      *  - delivering them safely
      *  - returning tokens for clearnup
      */

    public interface IMessageService
    {
        MessageToken Subscribe<TMessage>(Action<TMessage> handler)
            where TMessage : IMessage;

        void Publish<TMessage>(TMessage message)
            where TMessage : IMessage;
    }

}
