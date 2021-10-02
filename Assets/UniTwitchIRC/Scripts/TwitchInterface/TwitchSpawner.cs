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

using IRCnect.Channel.Monitor;
using IRCnect.Channel.Monitor.Replies.Inbounds.Commands;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UniTwitchIRC.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace UniTwitchIRC.TwitchInterface
{
    /// <summary>
    /// Spawn an item for a viewer from a chat message command
    /// </summary>
    [AddComponentMenu("Scripts/Twitch API Integration/TwitchInterface/TwitchIRC/TwitchSpawner")]
    public class TwitchSpawner : MonoBehaviour
    {
        [System.Serializable]
        public class SpawnEvent : UnityEvent<GameObject> { }

        /// <summary>
        /// Access to the Twitch chat client
        /// </summary>
        [Header("Twitch Controllers")]
        [SerializeField, RequiredInHierarchy(typeof(TwitchChat))]
        protected TwitchChat m_TwitchChat = null;

        /// <summary>
        /// Access to the players queue for spawn and validation
        /// </summary>
        [SerializeField, RequiredInHierarchy(typeof(TwitchPlayerQueue))]
        protected TwitchPlayerQueue m_TwitchPlayerQueue = null;

        [Header("Spawn Information")]
        [TwitchCommand]
        [SerializeField]
        string m_SpawnCommand = "spawn";

        [SerializeField]
        Vector3 m_Offset = Vector3.up * 2.0f;

        [SerializeField]
        bool m_OnePerUser = true;

        [SerializeField]
        float m_Rate = 1.0f;

        [SerializeField, ReadonlyField(false), Tooltip("Register for the spawn command event, if false manual registration is required")]
        bool m_AutoRegister = true;

        [SerializeField]
        SpawnEvent m_SpawnEvent = null;

        [Header("Prefabs")]
        [SerializeField]
        protected TwitchPlayerController[] m_PlayerControllers = null;

        [SerializeField, Tooltip("Any other prefabs to spawn along with the player controller.")]
        GameObject[] m_Others = null;

        Transform m_Trans = null;

        Dictionary<TwitchPlayerController, List<GameObject>> m_Group { get; set; } = new Dictionary<TwitchPlayerController, List<GameObject>>();

        /// <summary>
        /// The players spawned
        /// </summary>
        Dictionary<string, List<GameObject>> m_Objects = null;

        protected int m_CurrentIndex = -1;

        /// <summary>
        /// The current queued player not spawned yet
        /// </summary>
        protected Queue<string> m_Queue = null;

        /// <summary>
        /// Delegate to identify a spawn request sent from the player queue
        /// </summary>
        /// <param name="nick">The nick/viewer requesting the spawn</param>
        public delegate void SpawnRequested(string nick);

        /// <summary>
        /// Invoked when the spawn command is received and the player is validated
        /// </summary>
        public event SpawnRequested onSpawnRequested;

        protected virtual void Awake()
        {
            m_Trans = transform;
            m_Queue = new Queue<string>();
            m_Objects = new Dictionary<string, List<GameObject>>();
        }

        protected virtual void OnEnable()
        {
            if(m_AutoRegister)
            {
                CommandsBehaviour.OnCommandsReceived -= CommandsBehaviour_OnCommandsReceived;
                CommandsBehaviour.OnCommandsReceived += CommandsBehaviour_OnCommandsReceived;
            }
        }

        protected virtual void OnDisable()
        {
            if (m_AutoRegister)
            {
                CommandsBehaviour.OnCommandsReceived -= CommandsBehaviour_OnCommandsReceived;
            }
        }

        void CommandsBehaviour_OnCommandsReceived(TwitchChat twitchChat, CommandsArgs args)
        {
            Spawn(args);
        }

        IEnumerator Start()
        {
            m_TwitchPlayerQueue.onValidated += m_TwitchPlayerQueue_onValidated;
            while (enabled)
            {
                if (m_Queue.Count > 0)
                {
                    string nick = m_Queue.Dequeue();
                    TwitchPlayerController playerController = SpawnPlayerPrefabs(nick, 0) as TwitchPlayerController;
                    m_SpawnEvent.Invoke(playerController.gameObject);

                }
                yield return new WaitForSeconds(m_Rate);
            }
        }

        void m_TwitchPlayerQueue_onValidated(TwitchPlayerQueue.Player player)
        {
            string nick = player.nick;
            AddNickToObjectsList(nick);
        }

        void AddNickToObjectsList(string nick)
        {
            if (!m_Objects.ContainsKey(nick))
            {
                m_Objects.Add(nick, new List<GameObject>());
            }
        }

        /// <summary>
        /// Spawn a user object for a player
        /// <para>Override in derived classes to Instantiate additional objects and associate them with the PlayerController</para>
        /// </summary>
        /// <param name="nick">The viewer requesting the spawn</param>
        /// <param name="index">Index of item in the prefab array</param>
        protected virtual TwitchPlayerController SpawnPlayerPrefabs(string nick, int index)
        {
            if (m_OnePerUser)
            {
                if (m_Objects[nick].Count > 0) return m_Objects[nick][0].GetComponent<TwitchPlayerController>();
            }

            TwitchPlayerController playerController = GameObject.Instantiate<TwitchPlayerController>(m_PlayerControllers[index]);
            playerController.StartNew(nick, m_Trans.position + m_Offset);
            playerController.gameObject.name = string.Concat(m_PlayerControllers[index].name, ".", nick);

            if (!m_Objects[nick].Contains(playerController.gameObject))
            {
                m_Objects[nick].Add(playerController.gameObject);
                PlayerPrefabInstantiated(playerController, playerController.gameObject);
            }

            for (int i = 0; i < m_Others.Length; i++)
            {
                GameObject otherGO = GameObject.Instantiate<GameObject>(m_Others[i]);
                if (!m_Objects[nick].Contains(otherGO))
                {
                    m_Objects[nick].Add(otherGO);
                    PlayerPrefabInstantiated(playerController, otherGO);
                }
            }
            if (m_Group.TryGetValue(playerController, out List<GameObject> value) == false)
            {
                value = m_Objects[nick].Skip(1).ToList();
                m_Group.Add(playerController, value);
            }
            return playerController;
        }

        /// <summary>
        /// Gives derived classes an oppotunity to modify the GameObject
        /// </summary>
        /// <param name="playerController">Player controller created</param>
        /// <param name="playerGameObject">Player associated GameObject created</param>
        protected virtual void PlayerPrefabInstantiated(TwitchPlayerController playerController, GameObject playerGameObject)
        {
            IPlayerUI playerUI = playerGameObject.GetComponent<IPlayerUI>();
            if (playerUI != null)
            {
                playerGameObject.GetComponent<IPlayerUI>().playerController = playerController;
                playerGameObject.name = string.Concat(playerGameObject.name, ".", playerController.nick);
            }
        }

        /// <summary>
        /// Spawn a user object for a player
        /// </summary>
        /// <param name="monitorArgs">Raw Command args to check for spawn command</param>
        public void Spawn(MonitorArgs monitorArgs)
        {
            CommandsArgs commandsArgs = monitorArgs as CommandsArgs;
            if (commandsArgs.IsCommand(m_TwitchChat.commandSymbol, m_SpawnCommand))
            {
                Spawn(commandsArgs.nick);
            }
        }

        /// <summary>
        /// Spawn a user object for a player
        /// </summary>
        /// <param name="nick">The viewer requesting the spawn</param>
        public void Spawn(string nick)
        {
            if (nick == m_TwitchChat.messenger.channel)
            {
                AddNickToObjectsList(nick);
            }
            if (m_Objects.ContainsKey(nick))
            {
                m_Queue.Enqueue(nick);
            }
            if (onSpawnRequested != null)
            {
                onSpawnRequested.Invoke(nick);
            }
        }

        public Dictionary<string, List<GameObject>> SpawnedObjects()
        {
            return m_Objects.Where(x => x.Value != null && x.Value.Count > 0).ToDictionary(k => k.Key, v => v.Value);
        }

        public Dictionary<T, List<GameObject>> Group<T>() where T : TwitchPlayerController
        {
            return m_Group.ToDictionary(k => (T)k.Key, v => v.Value);
        }

        public void ResetCurrentIndex()
        {
            m_CurrentIndex = -1;
        }

        public T NextPlayer<T>() where T : TwitchPlayerController
        {
            Dictionary<string, List<GameObject>> spawnedObjects = SpawnedObjects();
            
            if (spawnedObjects.Count == 0) return default(T);

            m_CurrentIndex = (m_CurrentIndex + 1) % spawnedObjects.Count;

            T current = default(T);

            int counter = 0;
            while (counter < spawnedObjects.Count)
            {
                string key = spawnedObjects.Keys.ToArray()[m_CurrentIndex];
                if (spawnedObjects[key][0].activeInHierarchy)
                {
                    current = spawnedObjects[key][0].GetComponent<T>();
                    break;
                }
                m_CurrentIndex = (m_CurrentIndex + 1) % spawnedObjects.Count;
                counter++;
            }
            return current;
        }

        public bool AnyVisible()
        {
            Dictionary<string, List<GameObject>> spawnedObjects = SpawnedObjects();

            return spawnedObjects.Where(x => x.Value[0].activeInHierarchy).Count() > 0;
        }
        
        public void ActivateAll()
        {
            foreach (var items in SpawnedObjects())
            {
                items.Value[0].SetActive(true);
            }
        }

        public void PositionAll(Vector3 position)
        {
            foreach (var items in SpawnedObjects())
            {
                items.Value[0].transform.position = position;
            }
        }
    }
}
