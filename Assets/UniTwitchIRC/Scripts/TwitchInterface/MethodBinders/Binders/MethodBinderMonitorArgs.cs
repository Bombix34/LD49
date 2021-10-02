#region Author
/*
     
     Jones St. Lewis Cropper (caLLow)
     
     Another caLLowCreation
     
     Visit us on Google+ and other social media outlets @caLLowCreation
     
     Thanks for using our product.
     
     Send questions/comments/concerns/requests to 
      e-mail: caLLowCreation@gmail.com
      subject: UniTwirchIRC
     
*/
#endregion

using IRCnect.Channel.Monitor;
using System;
using UnityEngine.Events;

namespace UniTwitchIRC.TwitchInterface.MethodBinders.Binders
{
    [Serializable]
    public class MethodBinderMonitorArgs : MethodBinderBase
    {
        [Serializable]
        public class BindEventsMonitorArgs : UnityEvent<MonitorArgs> { }

        public BindEventsMonitorArgs action;
    }
}