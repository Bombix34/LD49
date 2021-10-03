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

    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;

    public string username, password, channelName; //Get the password from https://twitchapps.com/tmi

    public Text chatBox;
    public BoardManager boardManager;

    void Start()
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

    void Update()
    {
        if (twitchClient == null || !twitchClient.Connected)
        {
            Connect();
        }
        ReadChat();
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
                chatBox.text = chatBox.text + "\n" + String.Format("{0}: {1}", chatName, message);

                //Run the instructions to control the game!
                GameInputs(message);
            }
        }
    }

    private void GameInputs(string ChatInputs)
    {
        if (ChatInputs.Contains("!house"))
        {
            Vector2 position = GetPositionFromMessage(ChatInputs, 5);
            boardManager.SpawnBuilding(position, BuildingTypes.house);
        }
        else if (ChatInputs.Contains("!appartments"))
        {
            Vector2 position = GetPositionFromMessage(ChatInputs, 11);
            boardManager.SpawnBuilding(position, BuildingTypes.appartments);
        }
        else if (ChatInputs.Contains("!office"))
        {
            Vector2 position = GetPositionFromMessage(ChatInputs, 6);
            boardManager.SpawnBuilding(position, BuildingTypes.office);
        }
        else if (ChatInputs.Contains("!mall"))
        {
            Vector2 position = GetPositionFromMessage(ChatInputs, 4);
            boardManager.SpawnBuilding(position, BuildingTypes.mall);
        }
        else if (ChatInputs.Contains("!factory"))
        {
            Vector2 position = GetPositionFromMessage(ChatInputs, 7);
            boardManager.SpawnBuilding(position, BuildingTypes.factory);
        }
        else if (ChatInputs.Contains("!powerplant"))
        {
            Vector2 position = GetPositionFromMessage(ChatInputs, 10);
            boardManager.SpawnBuilding(position, BuildingTypes.powerPlant);
        }
        else if (ChatInputs.Contains("!cinema"))
        {
            Vector2 position = GetPositionFromMessage(ChatInputs, 6);
            boardManager.SpawnBuilding(position, BuildingTypes.cinema);
        }
        else if (ChatInputs.Contains("!casino"))
        {
            Vector2 position = GetPositionFromMessage(ChatInputs, 6);
            boardManager.SpawnBuilding(position, BuildingTypes.casino);
        }
    }

    private Vector2 GetPositionFromMessage(string message, int wordLenght)
    {
        int x=-1;
        int y = 0;
        var infoPosition = message.Substring(2 + wordLenght);
        string posX = infoPosition.Substring(0, 1);
        switch (posX)
        {
            case "A":
                x = 0;
                break;
            case "B":
                x = 1;
                break;
            case "C":
                x = 2;
                break;
            case "D":
                x = 3;
                break;
            case "E":
                x = 4;
                break;
            case "F":
                x = 5;
                break;
            case "G":
                x = 6;
                break;
            case "H":
                x = 7;
                break;
            case "I":
                x = 8;
                break;
            case "J":
                x = 9;
                break;
            case "K":
                x = 10;
                break;
            case "L":
                x = 11;
                break;
        }
        string infoPositionY = infoPosition.Substring(1);
        y = int.Parse(infoPositionY)-1;
        return new Vector2(x, y);
    }
}