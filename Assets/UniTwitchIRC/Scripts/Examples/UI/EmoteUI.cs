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

using System.Collections;
using UniTwitchIRC.TwitchInterface;
using UnityEngine;
using UnityEngine.UI;
using TwitchUnityIRC.Channel.Monitor.Capabilities.Emotes;
using UniTwitchIRC.Controllers;

namespace UniTwitchIRC.Examples.UI
{
    /// <summary>
    /// The player UI in the world shows display name and points
    /// </summary>
    [AddComponentMenu("Scripts/Twitch API Integration/Examples/UI/EmoteUI")]
    public class EmoteUI : PlayerControllerUI<RigidbodyPlayerController>
    {
        [SerializeField]
        RawImage m_Image = null;

        [SerializeField]
        float m_Scalar = 0.02f;

        float m_Scale = 0.01f;

        AnimatedGif m_AnimatedGif = null;

        protected override void PrepareUI()
        {
            m_Image.gameObject.SetActive(false);
        }

        protected override void UpdateUI()
        {
            if (m_Image.texture != null)
            {
                m_Image.rectTransform.localScale = new Vector3(m_Image.texture.width * m_Scalar, m_Image.texture.height * m_Scalar, 0.0f);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            TwitchEmotes.OnEmoteMessage -= TwitchEmotes_OnEmotes;
            TwitchEmotes.OnEmoteMessage += TwitchEmotes_OnEmotes;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            TwitchEmotes.OnEmoteMessage -= TwitchEmotes_OnEmotes;
        }

        void TwitchEmotes_OnEmotes(EmoteMessageArgs arg)
        {
            if (arg.complexTags.displayName.ToLower() == m_PlayerController.nick)
            {
                //m_PlayerController.emoteTexture = EmotesHandler.cachedAnimated[arg.emoteItems[0].messageEmote.id].AnimatedFrames[0].texture2D;// arg.emoteItems[0].GetTexture2D();

                //m_Image.texture = m_PlayerController.emoteTexture;

                m_Image.gameObject.SetActive(true);

                m_AnimatedGif = EmotesHandler.cachedAnimated[arg.emoteItems[0].messageEmote.id];

                StopCoroutine("AnimatedGif");
                StartCoroutine("AnimatedGif");
            }
        }

        IEnumerator AnimatedGif()
        {
            int index = -1;

            while (true)
            {
                index = (index + 1) % m_AnimatedGif.frames.Length;

                m_PlayerController.emoteTexture = m_AnimatedGif.frames[index].texture2D;

                m_Image.texture = m_PlayerController.emoteTexture;
                if (m_AnimatedGif.frames[index].duration > 0)
                {
                    yield return new WaitForSeconds(m_AnimatedGif.frames[index].duration * m_Scale);
                }
                else
                {
                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }
}
