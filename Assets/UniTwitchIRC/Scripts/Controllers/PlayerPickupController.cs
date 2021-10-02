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

using System;
using UnityEngine;

namespace UniTwitchIRC.Controllers
{
    /// <summary>
    /// Handles what happens when the player aquires a pickup
    /// </summary>
    [RequireComponent(typeof(RigidbodyPlayerController))]
    [AddComponentMenu("Scripts/Twitch API Integration/Controllers/PlayerPickupController")]
    public class PlayerPickupController : MonoBehaviour
    {
        /// <summary>
        /// Event invoked when a player triggers a pickup
        /// </summary>
        public static event Action<RigidbodyPlayerController, GameObject> OnPickUp;

        RigidbodyPlayerController m_PlayerController = null;
        
        void Awake()
        {
            m_PlayerController = GetComponent<RigidbodyPlayerController>();
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("PickUp"))
            {
                m_PlayerController.points++;
                if(OnPickUp != null)
                {
                    OnPickUp(m_PlayerController, other.gameObject);
                }
            }
        }

    }
}
