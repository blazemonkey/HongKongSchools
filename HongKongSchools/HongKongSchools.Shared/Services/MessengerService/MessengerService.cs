using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight.Messaging;

namespace HongKongSchools.Services.MessengerService
{
    public class MessengerService : IMessengerService
    {
        public void Register<T>(object recipient, object token, Action<T> action)
        {
            Messenger.Default.Register<T>(recipient, token, action);
        }

        public void Unregister<T>(object recipient, object token, Action<T> action)
        {
            Messenger.Default.Unregister<T>(recipient, token, action);
        }

        public void Send<T>(T message, object token)
        {
            Messenger.Default.Send<T>(message, token);
        }
    }
}
