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
using UnityEngine;

namespace UniTwitchIRC.TwitchInterface
{
    /// <summary>
    /// Allows the viewer to control the player from the Twitch chat room
    /// </summary>
    public abstract class TwitchPlayerController : MonoBehaviour
    {
        /// <summary>
        /// Caches the transform start world position in the Awake method on start up
        /// </summary>
        protected Vector3 m_StartPosition = Vector3.zero;

        /// <summary>
        /// Gets/Sets the name of the player for this controller
        /// </summary>
        public string nick { get; private set; }

        /// <summary>
        /// Gets the player name to be displayed in your application
        /// <para>This is different from the nick in that it can be used to provide a different name for the viewer.</para>
        /// </summary>
        public string displayName { get; protected set; }

        public Texture2D emoteTexture { get; set; }

        /// <summary>
        /// <para>Override in derived classes to add functionality</para>
        /// </summary>
        /// <param name="nick">The nick for the player controller</param>
        /// <param name="startPosition">Required start position</param>
        public virtual void StartNew(string nick, Vector3 startPosition)
        {
            m_StartPosition = startPosition;
            this.nick = nick;
            this.displayName = nick;
        }

        public virtual void OnDisplayNameChange(TwitchChat twitchChat, string nick, string command, string[] nArgument) { }

        /// <summary>
        /// Handles the on move request from the Twitch chat room
        /// </summary>
        /// <param name="twitchChat">Twitch Chat object</param>
        /// <param name="commandsArgs">The arguments received</param>
        public virtual void OnMove(TwitchChat twitchChat, CommandsArgs commandsArgs) { }
        
        /// <summary>
        /// Handles the on reset request from the Twitch chat room
        /// </summary>
        /// <param name="twitchChat">Twitch Chat object</param>
        /// <param name="commandsArgs">The arguments recieved</param>
        public virtual void OnReset(TwitchChat twitchChat, CommandsArgs commandsArgs) { }
    }
}
