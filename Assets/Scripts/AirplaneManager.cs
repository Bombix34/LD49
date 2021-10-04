using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AirplaneManager : MonoBehaviour
{
    public RectTransform airPlane;
    private Vector2 initPosition;
    public Vector2 randomPosY;

    private void Start()
    {
        initPosition = airPlane.transform.position;
        StartCoroutine(Behavior());
    }

    private IEnumerator Behavior()
    {
        while(true)
        {
            airPlane.transform.position = new Vector3(initPosition.x,initPosition.y +( initPosition.y * (Random.Range(-0.3f,0.6f))), 0);
            yield return new WaitForSeconds(Random.Range(15f, 30f));
            airPlane.transform.DOMoveX(-Screen.width/2, 10f);
        }
    }
}
