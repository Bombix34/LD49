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
using IRCnect.Channel.Monitor.Replies.Inbounds;
using IRCnect.Channel.Monitor.Replies.Inbounds.Commands;
using System.Collections.Generic;
using System.Linq;
using UniTwitchIRC.Controllers;
using UniTwitchIRC.TwitchInterface.Commands;
using UnityEngine;

namespace UniTwitchIRC.TwitchInterface
{
    /// <summary>
    /// List to enter to commands expected
    /// <para>All commands you use should be entered here for global access to the parameters</para>
    /// </summary>
    [RequireComponent(typeof(AdminReference))]
    [AddComponentMenu("Scripts/Twitch API Integration/TwitchInterface/TwitchIRC/CommandsBehaviour")]
    public class CommandsBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Event to invoke when a command has been added to the internal list from the start method
        /// </summary>
        public static event RepliesSet.Added OnCommandsAdded;

        /// <summary>
        /// Event invoked when a command is received from the client
        /// </summary>
        public static event System.Action<TwitchChat, CommandsArgs> OnCommandsReceived;

        [SerializeField]
        RepliesSet[] m_BasicCommands = null;

        [SerializeField]
        RepliesSet[] m_ParameterizedCommands = null;

        [SerializeField]
        RepliesSet[] m_NArgunmentCommands = null;

        [SerializeField]
        RepliesSet[] m_BaseCommands = null;
        
        AdminReference m_AdminReference = null;

        public AdminReference adminReference
        {
            get
            {
                if(m_AdminReference == null)
                {
                    m_AdminReference = GetComponent<AdminReference>();
                }
                return m_AdminReference;
            }
        }

        /// <summary>
        /// Event to invoke when a command has been added to the internal list from the start method
        /// </summary>
        public event RepliesSet.Added onAdded = (rs) => { };
        
        /// <summary>
        /// Event invoked when a command is received from the client
        /// </summary>
        public event System.Action<TwitchChat, CommandsArgs> onReceived = (tw, ca) => { };

        /// <summary>
        /// Concats all the commands array into one list
        /// </summary>
        /// <returns>The complete list of the commands</returns>
        public RepliesSet[] GetRepliesSets()
        {
            List<RepliesSet> repliesSets = new List<RepliesSet>();
            repliesSets.AddRange(m_BasicCommands);
            repliesSets.AddRange(m_ParameterizedCommands);
            repliesSets.AddRange(m_NArgunmentCommands);
            repliesSets.AddRange(m_BaseCommands);
            return repliesSets.ToArray();
        }
      
        /// <summary>
        /// Iterater over all the command arrays and adds them to the command filters callback request monitor
        /// <para>Override in derives classes to provide addition functionality</para>
        /// </summary>
        protected virtual void Start()
        {
            foreach(var command in m_BasicCommands)
            {
                adminReference.twitchChat.AddMonitorFilters(new CommandsFilter(adminReference.twitchChat.commandSymbol, InboundsFilter.MESSAGE_PATTERN)
                    .AddBasicCommand(command.message, InvokeOnReceived));
            }

            foreach(var command in m_ParameterizedCommands)
            {
                adminReference.twitchChat.AddMonitorFilters(new CommandsFilter(adminReference.twitchChat.commandSymbol, InboundsFilter.MESSAGE_PATTERN)
                    .AddParameterizedCommand(command.message, InvokeOnReceived));
            }

            foreach (var command in m_NArgunmentCommands)
            {
                adminReference.twitchChat.AddMonitorFilters(new CommandsFilter(adminReference.twitchChat.commandSymbol, InboundsFilter.MESSAGE_PATTERN)
                    .AddNParameterCommand(command.message, InvokeOnReceived));
            }

            foreach (var command in m_BaseCommands)
            {
                adminReference.twitchChat.AddMonitorFilters(new CommandsFilter(adminReference.twitchChat.commandSymbol, InboundsFilter.MESSAGE_PATTERN)
                    .AddBasicCommand(command.message, InvokeOnReceived)
                    .AddNParameterCommand(command.message, InvokeOnReceived));
            }

            InvokeOnAdded(m_BasicCommands);
            InvokeOnAdded(m_ParameterizedCommands);
            InvokeOnAdded(m_NArgunmentCommands);
            InvokeOnAdded(m_BaseCommands);
        }

        void OnEnable()
        {
            CommandManager.OnActivated -= CommandManager_OnActivated;
            CommandManager.OnActivated += CommandManager_OnActivated;
        }

        void OnDisable()
        {
            CommandManager.OnActivated -= CommandManager_OnActivated;
        }

        void CommandManager_OnActivated()
        {
            List<RepliesSet> list = m_BaseCommands.ToList();
            foreach (var item in CommandManager.commandHandlers)
            {
                string name = item.Key.Substring(adminReference.twitchChat.commandSymbol.Length);
                RepliesSet repliesSet = m_BaseCommands.FirstOrDefault(x => x.message == name);
                if (repliesSet == null)
                {
                    list.Add(new RepliesSet()
                    {
                        message = name,
                        description = item.Value.description
                    });
                }
            }
            m_BaseCommands = list.ToArray();
        }

        void InvokeOnAdded(IEnumerable<RepliesSet> repliesSet)
        {
            onAdded.Invoke(repliesSet);
            OnCommandsAdded?.Invoke(repliesSet);
        }

        void InvokeOnReceived(MonitorArgs obj)
        {
            CommandsArgs e = obj as CommandsArgs;
            onReceived.Invoke(adminReference.twitchChat, e);
            OnCommandsReceived?.Invoke(adminReference.twitchChat, e);
        }
    }
}