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

using UniTwitchIRC.TwitchInterface;
using UniTwitchIRC.TwitchInterface.MonitorEvents;
using UnityEngine;

namespace UniTwitchIRC.Controllers
{
    /// <summary>
    /// Spawn an item for a viewer from a chat message command
    /// </summary>
    [AddComponentMenu("Scripts/Twitch API Integration/Controllers/BallSpawner")]
    public class BallSpawner : TwitchSpawner
    {
        static int s_Index = -1;

        /// <summary>
        /// Access to the monitor number event handler
        /// </summary>
        [SerializeField, RequiredInHierarchy(typeof(MonitorNumberEvents))]
        protected MonitorNumberEvents m_MonitorNumberEvents;

        /// <summary>
        /// Access to the monitor string event handler
        /// </summary>
        [SerializeField, RequiredInHierarchy(typeof(MonitorStringEvents))]
        protected MonitorStringEvents m_MonitorStringsEvents;

        protected override TwitchPlayerController SpawnPlayerPrefabs(string nick, int index)
        {
            s_Index = (s_Index + 1) % m_PlayerControllers.Length;
            RigidbodyPlayerController playerController = base.SpawnPlayerPrefabs(nick, s_Index) as RigidbodyPlayerController;

            m_MonitorStringsEvents.onStringArray.AddListener((string username, string command, string[] nArgument) =>
            {
                playerController.OnDisplayNameChange(m_TwitchChat, username, command, nArgument);
            });

            m_MonitorNumberEvents.onTwitchCommand.AddListener(playerController.OnReset);

            m_MonitorNumberEvents.onTwitchCommand.AddListener(playerController.OnMoveUniDirection);
            m_MonitorNumberEvents.onTwitchCommand.AddListener(playerController.OnMove);

            return playerController;
        }
    }
}
