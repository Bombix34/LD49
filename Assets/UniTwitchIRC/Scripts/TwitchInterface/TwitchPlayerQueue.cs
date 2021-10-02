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
using IRCnect.Channel.Monitor.Capabilities;
using IRCnect.Channel.Monitor.Replies.Inbounds.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using TwitchConnectTv;
using TwitchUnityIRC.Channel.Monitor.Capabilities;
using UnityEngine;
using UnityEngine.Networking;
using IRCnect.Channel.Monitor.Replies.Inbounds;
using PRIVMSG = TwitchUnityIRC.Channel.Monitor.Capabilities.Tags.PRIVMSG;
using FetchHttpRequest;

namespace UniTwitchIRC.TwitchInterface
{
    /// <summary>
    /// Maintains a queue of the players
    /// <para>Also checks the follower staus of the viewer</para>
    /// </summary>
    [AddComponentMenu("Scripts/Twitch API Integration/TwitchInterface/TwitchIRC/TwitchPlayerQueue")]
    public class TwitchPlayerQueue : MonoBehaviour
    {
        const string JSON_EXECPTION_MESSAGE = "Exception: {0}";

        /// <summary>
        /// Reference to the player object
        /// </summary>
        [System.Serializable]
        public class Player
        {
            /// <summary>
            /// Name shown in the scene
            /// </summary>
            public string displayName;

            /// <summary>
            /// Index in player queue
            /// </summary>
            public int index;

            /// <summary>
            /// The Twitch chat user name
            /// </summary>
            public string nick;

            /// <summary>
            /// Pool for viewer/player information has been sent and received.
            /// </summary>
            public bool validated;

            /// <summary>
            /// Player follows channel
            /// </summary>
            public bool follows;

            // Added for subscriber
            /// <summary>
            /// Player tags properties about the IRC message
            /// </summary>
            public PRIVMSG.ComplexTags complexTags;

            /// <summary>
            /// Constructor
            /// <para>Creates a new player.</para>
            /// </summary>
            /// <param name="index">Index in the player queue array</param>
            /// <param name="nick">The Twitch chat user name</param>
            [System.Obsolete("Use constructor providing PRIVMSG.ComplexTags")]
            public Player(int index, string nick)
            {
                this.index = index;
                this.nick = nick;
                this.displayName = this.nick;
            }

            // Added for subscriber
            /// <summary>
            /// Constructor
            /// <para>Creates a new player.</para>
            /// </summary>
            /// <param name="index">Index in the player queue array</param>
            /// <param name="complexTags">Player tags properties about the IRC message</param>
            public Player(int index, string nick, PRIVMSG.ComplexTags complexTags)
            {
                this.index = index;
                this.nick = nick;
                this.displayName = complexTags.displayName;
                this.complexTags = complexTags;
            }
        }
        [SerializeField, RequiredInHierarchy(typeof(TwitchConnect))]
        TwitchConnect m_TwitchConnect = null;

        [SerializeField, RequiredInHierarchy(typeof(TwitchChat))]
        TwitchChat m_TwitchChat = null;

        [Header("Commands")]
        [SerializeField, TwitchCommand]
        string m_JoinCommand = "join";
        [SerializeField, Tooltip("The command nick must be a follower of the channel to join.")]
        bool m_JoinMustFollow = false;
        [SerializeField, Tooltip("{0} will be replaced with the nick.")]
        string m_JoinFirstFormat = "Opps, {0} you must !join first.";

        [Space]
        [SerializeField, TwitchCommand]
        string m_NameCommand = "name";
        [SerializeField, Tooltip("The command nick must be a follower of the channel to change the display name.")]
        bool m_NameMustFollow = false;
        [SerializeField, Tooltip("{0} will be replaced with the nick.")]
        string m_FollowFirstFormat = "Hey, {0}, you must be a follower to change your display name.  Follow use the join command again then try to change your display name.";

        [Space]
        [SerializeField, Tooltip("The command nick must be a subscriber to the channel to play.")]
        bool m_MustSubscribe = false;
        [SerializeField, Tooltip("{0} will be replaced with the nick and command name respectively.")]
        string m_SubscribeFirstFormat = "Hey, {0}, you must be a subscriber to use the {0} command.";

        [Header("Command Responses")]
        [Header("Followers")]
        [SerializeField]
        string m_followerResponse = "Thanks for being a Follower!";
        [SerializeField]
        string m_NotFollowerResponse = "Awww y u no Follow?";

        [Header("Join")]
        [SerializeField, Tooltip("{0} will be replaced with the nick.  The {1} with the Follower Response or Not Follower Response")]
        string m_JoinResponseFormat = "{0} has joined. {1}";
        [SerializeField, Tooltip("{0} will be replaced with the nick.  The {1} with the Follower Response or Not Follower Response")]
        string m_NotFollowerResponseFormat = "{0} can not join. {1}";

        [SerializeField, ReadonlyField(false), Tooltip("Register for the join command event, if false manual registration is required")]
        bool m_AutoRegister = true;


