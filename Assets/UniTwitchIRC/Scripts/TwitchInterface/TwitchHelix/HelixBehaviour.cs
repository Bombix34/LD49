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

using PasswordProtector;
using System;
using UnityEngine;
using TwitchHelixAPI;
using System.Threading.Tasks;
using TwitchUnityIRC.SharedHelix;

namespace UniTwitchIRC.TwitchInterface.TwitchHelix
{
    /// <summary>
    /// Provides a connection to the Twitch API information
    /// <para>https://dev.twitch.tv/docs/authentication#registration</para>
    /// </summary>
    [AddComponentMenu("Scripts/Twitch API Integration/TwitchInterface/TwitchHelix/HelixBehaviour")]
    public class HelixBehaviour : HelixAPI
    {
        [SerializeField, ReadonlyField(false)]
        bool m_UseTwitchAPI = false;

        [SerializeField, ToggleField("m_UseTwitchAPI", true)]
        string m_ClientId = string.Empty;
        [SerializeField, PasswordProtect]
        string m_ClientSecret = string.Empty;
        [SerializeField]
        string m_RedirectUrl = string.Empty;
        [SerializeField]
        string[] m_Scopes = null;

        [SerializeField]
        bool m_ForceVerify = false;

        [Header("Debug Info"), SerializeField]
        bool m_ShowDebugLogs = true;

        public override string clientId { get => m_ClientId; }

        public override string clientSecret { get => m_ClientSecret; }

        public override string[] scopes { get => m_Scopes; }

        public static event Action OnAuthorized;

        void Start()
        {
            if (m_UseTwitchAPI)
            {
                ExceptionHelper.ThrowIfEmpty(m_ClientId, string.IsNullOrWhiteSpace, $"The client ID is required, create a Twitch app here {ExceptionHelper.APP_REGISTRATION}");
                ExceptionHelper.ThrowIfEmpty(m_ClientSecret, string.IsNullOrWhiteSpace, $"The client secret is required, create a Twitch app here {ExceptionHelper.APP_REGISTRATION}");
                ExceptionHelper.ThrowIfEmpty(m_RedirectUrl, string.IsNullOrWhiteSpace, $"The redirect url is required, create a Twitch app here {ExceptionHelper.APP_REGISTRATION}");

                ExceptionHelper.ThrowIfEmpty(m_Scopes, value => value.Length == 0, $"At least one scope is required");

                _ = UseExamples();
            }
        }

        async Task UseExamples()
        {
            try
            {
                TokenResponse tokenResponse = DataIO.Load<TokenResponse>(GetTokenPath());
                if (tokenResponse == null || m_ForceVerify == true)
                {
                    DebugLog(" -- Start MakeTokenRequest --", m_ShowDebugLogs);
                    await Authorize();
                }
                else
                {
                    string url = "https://id.twitch.tv/oauth2/validate";
                    Validate response = await Request.GetEndpoint<Validate>(tokenResponse.access_token, m_ClientId, url);
                    if (ArraysValuesEqual(m_Scopes, response.scopes) == false)
                    {
                        DebugLog(" -- Start Scopes Authorized -- ", m_ShowDebugLogs);
                        await Authorize();
                    }
                    else if (Helix.IsExpired(tokenResponse.expires) == true)
                    {
                        DebugLog(" -- Start RefreshToken -- ", m_ShowDebugLogs);
                        await RefreshToken();
                    }
                    else
                    {
                        DebugLog(" -- Start Authorized -- ", m_ShowDebugLogs);
                    }
                }

                OnAuthorized?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError($"{ex.Message}");
            }
        }

        async Task Authorize()
        {
            TokenRequest tokenRequest = new TokenRequest
            {
                clientId = m_ClientId,
                clientSecret = m_ClientSecret,
                redirectUrl = m_RedirectUrl,
                scopes = m_Scopes,
                forceVerify = m_ForceVerify
            };
            string tokenDataPath = GetTokenPath();
            TokenResponse tokenResponse = await Helix.MakeTokenRequest(tokenDataPath, tokenRequest, MakeUnityTokenRequest, OpenBrowser);
            DataIO.Save<TokenResponse>(tokenDataPath, tokenResponse);
        }
    }
}