using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRenderingLayer : MonoBehaviour
{

    SpriteRenderer spriteRender;
    public int modificator;

    protected virtual void Start()
    {
        spriteRender = GetComponentInChildren<SpriteRenderer>();
        SpriteRenderer boardSprite = GetComponentInParent<SpriteRenderer>();
        float distance = Vector2.Distance(new Vector2(0, transform.position.y), new Vector2(0, 500));
        spriteRender.sortingOrder = (int)(distance)  + modificator + boardSprite.sortingOrder;
    }

}