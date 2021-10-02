using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitchHelixAPI;
using TwitchHelixAPI.Payloads.Response;
using TwitchPubSubAPI;
using UniTwitchIRC.TwitchInterface.TwitchHelix;
using UnityEngine;

namespace UniTwitchIRC.TwitchInterface.TwitchPubSub
{
    [AddComponentMenu("Scripts/Twitch API Integration/TwitchInterface/TwitchPubSub/PubSubBehaviour")]
    public class PubSubBehaviour : MonoBehaviour
    {
        [SerializeField, RequiredInHierarchy(typeof(TwitchChat))]
        TwitchChat m_TwitchChat = null;

        [SerializeField]
        string m_Host = PubSubClient.HOST;

        [SerializeField]
        bool m_ChannelBitsEventsV1 = false;
        [SerializeField]
        bool m_ChannelBitsEventsV2 = false;
        [SerializeField]
        bool m_ChannelBitsBadgeUnlocks = false;
        [SerializeField]
        bool m_ChannelPointsChannelV1 = false;
        [SerializeField]
        bool m_ChannelSubscribeEventsV1 = false;
        [SerializeField]
        bool m_ChatModeratorActions = false;
        [SerializeField]
        bool m_Whisper = false;
        
        [Header("Debug Info"), SerializeField]
        bool m_ShowDebugLogs = false;

        public bool channelBitsEventsV1 { get => m_ChannelBitsEventsV1; }
        public bool channelBitsEventsV2 { get => m_ChannelBitsEventsV2; }
        public bool channelBitsBadgeUnlocks { get => m_ChannelBitsBadgeUnlocks; }
        public bool channelPointsChannelV1 { get => m_ChannelPointsChannelV1; }
        public bool channelSubscribeEventsV1 { get => m_ChannelSubscribeEventsV1; }
        public bool chatModeratorActions { get => m_ChatModeratorActions; }
        public bool whisper { get => m_Whisper; }

        void DebugLog(string value)
        {
            if (m_ShowDebugLogs)
            {
                Debug.Log(value);
            }
        }
        
        async Task StartPubSub()
        {
            Validate validate = await HelixBehaviour.ValidatedData();

            PubSubClient.ValidateFromAuthScopes(validate.scopes);

            string url = $"https://api.twitch.tv/helix/users?login={m_TwitchChat.messenger.channel}";

            GetUsers getUsers = await HelixBehaviour.GetEndpoint<GetUsers>(url);
            
            try
            {
                string channelId = getUsers.data[0].id;

                List<string> topics = new List<string>();
                if (m_ChannelBitsEventsV1) topics.Add($"{Authentication.PubSub.Topics.CHANNEL_BITS_EVENTS_V1}.{channelId}");
                if (m_ChannelBitsEventsV2) topics.Add($"{Authentication.PubSub.Topics.CHANNEL_BITS_EVENTS_V2}.{channelId}");
                if (m_ChannelBitsBadgeUnlocks) topics.Add($"{Authentication.PubSub.Topics.CHANNEL_BITS_BADGE_UNLOCKS}.{channelId}");
                if (m_ChannelPointsChannelV1) topics.Add($"{Authentication.PubSub.Topics.CHANNEL_POINTS_CHANNEL_V1}.{channelId}");
                if (m_ChannelSubscribeEventsV1) topics.Add($"{Authentication.PubSub.Topics.CHANNEL_SUBSCRIBE_EVENTS_V1}.{channelId}");
                if (m_ChatModeratorActions) topics.Add($"{Authentication.PubSub.Topics.CHAT_MODERATOR_ACTIONS}.{channelId}");
                if (m_Whisper) topics.Add($"{Authentication.PubSub.Topics.WHISPER}.{channelId}");

                TokenResponse tokenResponse = DataIO.Load<TokenResponse>(HelixBehaviour.GetTokenPath());



                _ = PubSubClient.Start(m_Host, tokenResponse.access_token, topics.ToArray());

                DebugLog("PubSubClient State: " + PubSubClient.GetState);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }
        
        void OnEnable()
        {
            HelixBehaviour.OnAuthorized -= HelixBehaviour_OnAuthorized;
            HelixBehaviour.OnAuthorized += HelixBehaviour_OnAuthorized;

            PubSubClient.OnReconnectRequired -= PubSubClient_OnReconnectRequired;
            PubSubClient.OnReconnectRequired += PubSubClient_OnReconnectRequired;
        }

        void OnDestroy()
        {
            HelixBehaviour.OnAuthorized -= HelixBehaviour_OnAuthorized;

            PubSubClient.OnReconnectRequired -= PubSubClient_OnReconnectRequired;
        }

        void HelixBehaviour_OnAuthorized()
        {
            _ = StartPubSub();
        }

        void PubSubClient_OnReconnectRequired(ReconnectReason obj)
        {
            DebugLog($"<color=blue><b>{obj}</b></color>");
            _ = StartPubSub();
        }
    }
}