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

using UnityEngine;
using TwitchUnityIRC.Channel.Notifications;
using TwitchUnityIRC.Channel.Monitor.Capabilities.Emotes;
using IRCnect.Channel.Monitor.Capabilities;
using TwitchUnityIRC.Channel.Monitor.Capabilities;
using TwitchUnityIRC.Channel.Monitor.Capabilities.Tags.PRIVMSG;
using System.Linq;
using System;
using TwitchUnityIRC.Channel.Monitor.Capabilities.Emotes.BTTV;
using TwitchUnityIRC.Channel.Monitor.Capabilities.Emotes.FFZ;
using System.Collections;
using System.Collections.Generic;

namespace UniTwitchIRC.TwitchInterface
{
    /// <summary>
    /// This gets and manages emotes from chat messages
    /// </summary>
    [AddComponentMenu("Scripts/Twitch API Integration/TwitchInterface/TwitchIRC/TwitchEmotes")]
    public class TwitchEmotes : MonoBehaviour
    {
        /*
        /// <summary>
        /// 
        /// </summary>
        public readonly static Dictionary<string, Texture2D> cachedTextures = new Dictionary<string, Texture2D>();
        */
        static List<EmoteItem> s_TempEmoteItems = new List<EmoteItem>();
        
        public static event Action<EmoteMessageArgs> OnEmoteMessage;
        
        [SerializeField, ReadonlyField(false)]
        bool m_BTTVEnabled = false;
        
        [SerializeField, ReadonlyField(false)]
        bool m_FFZEnabled = false;

        [Header("Shows when emotes are ready")]
        [SerializeField, ReadonlyField]
        bool m_BTTVReady = false;

        [SerializeField, ReadonlyField]
        bool m_FFZReady = false;

        void Awake()
        {
            BttvEmote.isEnabled = m_BTTVEnabled;
            FfzEmote.isEnabled = m_FFZEnabled;

            EmotesHandler.cachedAnimated = new Dictionary<string, AnimatedGif>();
        }

        IEnumerator Start()
        {
            int amount = 0;
            int count = 0;

            if (m_BTTVEnabled) amount++;
            if (m_FFZEnabled) amount++;

            while (count != amount)
            {
                count = 0;

                m_BTTVReady = BttvEmote.isReady;
                m_FFZReady = FfzEmote.isReady;

                if (BttvEmote.isReady) count++;
                if (FfzEmote.isReady) count++;
                
                yield return null;
            }
        }

        void OnEnable()
        {
            NotificationHandler.OnJoin -= EmotesHandler.InitOnJoin;
            NotificationHandler.OnJoin += EmotesHandler.InitOnJoin;

            CapabilitiesFilter.OnPrivMsg -= CapabilitiesFilter_OnPrivMsg;
            CapabilitiesFilter.OnPrivMsg += CapabilitiesFilter_OnPrivMsg;
        }

        void OnDisable()
        {
            NotificationHandler.OnJoin -= EmotesHandler.InitOnJoin;

            CapabilitiesFilter.OnPrivMsg -= CapabilitiesFilter_OnPrivMsg;
        }
        
        async void CapabilitiesFilter_OnPrivMsg(CapabilitiesArgs obj)
        {
            ComplexTags complexTags = IRCTags.GetTags<ComplexTags>(obj.info);

            s_TempEmoteItems.Clear();
            for (int i = 0; i < complexTags.emotes?.Length; i++)
            {
                s_TempEmoteItems.Add(await EmotesHandler.GetEmote<EmoteItem>(complexTags.emotes[i]));
            }

            if(m_BTTVEnabled && m_BTTVReady)
            {
                List<BttvEmote> bttvEmotes = BttvEmote.ParseBTTV(obj.message, EmotesHandler.responseEmotes);
                for (int i = 0; i < bttvEmotes.Count; i++)
                {
                    s_TempEmoteItems.Add(await EmotesHandler.GetEmote<EmoteItem>(bttvEmotes[i]));
                }
            }

            if(m_FFZEnabled && m_FFZReady)
            {
                List<FfzEmote> ffzEmotes = FfzEmote.ParseFFZ(obj.message, EmotesHandler.responseEmotes);
                for (int i = 0; i < ffzEmotes.Count; i++)
                {
                    s_TempEmoteItems.Add(await EmotesHandler.GetEmote<EmoteItem>(ffzEmotes[i]));
                }
            }

            if (s_TempEmoteItems.Count > 0)
            {
                List<EmoteItem> sortedItems = s_TempEmoteItems.OrderBy(x => x.messageEmote.first).ToList();
                OnEmoteMessage?.Invoke(new EmoteMessageArgs(obj, complexTags, sortedItems));
            }
        }
    }
}
