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

using UniTwitchIRC.Controllers;
using UniTwitchIRC.TwitchInterface;
using UnityEngine;
using UnityEngine.UI;

namespace UniTwitchIRC.Examples.UI
{
    /// <summary>
    /// The player UI in the world shows display name and points
    /// </summary>
    [AddComponentMenu("Scripts/Twitch API Integration/Examples/UI/PlayerUI")]
    public class PlayerUI : PlayerControllerUI<RigidbodyPlayerController>
    {
        [SerializeField]
        Text m_TextNick = null;

        [SerializeField]
        Text m_TextPoints = null;

        string m_Format = string.Empty;

        protected override void PrepareUI()
        {
            m_Format = m_TextPoints.text;
        }

        protected override void UpdateUI()
        {
            m_TextNick.text = m_PlayerController.displayName;
            m_TextPoints.text = string.Format(m_Format, m_PlayerController.points);
        }
    }
}
