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

using System;
using UniTwitchIRC.TwitchInterface;

namespace UniTwitchIRC.TwitchInterface.MethodBinders.Binders
{
    [Serializable]
    public abstract class MethodBinderBase
    {
        [TwitchCommand]
        public string command;
    }
}