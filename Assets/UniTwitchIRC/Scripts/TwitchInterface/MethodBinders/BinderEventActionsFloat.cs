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

using IRCnect.Channel.Monitor.Replies.Inbounds.Commands;
using UniTwitchIRC.TwitchInterface.MethodBinders.Binders;
using UnityEngine;

namespace UniTwitchIRC.TwitchInterface.MethodBinders
{
    [AddComponentMenu("Scripts/Twitch API Integration/TwitchInterface/MethodBinders/BinderEventActionsFloat", 1)]
    public class BinderEventActionsFloat : BinderEventActionsBase<MethodBinderFloat>
    {
        protected override void InvokeBinderAction(MethodBinderFloat binder, CommandsArgs commandsArgs)
        {
            if (float.TryParse(commandsArgs.argument, out float number))
            {
                binder.action.Invoke(number);
            }
        }
    }
}