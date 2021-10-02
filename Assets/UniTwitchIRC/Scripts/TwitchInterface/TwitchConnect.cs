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

using FetchHttpRequest;
using PasswordProtector;
using System;
using System.Collections;
using TwitchConnectTv;
using TwitchUnityIRC.Channel.Notifications;
using UnityEngine;
using System.Collections.Generic;

namespace UniTwitchIRC.TwitchInterface
{
    /// <summary>
    /// Provides a connection to the Twitch API and TMI information
    /// <para>This class polls for the viewers in the chat room at the present time</para>
    /// </summary>
    [AddComponentMenu("Scripts/Twitch API Integration/TwitchInterface/TwitchIRC/TwitchConnect")]
    public class TwitchConnect : MonoBehaviour
    {
        const string JSON_EXECPTION_MESSAGE = "<color=red>Exception</color>: {0}\n<color=green>Trying to read jsonn again from stream in {1} seconds.</color>";

        /// <summary>
        /// Event invoked when the JSON data has been refreshed or reloaded
        /// </summary>
        [System.Obsolete("Use OnRefreshedPolling for event subscription", false)]
        public static event Action<TwitchChat, TwitchTv.TMI> OnRefreshed;
        public static event Action<EventArgs> OnRefreshedPolling;

        public class RefreshArgs<T> : EventArgs
        {
            public readonly TwitchChat twitchChat;
            public readonly T result;
            public RefreshArgs(TwitchChat twitchChat, T result)
            {
                this.twitchChat = twitchChat;
                this.result = result;
            }
        }

        /*
        /// <summary>
        /// Event invoked when the JSON data has been refreshed or reloaded
        /// </summary>
        public static event Action<TwitchChat, TwitchTv.API.Followers> OnRefreshedFollows;
        */

        [SerializeField, Tooltip("This is the client id for connection to Twitch API.")]
        string m_ClientId = string.Empty;

        [SerializeField, RequiredInHierarchy(typeof(TwitchChat))]
        TwitchChat m_TwitchIRC = null;

        [SerializeField, Tooltip("Delay between the polling for chat room information.")]
        float m_RefreshDelay = 60.0f;

        [SerializeField, Tooltip("Collection of present chat viewers.")]
        TwitchTv.TMI.Chatters m_Chatters = null;

        [SerializeField, Tooltip("Amount of recent followers to retrieve.")]
        int m_FollowsLimit = 25;

        [SerializeField, Tooltip("Collection of recent stream followers.")]
        TwitchTv.API.Followers.Follows[] m_Follows = null;

        /// <summary>
        /// Collection of present chat viewers.
        /// </summary>
        public TwitchTv.TMI.Chatters chatters { get { return m_Chatters; } }

        public Broadcaster broadcaster { get; private set; } = null;

        void OnEnable()
        {
            NotificationHandler.OnJoin -= NotificationHandler_OnJoin;
            NotificationHandler.OnJoin += NotificationHandler_OnJoin;
        }

        void OnDisable()
        {
            NotificationHandler.OnJoin -= NotificationHandler_OnJoin;
        }

        void NotificationHandler_OnJoin(RoomStateArgs obj)
        {
            Debug.LogFormat("{0}", obj.complexTags);

            ExceptionHelper.ThrowIfEmpty(m_ClientId, string.IsNullOrWhiteSpace, $"The client ID is required, create a Twitch app here {ExceptionHelper.APP_REGISTRATION}");

            broadcaster = new Broadcaster(m_TwitchIRC.messenger.channel, m_ClientId, obj.complexTags.roomId);

            StartCoroutine(EndpointPolling<TwitchTv.TMI>(string.Format(TwitchURI.UriFormats.CHATTERS, broadcaster.channel), m_RefreshDelay, (result) =>
            {
                m_Chatters = result.chatters;
                OnRefreshedPolling?.Invoke(new RefreshArgs<TwitchTv.TMI>(m_TwitchIRC, result));
                OnRefreshed?.Invoke(m_TwitchIRC, result);
            }));

            string url = string.Format(TwitchURI.UriFormats.FOLLOWS, broadcaster.channelId);
            StartCoroutine(EndpointPolling<TwitchTv.API.Followers>($"{url}?limit={m_FollowsLimit}", broadcaster.url.clientId, m_RefreshDelay, (result) =>
            {
                m_Follows = result.follows;
                OnRefreshedPolling?.Invoke(new RefreshArgs<TwitchTv.API.Followers>(m_TwitchIRC, result));
            }));
        }

        static IEnumerator EndpointPolling<T>(string url, string clientId, float refreshDelay, Action<T> callback)
        {
            while (true)
            {
                IEnumerator request = UnityFetch.GetRequestEnumerator<T>(url, new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>("Accept",  "application/vnd.twitchtv.v5+json"),
                    new KeyValuePair<string, string>("Client-ID",  clientId)
                });
                yield return request;
                try
                {
                    callback.Invoke((T)request.Current);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"Retrying endpoint polling {url} {ex.Message}");
                }

                yield return new WaitForSeconds(refreshDelay);
            }
        }

        static IEnumerator EndpointPolling<T>(string url, float refreshDelay, Action<T> callback)
        {
            while (true)
            {

                IEnumerator request = UnityFetch.GetRequestEnumerator<T>(url);
                
                yield return request;

                try
                {
                    callback.Invoke((T)request.Current);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"Retrying endpoint polling {url} {ex.Message}");
                }

                yield return new WaitForSeconds(refreshDelay);
            }
        }
    }
}
