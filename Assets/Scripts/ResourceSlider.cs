using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResourceSlider : MonoBehaviour
{
    public ResourcesTypes type;
    public Image fillArea;
    private Slider slider;

    private Color baseColor;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        baseColor = fillArea.color;
    }

    public void UpdateSlider(float resourceValue)
    {
        if (slider.value < resourceValue)
            fillArea .color= Color.green;
        else
            fillArea.color = Color.red;
        slider.DORewind();
        slider.DOValue(resourceValue, 0.3f)
            .OnComplete(() => fillArea.color = baseColor);
    }
}
