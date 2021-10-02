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

using IRCnect.Channel.Monitor.Replies.Inbounds.Commands;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using System;
using IRCnect.Channel.Monitor.Replies.Inbounds;
using IRCnect.Channel.Monitor;

namespace UniTwitchIRC.TwitchInterface.Commands
{
    /// <summary>
    /// Loads command classes that inherit from BaseCommandHandler
    /// </summary>
    [AddComponentMenu("Scripts/Twitch API Integration/TwitchInterface/Commands/CommandManager", 0)]
    public class CommandManager : MonoBehaviour
    {

        static Dictionary<string, BaseCommandHandler> s_CommandHandlers = new Dictionary<string, BaseCommandHandler>();
        
        [SerializeField, RequiredInHierarchy(typeof(TwitchChat))]
        TwitchChat m_TwitchChat = null;

        public static Dictionary<string, BaseCommandHandler> commandHandlers => s_CommandHandlers;

        public static event Action OnActivated;

        /// <summary>
        /// Get all command names
        /// </summary>
        /// <returns></returns>
        public static string[] GetCommandNames()
        {
            return s_CommandHandlers.Keys.ToArray();
        }

        /// <summary>
        /// Get all the commands
        /// </summary>
        /// <returns></returns>
        public static BaseCommandHandler[] GetCommands()
        {
            return s_CommandHandlers.Values.ToArray();
        }

        void Awake()
        {
            //ActivateCommandHandlers(m_TwitchChat);
        }
        
        public static void ActivateCommandHandlers(TwitchChat twitchChat)
        {
            if (s_CommandHandlers.Count() == 0)
            {

                string BuildCommandKey(Type type)
                {
                    return $"{twitchChat.commandSymbol}{type.Name.Replace("CommandHandler", "").ToLower()}";
                }

                BaseCommandHandler BuildCommandValue(Type type)
                {
                    BaseCommandHandler bch = (BaseCommandHandler)Activator.CreateInstance(type);
                    bch.SetUp(twitchChat);
                    return bch;
                }

                s_CommandHandlers = new Dictionary<string, BaseCommandHandler>();

                s_CommandHandlers = Assembly.GetExecutingAssembly().GetTypes()
                           .Where(CanUse)
                           .ToDictionary(BuildCommandKey, BuildCommandValue);

            }
            OnActivated?.Invoke();
        }

        static bool CanUse(Type m)
        {
            if(m.GetCustomAttribute<SharedBaseAttribute>() != null && m.BaseType == typeof(BaseCommandHandler)) return false;
            return m.IsSubclassOf(typeof(BaseCommandHandler));
        }

        public static void DeactivateCommandHandlers()
        {
            foreach (KeyValuePair<string, BaseCommandHandler> bch in s_CommandHandlers)
            {
                bch.Value.ShutDown();
            }
        }

        void Start()
        {
            m_TwitchChat.AddMonitorFilters(new CommandsFilter(m_TwitchChat.commandSymbol, InboundsFilter.MESSAGE_PATTERN));
            m_TwitchChat.onReceived += m_TwitchChat_onReceived;

            ActivateCommandHandlers(m_TwitchChat);
        }

        void OnApplicationQuit()
        {
            DeactivateCommandHandlers();
        }

        void m_TwitchChat_onReceived(object sender, MonitorArgs e)
        {
            //Debug.Log($"{e.rawData}");
            if (e is CommandsArgs)
            {
                CommandsArgs args = e as CommandsArgs;
                
                if (s_CommandHandlers.TryGetValue(args.command, out BaseCommandHandler value))
                {
                    value.OnCommandRecieved(args);
                }
            }
        }

        /// <summary>
        /// Get a command
        /// </summary>
        /// <param name="name">The lowercase name of the command</param>
        /// <returns></returns>
        public BaseCommandHandler this[string name]
        {
            get
            {
                if(s_CommandHandlers.TryGetValue(name, out BaseCommandHandler value)) return value;
                return null;
            }
        }

    }
}