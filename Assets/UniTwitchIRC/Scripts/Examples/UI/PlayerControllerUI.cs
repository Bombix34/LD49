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
using UniTwitchIRC.Controllers;
using UniTwitchIRC.TwitchInterface;
using UnityEngine;

namespace UniTwitchIRC.Examples.UI
{
    /// <summary>
    /// The viewer UI in the world shows display name and points
    /// </summary>
    public abstract class PlayerControllerUI<T> : MonoBehaviour, IPlayerUI where T : RigidbodyPlayerController
    {
        [SerializeField]
        protected T m_PlayerController = null;

        [SerializeField]
        Vector3 m_Offset = Vector3.up * 2.0f;

        [SerializeField]
        float m_SmoothTime = 1.0f;

        Transform m_Target = null;
        Transform m_Trans = null;
        Vector3 m_CurrentVelocity = Vector3.zero;

        /// <summary>
        /// Access to the UI player controller
        /// </summary>
        public T playerController { set { m_PlayerController = value; } }

        TwitchPlayerController IPlayerUI.playerController { set => playerController = value as T; }

        protected virtual void OnEnable()
        {
            StartCoroutine(OnStartTrack());
        }

        protected virtual void OnDisable() { }

        protected virtual IEnumerator OnStartTrack()
        {
            while (!m_PlayerController)
            {
                yield return new WaitForEndOfFrame();
            }
            m_Trans = transform;
            m_Target = m_PlayerController.transform;

            PrepareUI();

            while (enabled)
            {
                UpdateUI();

                m_Trans.position = Vector3.SmoothDamp(m_Trans.position, m_Target.position, ref m_CurrentVelocity, m_SmoothTime);
                m_Trans.position += m_Offset;
                yield return new WaitForFixedUpdate();
            }
        }

        protected virtual void PrepareUI() { }

        protected virtual void UpdateUI() { }

    }
}
