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

#pragma warning disable 0649

using IRCnect.Channel.Monitor;
using IRCnect.Channel.Monitor.Replies.Inbounds;
using System.Linq;
using System.Collections.Generic;
using UniTwitchIRC.Controllers;
using UnityEngine;
using System;

namespace UniTwitchIRC.TwitchInterface.PollManagement
{
    /// <summary>
    /// Manages the stream polls
    /// </summary>
    [System.Serializable]
    [AddComponentMenu("Scripts/Twitch API Integration/TwitchInterface/PollManagement/PollManager")]
    public class PollManager : MonoBehaviour
    {

        static PollManager s_Instance = null;

        public static PollManager instance
        {
            get
            {
                if(s_Instance == null)
                {
                    s_Instance = FindObjectOfType<PollManager>();
                }
                return s_Instance;
            }
        }
        
        [SerializeField]
        bool m_OpenPoll = false;
        [SerializeField, Tooltip("Send poll open/close messages to chat")]
        bool m_SendPoll = true;
        [SerializeField]
        Statements m_Statements;

        [SerializeField]
        string m_FormatMessage = "{0} Choices - {1}";
        [SerializeField]
        string m_NumberSeperator = ")";
        [SerializeField]
        string m_Delimiter = " | ";

        [SerializeField]
        string m_Question = "Should we have a poll";

        [SerializeField]
        List<Option> m_Options = new List<Option>()
        {
            new Option(){value = "Yes", votes = 0},
            new Option(){value = "No", votes = 0},
            new Option(){value = "Maybe", votes = 0}
        };

        List<string> m_UserAnswered = new List<string>();
        List<PollFilter> m_PollFilters = new List<PollFilter>();
        
        AdminReference m_AdminReference = null;
        bool m_LastOpenPollState = false;

        /// <summary>
        /// Will toggle poll functionality by invoking methods needed to filter poll responses
        /// </summary>
        public bool openPoll
        {
            get
            {
                return m_OpenPoll;
            }
            set
            {
                m_OpenPoll = value;
                TogglePollAvailability();
            }
        }

        void TogglePollAvailability()
        {
            if(!m_AdminReference)
            {
                m_AdminReference = GameObject.FindObjectOfType<AdminReference>();
            }
            if(m_OpenPoll != m_LastOpenPollState)
            {
                if(m_OpenPoll == true)
                {
                    m_UserAnswered.Clear();
                    for (int i = 0; i < m_Options.Count; i++)
                    {
                        Option tempOption = m_Options[i];
                        tempOption.votes = 0;
                        m_Options[i] = tempOption;
                    }

                    if (string.IsNullOrEmpty(m_Statements.opened) == false)
                    {
                        m_AdminReference.twitchChat.SendChatMessage(m_Statements.opened);
                    }
                    if (m_SendPoll == true)
                    {
                        string pollMessage = string.Format(m_FormatMessage, m_Question, string.Join(m_Delimiter, m_Options.Select((x, i) => $"{i + 1}{m_NumberSeperator} {x.value}").ToArray()));
                        m_AdminReference.twitchChat.SendChatMessage(pollMessage);
                    }

                    PollEvents.instance.InvokStateChanged(new StateChangeEventArgs(m_Options, PollState.OPENED));

                    AddMonitorFilters();
                }
                else
                {
                    if (string.IsNullOrEmpty(m_Statements.closed) == false)
                    {
                        m_AdminReference.twitchChat.SendChatMessage(m_Statements.closed);
                    }

                    PollEvents.instance.InvokStateChanged(new StateChangeEventArgs(m_Options, PollState.CLOSED));

                    if (m_SendPoll == true)
                    {
                        string[] totalMessages = m_Options.Select(x => $"{x.value} has {x.votes}").ToArray();
                        string totalMessage = string.Join(m_Delimiter, totalMessages);
                        m_AdminReference.twitchChat.SendChatMessage(totalMessage);
                    }

                    m_AdminReference.twitchChat.RemoveMonitorFilters(m_PollFilters.ToArray());
                }
            }
            m_LastOpenPollState = m_OpenPoll;
        }

        void InvokeOnReceived(MonitorArgs obj)
        {
            if (m_OpenPoll == false) return;

            InboundsArgs e = obj as InboundsArgs;

            if (m_UserAnswered.Contains(e.nick)) return;

            m_UserAnswered.Add(e.nick);
            
            if (int.TryParse(e.said, out int index) == false)
            {
                index = m_Options.FindIndex(x => x.value.ToLower() == e.said.ToLower());
            }
            else
            {
                index--; // vote will be from 1 to max number
            }

            Option option = m_Options[index];

            option.votes++;

            m_Options[index] = option;

            PollEvents.instance.InvokeVoteChanged(new VoteChangeEventArgs(option, e));
        }

        void AddMonitorFilters()
        {
            m_AdminReference.twitchChat.RemoveMonitorFilters(m_PollFilters.ToArray());
            m_PollFilters.Clear();
            for (int i = 1; i < m_Options.Count + 1; i++)
            {
                int index = i - 1;
                Option tempOption = m_Options[index];
                tempOption.index = index;
                m_Options[index] = tempOption;
                m_PollFilters.Add(((PollFilter)new PollFilter()
                    .AddRepliesFilter(new[] { i.ToString() }, InvokeOnReceived)
                    .AddRepliesFilter(new[] { tempOption.value }, InvokeOnReceived)));
            }
            m_AdminReference.twitchChat.AddMonitorFilters(m_PollFilters.ToArray());
        }

        public void CreatePoll(string question, IEnumerable<Option> options)
        {
            m_Question = question;
            m_Options = options.ToList();
        }
    }
}
#pragma warning restore 0649