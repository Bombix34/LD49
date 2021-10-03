using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private SpriteRenderer sprite;
    public bool isEmpty = true;

    private BuildingData currentBuilding;

    public int posX, posY;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void ChangeBuilding(BuildingData newBuilding)
    {
        isEmpty = false;
        sprite.sprite = newBuilding.sprite;
        currentBuilding = newBuilding;
    }
}
