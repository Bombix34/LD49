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
            airPlane.transform.position = new Vector3(initPosition.x,initPosition.y +( initPosition.y * (Random.Range(-0.3f,0.6f))), 0);
            yield return new WaitForSeconds(Random.Range(15f, 30f));
            airplaneText.text = "Je suis un texte d'exemple";
            airPlane.transform.DOMoveX(-Screen.width/2, 10f);
        }
    }
}