        [Header("Others")]
        [SerializeField, Tooltip("{0} will be replaced with the nick..  The {1} with the index quued at.")]
        string m_AlreadyQueuedFormat = "{0} already queued at {1}";

        /// <summary>
        /// DO NOT USE
        /// <para>For Unity Inspector viewing</para>
        /// </summary>
        public List<Player> playerList;

        public Dictionary<string, Player> PlayerQueue { get; private set; } = null;

        /// <summary>
        /// Delegate to identify a player validation status
        /// <para>A request has been made to twitch API for follows status</para>
        /// </summary>
        /// <param name="player">The player that has been validated</param>
        public delegate void Validated(Player player);

        /// <summary>
        /// Delegate to identify a join and name change request sent
        /// </summary>
        /// <param name="nick">The nick/viewer requesting the join or name change commands</param>
        public delegate void Requested(string nick);

        /// <summary>
        /// Invoked when the join command is received and the player is validated
        /// </summary>
        public event Requested onJoinRequested;

        /// <summary>
        /// Invoked when the name command is received and the player is validated
        /// </summary>
        public event Requested onNameChangeRequested;

        /// <summary>
        /// Invoked when the request has been returened for validation
        /// </summary>
        public event Validated onValidated;

        void Start()
        {
            PlayerQueue = new Dictionary<string, Player>();

            CommandsFilter commandsFilter = new CommandsFilter(m_TwitchChat.commandSymbol, InboundsFilter.MESSAGE_PATTERN)
                    .AddParameterizedCommand(m_NameCommand, CommandName);

            if (m_AutoRegister)
            {
                commandsFilter.AddBasicCommand(m_JoinCommand, CommandJoin);
            }

            m_TwitchChat.monitor.AddFilters(commandsFilter);
        }

        public void CommandJoin(MonitorArgs e)
        {
            CommandsArgs obj = e as CommandsArgs;

            IRCTags tags = e.GetTagsByTypeName(DefaultTagParsers.COMPLEXTAGS);

            PRIVMSG.ComplexTags complexTags = tags as PRIVMSG.ComplexTags;

            if (m_MustSubscribe == false || (m_MustSubscribe && complexTags.IsSubscriber()))
            {

                if (PlayerQueue.TryGetValue(obj.nick, out Player player) == false)
                {
                    player = new Player(PlayerQueue.Count, obj.nick, complexTags);

                    PlayerQueue.Add(obj.nick, player);

                    if (onJoinRequested != null)
                    {
                        onJoinRequested.Invoke(obj.nick);
                    }

                    StartCoroutine(JoinQueue(player));
                }
            }
            else
            {
                m_TwitchChat.SendChatMessageFormat(m_SubscribeFirstFormat, obj.nick, obj.command);
            }
        }

        public void CommandName(MonitorArgs e)
        {
            CommandsArgs obj = e as CommandsArgs;

            IRCTags tags = e.GetTagsByTypeName(DefaultTagParsers.COMPLEXTAGS);

            var complexTags = tags as PRIVMSG.ComplexTags;

            if (m_MustSubscribe == false || (m_MustSubscribe && complexTags.IsSubscriber()))
            {
                if (onNameChangeRequested != null)
                {
                    onNameChangeRequested.Invoke(obj.nick);
                }

                if (PlayerQueue.TryGetValue(obj.nick, out Player player) == true)
                {
                    if (m_NameMustFollow)
                    {
                        if (player.follows)
                        {
                            player.displayName = obj.argument;
                        }
                        else
                        {
                            m_TwitchChat.SendChatMessageFormat(m_FollowFirstFormat, obj.nick);
                        }
                    }
                    else
                    {
                        player.displayName = obj.argument;
                    }
                }
                else
                {
                    m_TwitchChat.SendChatMessageFormat(m_JoinFirstFormat, obj.nick);
                }
            }
            else
            {
                m_TwitchChat.SendChatMessageFormat(m_SubscribeFirstFormat, obj.nick, obj.command);
            }
        }

        /// <summary>
        /// Invoked when a join command is received
        /// <para>The command must be validated within the method</para>
        /// </summary>
        /// <param name="nick">The nick/viewer who sent the command</param>
        /// <param name="command">The command received</param>
        [Obsolete("Will be removed, use CommandJoin")]
        public void OnJoinRecieved(string nick, string command)
        {
            if (command.IsCommand(m_TwitchChat.commandSymbol, m_JoinCommand))
            {
                if (onJoinRequested != null)
                {
                    onJoinRequested.Invoke(nick);
                }

                if (PlayerQueue.TryGetValue(nick, out Player player) == true)
                {
                    StartCoroutine(JoinQueue(player));
                }
            }
        }

