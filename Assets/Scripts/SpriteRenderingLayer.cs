using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRenderingLayer : MonoBehaviour
{

    SpriteRenderer spriteRender;
    public float modificator;

    protected virtual void Start()
    {
        spriteRender = GetComponentInChildren<SpriteRenderer>();
        float parentPosY = Mathf.Abs(transform.parent.position.y)*20f;
        spriteRender.sortingOrder = (int)(((1 / (transform.position.y+17*(1/parentPosY)) * 1000) + modificator));
    }

}