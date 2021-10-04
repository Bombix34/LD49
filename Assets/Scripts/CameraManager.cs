using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CameraManager : Singleton<CameraManager>
{
    public Transform target;
    public float travelingSpeed = 1f;
    private float posZ;

    public Vector3 offset;

    public List<Text> worldFeedbackTexts;
    public SpriteRenderer gridRenderer;

    private bool isNearTarget = false;

    private void Awake()
    {
        posZ = this.transform.position.z;
    }

    private void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, target.position+offset, Time.deltaTime* travelingSpeed);
        this.transform.position = new Vector3(transform.position.x, transform.position.y, posZ);
        if (Vector2.Distance(this.transform.position, target.position) < 9f)
        {
            if (!isNearTarget)
                DisplayFeedbacksElements(true);
            isNearTarget = true;
        }
        else
        {
            if (isNearTarget)
                DisplayFeedbacksElements(false);
            isNearTarget = false;
        }
    }

    private void DisplayFeedbacksElements(bool isDisplay)
    {
        float finalValue = isDisplay ? 0.5f : 0f;
        foreach(var text in worldFeedbackTexts)
        {
            text.DOFade(finalValue, 1f);
        }
        gridRenderer.DOFade(finalValue, 1.3f);
    }
}
