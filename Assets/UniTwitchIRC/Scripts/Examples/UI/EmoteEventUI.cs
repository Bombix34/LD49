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
using UnityEngine;
using UnityEngine.UI;
using TwitchUnityIRC.Channel.Monitor.Capabilities.Emotes;

namespace UniTwitchIRC.Examples.UI
{
    /// <summary>
    /// The UI to display emotes
    /// </summary>
    [AddComponentMenu("Scripts/Twitch API Integration/Examples/UI/EmoteEventUI")]
    public class EmoteEventUI : MonoBehaviour
    {
        [SerializeField]
        RawImage m_Image = null;
        
        [SerializeField, Tooltip("Image Size Scale")]
        float m_Scalar = 0.02f;
        
        Transform m_Trans = null;

        [SerializeField, Range(0.001f, 0.02f), Tooltip("Animation Delay Scale")]
        float m_DelayScale = 0.01f;

        AnimatedGif m_AnimatedGif = null;

        IEnumerator Start()
        {
            m_Image.gameObject.SetActive(false);

            m_Trans = transform;

            while(enabled)
            {
                if(m_Image.texture != null)
                {
                    m_Image.rectTransform.localScale = new Vector3(m_Image.texture.width * m_Scalar, m_Image.texture.height * m_Scalar, 0.0f);
                }

                yield return new WaitForFixedUpdate();
            }
        }

        public void OnEmotesMessage(EmoteMessageArgs arg, int index)
        {
            m_Image.gameObject.SetActive(true);
            m_Image.texture = EmotesHandler.cachedAnimated[arg.emoteItems[index].messageEmote.id].frames[0].texture2D;
        }

        public void OnEmotesMessage(EmoteMessageArgs arg)
        {
            m_Image.gameObject.SetActive(true);
            
            m_AnimatedGif = EmotesHandler.cachedAnimated[arg.emoteItems[0].messageEmote.id];

            StopCoroutine("AnimatedGif");
            StartCoroutine("AnimatedGif");
        }

        IEnumerator AnimatedGif()
        {
            int index = -1;

            while(true)
            {
                index = (index + 1) % m_AnimatedGif.frames.Length;

                m_Image.texture = m_AnimatedGif.frames[index].texture2D;

                if (m_AnimatedGif.frames[index].duration > 0)
                {
                    yield return new WaitForSeconds(m_AnimatedGif.frames[index].duration * m_DelayScale);
                }
                else
                {
                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }
}
