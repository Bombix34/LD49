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

using TwitchUnityIRC.Channel.Monitor.Capabilities.Emotes;
using UnityEngine;

namespace UniTwitchIRC.TwitchInterface.MonitorEvents
{
    /// <summary>
    /// Component to monitor String types of events received from the client
    /// <para>Component based event handlers using UnityEvent for inspector access</para>
    /// </summary>
    [AddComponentMenu("Scripts/Twitch API Integration/TwitchInterface/MonitorEvents/MonitorEmoteEvents")]
    public class MonitorEmoteEvents : MonoBehaviour
    {
        [SerializeField, Tooltip("Sends texture information.")]
        MonitorEmote m_MonitorEmote = null;
        
        [SerializeField, Tooltip("Sends full emote message and detailed information.")]
        MonitorEmoteMessage m_MonitorEmoteMessage = null;

        /// <summary>
        /// Sends full emote message and detailed information.
        /// </summary>
        public MonitorEmoteMessage onEmoteMessage { get { return m_MonitorEmoteMessage; } }

        void OnEnable()
        {
            TwitchEmotes.OnEmoteMessage -= TwitchEmotes_OnEmoteMessage;
            TwitchEmotes.OnEmoteMessage += TwitchEmotes_OnEmoteMessage;
        }

        void OnDisable()
        {
            TwitchEmotes.OnEmoteMessage -= TwitchEmotes_OnEmoteMessage;
        }

        void TwitchEmotes_OnEmoteMessage(EmoteMessageArgs obj)
        {
            m_MonitorEmoteMessage.Invoke(obj);
            m_MonitorEmote.Invoke(EmotesHandler.cachedAnimated[obj.emoteItems[0].messageEmote.id]);
        }
    }
}
