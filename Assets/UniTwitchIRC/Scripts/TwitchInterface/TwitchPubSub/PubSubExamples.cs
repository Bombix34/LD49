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
using TwitchPubSubAPI;
using TwitchPubSubAPI.Payloads.Response;
using TwitchPubSubAPI.Payloads;

namespace UniTwitchIRC.TwitchInterface.TwitchPubSub
{
    [AddComponentMenu("Scripts/Twitch API Integration/TwitchInterface/TwitchPubSub/PubSubExamples")]
    public class PubSubExamples : MonoBehaviour
    {
        [Header("Example Events")]
        [SerializeField, ReadonlyField(false)]
        bool m_OnTaskException = false;
        [SerializeField, ReadonlyField(false)]
        bool m_OnPayload = false;
        [SerializeField, ReadonlyField(false)]
        bool m_OnResponse = false;
        [SerializeField, ReadonlyField(false)]
        bool m_OnPoints = false;
        [SerializeField, ReadonlyField(false)]
        bool m_OnWhispers = false;
        [SerializeField, ReadonlyField(false)]
        bool m_OnBits = false;
        
        void OnEnable()
        {
            if(m_OnTaskException)
            {
                PubSubClient.OnTaskException -= PubSubClient_OnTaskException;
                PubSubClient.OnTaskException += PubSubClient_OnTaskException;
            }

            if (m_OnPayload)
            {
                PubSubClient.OnPayload -= PubSubClient_OnPayload;
                PubSubClient.OnPayload += PubSubClient_OnPayload;
            }

            if (m_OnResponse)
            {
                PubSubClient.OnResponse -= PubSubClient_OnResponse;
                PubSubClient.OnResponse += PubSubClient_OnResponse;
            }

            if (m_OnPoints)
            {
                PubSubClient.OnPoints -= PubSubClient_OnPoints;
                PubSubClient.OnPoints += PubSubClient_OnPoints;
            }

            if (m_OnWhispers)
            {
                PubSubClient.OnWhispersThread -= PubSubClient_OnWhispersThread;
                PubSubClient.OnWhispersThread += PubSubClient_OnWhispersThread;

                PubSubClient.OnWhispers -= PubSubClient_OnWhispers;
                PubSubClient.OnWhispers += PubSubClient_OnWhispers;
            }

            if (m_OnBits)
            {
                PubSubClient.OnBits -= PubSubClient_OnBits;
                PubSubClient.OnBits += PubSubClient_OnBits;
            }
        }

        void OnDestroy()
        {
            if (m_OnTaskException)
            {
                PubSubClient.OnTaskException -= PubSubClient_OnTaskException;
            }

            if (m_OnPayload)
            {
                PubSubClient.OnPayload -= PubSubClient_OnPayload;
            }

            if (m_OnResponse)
            {
                PubSubClient.OnResponse -= PubSubClient_OnResponse;
            }

            if (m_OnPoints)
            {
                PubSubClient.OnPoints -= PubSubClient_OnPoints;
            }

            if (m_OnWhispers)
            {
                PubSubClient.OnWhispersThread -= PubSubClient_OnWhispersThread;

                PubSubClient.OnWhispers -= PubSubClient_OnWhispers;
            }

            if (m_OnBits)
            {
                PubSubClient.OnBits -= PubSubClient_OnBits;
            }
        }

        void PubSubClient_OnTaskException(TaskExceptionArgs args)
        {
            Debug.Log($"<color=green><b>{args.reason} {args.exception.Message}</b></color>");
        }

        void PubSubClient_OnPayload(Payload obj)
        {
            Debug.Log($"<color=yellow><b>{obj.type}</b></color>");
        }

        void PubSubClient_OnResponse(PayloadResponse obj)
        {
            Debug.Log($"Type: {obj.type} <color=red><b>{obj.error}</b></color>");
        }

        void PubSubClient_OnWhispersThread(Whispers.WhispersThreadData data)
        {
            Debug.Log($"{data.id} {data.spam_info.likelihood}");
        }

        void PubSubClient_OnWhispers(Whispers.WhispersData data)
        {
            Debug.Log($"{data.thread_id} {data.tags.display_name} <color=green><b>{data.body}</b></color>");
        }

        void PubSubClient_OnPoints(Points obj)
        {
            Debug.Log($"{obj.data.redemption.user.display_name} {obj.data.redemption.reward.title}");
        }

        void PubSubClient_OnBits(Bits obj)
        {
            Debug.Log($"{obj.data.user_name} {obj.data.bits_used}");
        }
    }
}
