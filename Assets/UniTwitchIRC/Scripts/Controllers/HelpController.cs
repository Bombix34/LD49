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

using IRCnect.Channel.Monitor.Replies.Inbounds;
using IRCnect.Channel.Monitor.Replies.Inbounds.Commands;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UniTwitchIRC.TwitchInterface;
using UniTwitchIRC.TwitchInterface.Commands;
using UnityEngine;
using UniTwitchIRC.TwitchInterface.PollManagement;

namespace UniTwitchIRC.Controllers
{
    /// <summary>
    /// Help controller provides help to the Twitch Chat room about the commands available within your application
    /// </summary>
    [RequireComponent(typeof(AdminReference))]
    [AddComponentMenu("Scripts/Twitch API Integration/Controllers/HelpController")]
    public class HelpController : MonoBehaviour
    {

        const string SPACED_DASH = " - ";
        const string SPACED_COMMA = ", ";
        const string SPACED_COLON = ": ";
        const string HELP_PREFIX = "Help for ";
        const string COMMANDS_PREFIX = "Commands: ";
        const string GREETINGS_PREFIX = "Greetings: ";

        [SerializeField, TwitchCommand, Tooltip("Gets avaliable help from the CommandsBehaviour list.")]
        string m_HelpCommand = "help";

        [SerializeField, TwitchCommand, Tooltip("Gets avaliable help for a specific command.")]
        string m_CommandHelp = "h";

        AdminReference m_AdminReference = null;
        
        string m_InboundMessage;

        Dictionary<string, string> m_HelpDescriptions = new Dictionary<string, string>();

        void Awake()
        {
            StringBuilder inboundBuilder = new StringBuilder(GREETINGS_PREFIX);

            m_AdminReference = GetComponent<AdminReference>();
            m_AdminReference.twitchChat.onFilterAdded += (filters) =>
            {
                for(int i = 0; i < filters.Length; i++)
                {
                    foreach(var item in filters[i].rawInput)
                    {
                        if (filters[i] is CommandsFilter) continue;
                        if (filters[i] is PollFilter) continue;

                        if (filters[i] is InboundsFilter)
                        {
                            inboundBuilder.Append(item.Value);
                            inboundBuilder.Append(SPACED_COMMA);
                        }
                    }
                }
                m_InboundMessage = inboundBuilder.ToString().Trim(new char[] { ',', ' ' });
            };
        }

        void OnEnable()
        {
            CommandManager.OnActivated -= CommandManager_OnActivated;
            CommandManager.OnActivated += CommandManager_OnActivated;

            CommandsBehaviour.OnCommandsAdded -= CommandsBehaviour_OnCommandsAdded;
            CommandsBehaviour.OnCommandsAdded += CommandsBehaviour_OnCommandsAdded;

            CommandsBehaviour.OnCommandsReceived -= CommandsBehaviour_OnCommandsReceived;
            CommandsBehaviour.OnCommandsReceived += CommandsBehaviour_OnCommandsReceived;
        }

        void OnDisable()
        {
            CommandManager.OnActivated -= CommandManager_OnActivated;

            CommandsBehaviour.OnCommandsAdded -= CommandsBehaviour_OnCommandsAdded;

            CommandsBehaviour.OnCommandsReceived -= CommandsBehaviour_OnCommandsReceived;
        }

        void CommandManager_OnActivated()
        {
            foreach (KeyValuePair<string, BaseCommandHandler> item in CommandManager.commandHandlers)
            {
                string key = item.Key.Remove(0, 1);
                if (!m_HelpDescriptions.ContainsKey(key))
                {
                    m_HelpDescriptions.Add(key, item.Value.description);
                }
            }
        }

        void CommandsBehaviour_OnCommandsAdded(IEnumerable<RepliesSet> repliesSet)
        {
            foreach (var replies in repliesSet)
            {
                if(!m_HelpDescriptions.ContainsKey(replies.message))
                {
                    m_HelpDescriptions.Add(replies.message, replies.description);
                }
            }
        }

        void CommandsBehaviour_OnCommandsReceived(TwitchChat twitchChat, CommandsArgs commandsArgs)
        {
            if(commandsArgs.IsCommand(twitchChat.commandSymbol, m_HelpCommand))
            {
                StringBuilder sb = new StringBuilder(COMMANDS_PREFIX);
                foreach (var item in m_HelpDescriptions)
                {
                    sb.Append(twitchChat.commandSymbol);
                    sb.Append(item.Key);
                    sb.Append(SPACED_COMMA);
                }

                string names = sb.ToString().Trim(new char[] { ',', ' ' });

                sb.Clear();
                sb.Append(m_InboundMessage);
                sb.Append(SPACED_DASH);
                sb.Append(names);

                twitchChat.SendChatMessage(sb.ToString());
            }
            else if(commandsArgs.IsCommand(twitchChat.commandSymbol, m_CommandHelp))
            {
                if(m_HelpDescriptions.ContainsKey(commandsArgs.argument))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(HELP_PREFIX);
                    sb.Append(commandsArgs.argument);
                    sb.Append(SPACED_COLON);
                    sb.Append(string.Format(m_HelpDescriptions[commandsArgs.argument], twitchChat.commandSymbol));

                    twitchChat.SendChatMessage(sb.ToString());
                }
            }
        }
    }
}