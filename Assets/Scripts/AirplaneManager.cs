using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro; 

public class AirplaneManager : MonoBehaviour
{
    public TwitchChat twitchChat;
    public RectTransform airPlane;
    private Vector2 initPosition;
    public Vector2 randomPosY;
    public string[] airplaneTexts;

    private TextMeshProUGUI airplaneText;

    private int currentTextIndex = -1;

    private void Start()
    {
        airplaneText = GetComponentInChildren<TextMeshProUGUI>();
        initPosition = airPlane.transform.position;
        StartCoroutine(Behavior());
    }

    private IEnumerator Behavior()
    {
        while(true)
        {
            airPlane.transform.position = new Vector3(initPosition.x,initPosition.y +( initPosition.y * (Random.Range(-0.2f,0.35f))), 0);
            yield return new WaitForSeconds(Random.Range(40f, 60f));
            airplaneText.text = chooseRandomText();
            airPlane.transform.DOMoveX(-Screen.width, 15f);
        }
    }

    private string chooseRandomText()
    {
        int newIndex = Random.Range(0, airplaneTexts.Length);
        while(newIndex == currentTextIndex)
            newIndex = Random.Range(0, airplaneTexts.Length);
        currentTextIndex = newIndex;
        string textToChose = airplaneTexts[currentTextIndex];
        if (textToChose.Contains("$currentPlayers"))
        {
            textToChose = textToChose.Replace("$currentPlayers", GameManager.Instance.ActivePlayers.ToString());
        }
        if (textToChose.Contains("$totalPlayers"))
        {
            textToChose = textToChose.Replace("$totalPlayers", twitchChat.TotalPlayers.ToString());
        }
        return textToChose;
    }
}
