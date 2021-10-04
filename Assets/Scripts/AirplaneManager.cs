using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro; 

public class AirplaneManager : MonoBehaviour
{
    public RectTransform airPlane;
    private Vector2 initPosition;
    public Vector2 randomPosY;
    public string[] airplaneTexts;

    private TextMeshProUGUI airplaneText;

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
            airPlane.transform.position = new Vector3(initPosition.x,initPosition.y +( initPosition.y * (Random.Range(-0.1f,0.1f))), 0);
            yield return new WaitForSeconds(Random.Range(15f, 30f));
            airplaneText.text = chooseRandomText();
            airPlane.transform.DOMoveX(-Screen.width*0.6f, 20f);
        }
    }

    private string chooseRandomText()
    {
        string textToChose = airplaneTexts[Random.Range(0, airplaneTexts.Length - 1)];
        if (textToChose.Contains("$currentPlayers"))
        {
            textToChose = textToChose.Replace("$currentPlayers", GameManager.Instance.ActivePlayers.ToString());
        }
        if (textToChose.Contains("$totalPlayers"))
        {
            textToChose = textToChose.Replace("$totalPlayers", GameManager.Instance.ActivePlayers.ToString());
        }
        return textToChose;
    }
}