        /// <summary>
        /// Invoked when a name change command is received
        /// <para>The command must be validated within the method</para>
        /// </summary>
        /// <param name="nick">The nick/viewer who sent the command</param>
        /// <param name="command">The command received</param>
        /// <param name="argument">The argument received containing the new name to display</param>
        [Obsolete("Will be removed, use CommandName")]
        public void OnNameRecieved(string nick, string command, string argument)
        {
            if (command.IsCommand(m_TwitchChat.commandSymbol, m_NameCommand))
            {
                if (onNameChangeRequested != null)
                {
                    onNameChangeRequested.Invoke(nick);
                }

                if (PlayerQueue.TryGetValue(nick, out Player player) == true)
                {
                    if (m_NameMustFollow)
                    {
                        if (player.follows)
                        {
                            player.displayName = argument;
                        }
                        else
                        {
                            m_TwitchChat.SendChatMessageFormat(m_FollowFirstFormat, nick);
                        }
                    }
                    else
                    {
                        player.displayName = argument;
                    }
                }
                else
                {
                    m_TwitchChat.SendChatMessageFormat(m_JoinFirstFormat, nick);
                }
            }
        }

        IEnumerator JoinQueue(Player player)
        {
            if (!player.follows)
            {
                yield return StartCoroutine(CheckValidated(player));
                yield break;
            }
            m_TwitchChat.SendChatMessageFormat(m_AlreadyQueuedFormat, player.nick, player.index);
            yield break;
        }

        IEnumerator CheckValidated(Player player)
        {
            player.validated = false;

            string url = string.Format(TwitchURI.UriFormats.ISFOLLOWER, m_TwitchConnect.broadcaster.channelId, player.complexTags.userId);

            IEnumerator enumerator = UnityFetch.GetRequestEnumerator<TwitchTv.API.IsFollower>(url, new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("Accept",  "application/vnd.twitchtv.v5+json"),
                new KeyValuePair<string, string>("Client-ID",  m_TwitchConnect.broadcaster.url.clientId)
            });
            yield return enumerator;

            TwitchTv.API.IsFollower twitchTMI = enumerator.Current as TwitchTv.API.IsFollower;

            player.validated = twitchTMI != null;
            player.follows = twitchTMI != null ? twitchTMI.Confirmed() : false;

            string joinFormat = m_JoinResponseFormat;

            if (m_JoinMustFollow && (!player.follows && player.nick != m_TwitchChat.messenger.channel))
            {
                PlayerQueue.Remove(player.nick);
                joinFormat = m_NotFollowerResponseFormat;
            }
            if (player.nick != m_TwitchChat.messenger.channel)
            {
                string followMessage = player.follows ? m_followerResponse : m_NotFollowerResponse;
                m_TwitchChat.SendChatMessageFormat(joinFormat, player.nick, followMessage);
            }
            else
            {
                player.follows = true;
            }
            if (PlayerQueue.ContainsKey(player.nick))
            {
                if (onValidated != null)
                {
                    onValidated.Invoke(player);
                }
            }

            /*using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                www.SetRequestHeader("Accept", "application/vnd.twitchtv.v5+json");
                www.SetRequestHeader("Client-ID", m_TwitchConnect.broadcaster.url.clientId);
                // Request and wait for the desired page.
                yield return www.SendWebRequest();

                if (www.isNetworkError)
                {
                    string[] pages = url.Split('?');
                    Debug.LogError(pages[0] + ": Error isNetworkError: " + www.error);
                }
                else if (www.isHttpError && m_JoinMustFollow)
                {
                    if (www.responseCode != 404)
                    {
                        string[] pages = url.Split('?');
                        Debug.LogError(pages[0] + ": Error isHttpError: " + www.error);
                    }
                }
                else
                {
                    //Debug.Log(pages[page] + ":\nReceived: " + www.downloadHandler.text);

                    string rawResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    string result = rawResult.Replace("\n", "").Replace("\r", "");

                    TwitchTv.API.IsFollower twitchTMI = null;

                    try
                    {
                        twitchTMI = m_TwitchConnect.broadcaster.ParseJsonIsFollower(result);
                        player.validated = true;
                        player.follows = twitchTMI.Confirmed();

                        if (m_JoinMustFollow && (!player.follows && player.nick != m_TwitchChat.messenger.channel))
                        {
                            PlayerQueue.Remove(player.nick);
                        }
                        if (player.nick != m_TwitchChat.messenger.channel)
                        {
                            string followMessage = player.follows ? m_followerResponse : m_NotFollowerResponse;
                            m_TwitchChat.SendChatMessageFormat(m_JoinResponseFormat, player.nick, followMessage);
                        }
                        else
                        {
                            player.follows = true;
                        }
                        if (PlayerQueue.ContainsKey(player.nick))
                        {
                            if (onValidated != null)
                            {
                                onValidated.Invoke(player);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogFormat(JSON_EXECPTION_MESSAGE, $"{ex.Message}");
                        refreshDelay = 0.0f;
                    }
                }
            }*/

            yield return null;

        }

        /// <summary>
        /// Removes a player from the player queue be their username or nick
        /// </summary>
        /// <param name="nick">The username of the player to remove</param>
        public void RemovePlayerbyNick(string nick)
        {
            if (PlayerQueue.ContainsKey(nick))
            {
                PlayerQueue.Remove(nick);
            }
        }
    }
}