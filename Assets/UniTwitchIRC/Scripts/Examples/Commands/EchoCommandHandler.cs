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
using UniTwitchIRC.TwitchInterface.Commands;
using UnityEngine;

namespace UniTwitchIRC.Examples.Commands
{
    /// <summary>
    /// Command echos back what was said
    /// <para>an example command</para>
    /// </summary>
    [AddComponentMenu("Scripts/Twitch API Integration/Examples/Commands/EchoCommandHandler")]
    public class EchoCommandHandler : BaseCommandHandler
    {

        /// <summary>
        /// Describe what the command will do
        /// </summary>
        public override string description => "Will echo command parameter back to chat";
        
        /// <summary>
        /// Executed when (echo) command is received
        /// </summary>
        /// <param name="args">Arguments received from the command filter</param>
        public override void OnCommandRecieved(CommandsArgs args)
        {
            m_TwitchChat.SendChatMessage($"{args.nick} said {string.Join(" ", args.nArgument)}");
        }
    }
}