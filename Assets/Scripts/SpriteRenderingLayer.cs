using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRenderingLayer : MonoBehaviour
{

    SpriteRenderer spriteRender;
    public float modificator;
    public bool isParentTransform;

    protected virtual void Start()
    {
        spriteRender = GetComponentInChildren<SpriteRenderer>();
        if (isParentTransform)
            spriteRender.sortingOrder = (int)(((1 / (transform.parent.position.y+17)) * 1000) + modificator);
        else
            spriteRender.sortingOrder = (int)(((1 / (transform.position.y+17)) * 1000) + modificator);
    }

}