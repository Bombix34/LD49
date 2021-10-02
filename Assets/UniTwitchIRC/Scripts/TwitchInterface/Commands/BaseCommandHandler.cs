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

namespace UniTwitchIRC.TwitchInterface.Commands
{
    /// <summary>
    /// Use to create derived commands
    /// </summary>
    public abstract class BaseCommandHandler
    {

        protected TwitchChat m_TwitchChat = null;

        /// <summary>
        /// Describe what the command will do
        /// </summary>
        public virtual string description { get; } = string.Empty;

        /// <summary>
        /// Happens when the command is instantiated
        /// </summary>
        public virtual void SetUp(TwitchChat twitchChat)
        {
            m_TwitchChat = twitchChat;
        }

        /// <summary>
        /// Clean up here
        /// </summary>
        public virtual void ShutDown() { }

        /// <summary>
        /// Executed when command is received
        /// </summary>
        /// <param name="args">Arguments received from the command filter</param>
        public abstract void OnCommandRecieved(CommandsArgs args);
    }
}