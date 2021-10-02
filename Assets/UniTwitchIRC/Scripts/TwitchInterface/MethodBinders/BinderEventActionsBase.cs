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
    public abstract class BinderEventActionsBase<T> : MonoBehaviour where T : MethodBinderBase
    {
        public T[] binder;

        protected virtual void Awake()
        {
            CommandsBehaviour.OnCommandsReceived += CommandsBehaviour_OnCommandsReceived;
        }

        protected virtual void CommandsBehaviour_OnCommandsReceived(TwitchChat twitchChat, CommandsArgs commandsArgs)
        {
            for (int i = 0; i < binder.Length; i++)
            {
                if (commandsArgs.IsCommand(twitchChat.commandSymbol, binder[i].command))
                {
                    InvokeBinderAction(binder[i], commandsArgs);
                }
            }
        }

        protected abstract void InvokeBinderAction(T binder, CommandsArgs commandsArgs);
    }
}