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

namespace UniTwitchIRC.TwitchInterface
{
    /// <summary>
    /// Host the basic components required to use UniTwitch IRC
    /// </summary>
    [AddComponentMenu("Scripts/Twitch API Integration/TwitchInterface/TwitchIRC/UniTwitchIRCBehaviours")]
    public class UniTwitchIRCBehaviours : MonoBehaviour
    {
        [System.Obsolete("Discontiued the chat popup support", true)]
        [SerializeField, HideInInspector]
        bool m_OpenOnRun = false;

        /// <summary>
        /// Opens the chat popup when playmode starts
        /// </summary>
        [System.Obsolete("Discontiued the chat popup support", true)]
        public bool openOnRun { get { return m_OpenOnRun; } }

    }
}