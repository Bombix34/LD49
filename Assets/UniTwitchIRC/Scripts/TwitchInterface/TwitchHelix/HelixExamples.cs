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
using System.Threading.Tasks;
using Newtonsoft.Json;
using TwitchHelixAPI.Payloads.Response;
using TwitchHelixAPI;
using TwitchUnityIRC.SharedHelix;

namespace UniTwitchIRC.TwitchInterface.TwitchHelix
{
    [AddComponentMenu("Scripts/Twitch API Integration/TwitchInterface/TwitchHelix/HelixExamples")]
    public class HelixExamples : MonoBehaviour
    {

        [SerializeField, RequiredInHierarchy(typeof(TwitchChat))]
        TwitchChat m_TwitchChat = null;

        [Header("Example Endpoint Requests")]
        [SerializeField, ReadonlyField(false)]
        bool m_CreateStreamMarker = false;
        [SerializeField, ReadonlyField(false)]
        bool m_CreateClip = false;
        [SerializeField, ReadonlyField(false)]
        bool m_FollowChannel = false;
        [SerializeField, ReadonlyField(false)]
        bool m_UnFollowChannel = false;
        [SerializeField, ToggleField(new[] { "m_FollowChannel", "m_UnFollowChannel" }, true), ReadonlyField(false), Tooltip("Other user to follow and unfollow channel")]
        string m_OtherUsername = "woLLac";
        [SerializeField, ReadonlyField(false)]
        bool m_PutStreamTags = false;

        [SerializeField, ReadonlyField(false)]
        bool m_ModifyChannelInformation = false;
        [SerializeField, ToggleField(new[] { "m_ModifyChannelInformation" }), ReadonlyField(false)]
        string m_ChannelTitle = "Unity Asset Dev C# for Twitch Integration";
        
        void OnEnable()
        {
            HelixBehaviour.OnAuthorized -= ExamplesBehaviour_OnAuthorized;
            HelixBehaviour.OnAuthorized += ExamplesBehaviour_OnAuthorized;
        }

        void OnDestroy()
        {
            HelixBehaviour.OnAuthorized -= ExamplesBehaviour_OnAuthorized;
        }

        async void ExamplesBehaviour_OnAuthorized()
        {

            //Debug.Log("Twitch username --->");

            string url = $"https://api.twitch.tv/helix/users?login={m_TwitchChat.messenger.channel}&login={m_OtherUsername}";

            GetUsers users = await HelixBehaviour.GetEndpoint<GetUsers>(url);

            //Debug.Log($"user login: {username} <--- Twitch usersResponse.data[0].login");

            if(m_CreateStreamMarker)
            {
                // Post<T>(url, dataString)
                await CreateStreamMarkerExample(users.data[0].id);
            }

            if (m_CreateClip)
            {
                // Post<T>(url, string.Empty)
                await CreateClipExample(users.data[0].id);
            }

            if (m_FollowChannel)
            {
                // Post
                await FollowChannelExample(users.data[0].id, users.data[1].id);
            }

            if (m_UnFollowChannel)
            {
                // Delete
                await UnFollowChannelExample(users.data[0].id, users.data[1].id);
            }

            if (m_PutStreamTags)
            {
                // Put
                await PutStreamTagsExample(users.data[0].id);
            }

            if (m_ModifyChannelInformation)
            {
                // Patch
                await ModifyChannelInformationExample(users.data[0].id, m_ChannelTitle);
            }

        }

        static async Task ModifyChannelInformationExample(string broadcasterId, string title)
        {
            string url = Authentication.Endpoints.MODIFY_CHANNEL_INFORMATION + $"?broadcaster_id={broadcasterId}";
            string dataString = JsonConvert.SerializeObject(new { title });
            long modifyChannelInformation = await PatchEndpoint(url, dataString);
            Debug.Log($"ModifyChannelInformation: {dataString} {modifyChannelInformation} <--- ModifyChannelInformation");
        }

        /// <summary>
        /// Makes a Patch UnityWebRequest
        /// </summary>
        /// <param name="url">A fully qualified url endpoint</param>
        /// <param name="dataString">A json string {username: 'My Name'}</param>
        /// <returns>An http status code</returns>
        public static async Task<long> PatchEndpoint(string url, string dataString)
        {
            //var str = Authentication.GetAssociatedScope(url, Method.PATCH);

            HelixAPI.ValidateScope(url, Methods.PATCH);
            TokenResponse tokenResponse = DataIO.Load<TokenResponse>(HelixAPI.GetTokenPath());
            bool isValid = await HelixAPI.IsValidated();
            if (isValid == false) return 401;

            return await Request.PatchEndpoint(tokenResponse.access_token, HelixAPI.instance.clientId, url, dataString);
        }

        static async Task CreateStreamMarkerExample(string user_id)
        {
            string url = Authentication.Endpoints.CREATE_STREAM_MARKER;
            string dataString = JsonConvert.SerializeObject(new { user_id });
            CreateStreamMarker createStreamMarker = await HelixBehaviour.PostEndpoint<CreateStreamMarker>(url, dataString);
            Debug.Log($"CreateStreamMarker: {dataString} {createStreamMarker.data[0].created_at} <--- CreateStreamMarker");
        }

        static async Task CreateClipExample(string broadcasterId)
        {
            string url = $"{Authentication.Endpoints.CREATE_CLIP}?broadcaster_id={broadcasterId}";
            CreateClip createClip = await HelixBehaviour.PostEndpoint<CreateClip>(url);
            Debug.Log($"createClip: {createClip.data[0].edit_url} <--- createClip");
        }

        static async Task FollowChannelExample(string fromId, string toId)
        {
            string url = $"{Authentication.Endpoints.CREATE_USER_FOLLOWS}?to_id={toId}&from_id={fromId}";
            long followsResponse = await HelixBehaviour.PostEndpoint(url, string.Empty);
            Debug.Log($"followsResponse: {followsResponse} <--- followsResponse");
        }

        static async Task UnFollowChannelExample(string fromId, string toId)
        {
            string url = $"{Authentication.Endpoints.DELETE_USER_FOLLOWS}?from_id={fromId}&to_id={toId}";
            long unfollowResponseCode = await HelixBehaviour.DeleteEndpoint(url);
            Debug.Log($"unfollowResponse: {unfollowResponseCode} <--- unfollowResponse");
        }

        static async Task PutStreamTagsExample(string broadcasterId)
        {
            string[] tag_ids = new[]
            {
                "621fb5bf-5498-4d8f-b4ac-db4d40d401bf", "79977fb9-f106-4a87-a386-f1b0f99783dd"
            };
            string dataString = JsonConvert.SerializeObject(new { tag_ids });
            string url = $"{Authentication.Endpoints.REPLACE_STREAM_TAGS}?broadcaster_id={broadcasterId}";
            long streamTagsResponse = await HelixBehaviour.PutEndpoint(url, dataString);
            Debug.Log($"Stream Tags: {dataString} <--- Stream Tags");
        }
    }
}
