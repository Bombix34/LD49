using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.IO;
using UnityEngine.UI;

public class TwitchChat : MonoBehaviour
{
    public List<string> activePlayers;

    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;

    public string username, password, channelName; //Get the password from https://twitchapps.com/tmi

    public Text chatBox;
    public BoardManager boardManager;

    private void Awake()
    {
        activePlayers = new List<string>();
    }

    private void Start()
    {
        Connect();
        /*
        GameInputs("!house J4");
        GameInputs("!appartments A2");
        GameInputs("!office A5");
        GameInputs("!mall C2");
        GameInputs("!factory G6");
        GameInputs("!powerplant D6");
        GameInputs("!cinema H7");
        GameInputs("!casino A9");
        */
    }

    private void Update()
    {
        if (twitchClient == null || !twitchClient.Connected)
        {
            Connect();
        }
        ReadChat();
        CountActivePlayers();
        PingServer();
    }

    private void Connect()
    {
        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        reader = new StreamReader(twitchClient.GetStream());
        writer = new StreamWriter(twitchClient.GetStream());

        writer.WriteLine("PASS " + password);
        writer.WriteLine("NICK " + username);
        writer.WriteLine("USER " + username + " 8 * :" + username);
        writer.WriteLine("JOIN #" + channelName);
        writer.Flush();
    }

    private void ReadChat()
    {
        if(GameManager.Instance.IsGameFinished)
        {
            return;
        }
        if (twitchClient.Available > 0)
        {
            var message = reader.ReadLine(); //Read in the current message

            if (message.Contains("PRIVMSG"))
            {
                //Get the users name by splitting it from the string
                var splitPoint = message.IndexOf("!", 1);
                var chatName = message.Substring(0, splitPoint);
                chatName = chatName.Substring(1);

                //Get the users message by splitting it from the string
                splitPoint = message.IndexOf(":", 1);
                message = message.Substring(splitPoint + 1);
                //print(String.Format("{0}: {1}", chatName, message));

                //Run the instructions to control the game!
                if (GameInputs(message.ToLower()))
                {
                    chatBox.text = chatBox.text + "\n" + String.Format("{0}: {1}", chatName, message);

                    if (!activePlayers.Contains(chatName))
                    {
                        activePlayers.Add(chatName);
                    }
                }
            }
        }
    }

    private void PingServer()
    {
        if (Time.frameCount % 60 == 0 && activePlayers.Count < 2)
        {
            writer = new StreamWriter(twitchClient.GetStream());
            writer.WriteLine("PING");
            writer.Flush();
        }
    }

    private void CountActivePlayers()
    {
        if(Time.frameCount % 1200 == 0)
        {
            int activePlayersCounter = activePlayers.Count;
            ResourcesManager.Instance.UpdateActivePlayers(activePlayersCounter);
            activePlayers.Clear();
            GameManager.Instance.ActivePlayers = activePlayersCounter;
        }
    }

    private bool GameInputs(string ChatInputs)
    {
        if (ChatInputs.Contains("!house"))
        {
            Vector2 position = GetPositionFromMessage(ChatInputs, 5);
            boardManager.SpawnBuilding(position, BuildingTypes.house);
            return true;
        }
        else if (ChatInputs.Contains("!appartments"))
        {
            Vector2 position = GetPositionFromMessage(ChatInputs, 11);
            boardManager.SpawnBuilding(position, BuildingTypes.appartments);
            return true;
        }
        else if (ChatInputs.Contains("!office"))
        {
            Vector2 position = GetPositionFromMessage(ChatInputs, 6);
            boardManager.SpawnBuilding(position, BuildingTypes.office);
            return true;
        }
        else if (ChatInputs.Contains("!mall"))
        {
            Vector2 position = GetPositionFromMessage(ChatInputs, 4);
            boardManager.SpawnBuilding(position, BuildingTypes.mall);
            return true;
        }
        else if (ChatInputs.Contains("!factory"))
        {
            Vector2 position = GetPositionFromMessage(ChatInputs, 7);
            boardManager.SpawnBuilding(position, BuildingTypes.factory);
            return true;
        }
        else if (ChatInputs.Contains("!powerplant"))
        {
            Vector2 position = GetPositionFromMessage(ChatInputs, 10);
            boardManager.SpawnBuilding(position, BuildingTypes.powerPlant);
            return true;
        }
        else if (ChatInputs.Contains("!cinema"))
        {
            Vector2 position = GetPositionFromMessage(ChatInputs, 6);
            boardManager.SpawnBuilding(position, BuildingTypes.cinema);
            return true;
        }
        else if (ChatInputs.Contains("!casino"))
        {
            Vector2 position = GetPositionFromMessage(ChatInputs, 6);
            boardManager.SpawnBuilding(position, BuildingTypes.casino);
            return true;
        }
        else if (ChatInputs.Contains("!road"))
        {
            Vector2 position = GetPositionFromMessage(ChatInputs, 4);
            boardManager.SpawnBuilding(position, BuildingTypes.road);
            return true;
        }
        else if(ChatInputs.Contains("!burn"))
        {
            Vector2 position = GetPositionFromMessage(ChatInputs, 4);
            boardManager.RemoveBuilding(position);
            return true;
        }
        return false;
    }

    private Vector2 GetPositionFromMessage(string message, int wordLenght)
    {
        int x=-1;
        int y = 0;
        var infoPosition = message.Substring(2 + wordLenght);
        string posX = infoPosition.Substring(0, 1);
        switch (posX)
        {
            case "a":
                x = 0;
                break;
            case "b":
                x = 1;
                break;
            case "c":
                x = 2;
                break;
            case "d":
                x = 3;
                break;
            case "e":
                x = 4;
                break;
            case "f":
                x = 5;
                break;
            case "g":
                x = 6;
                break;
            case "h":
                x = 7;
                break;
            case "i":
                x = 8;
                break;
            case "j":
                x = 9;
                break;
            case "k":
                x = 10;
                break;
            case "l":
                x = 11;
                break;
        }
        string infoPositionY = infoPosition.Substring(1);
        y = int.Parse(infoPositionY)-1;
        return new Vector2(x, y);
    }

}