using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private SpriteRenderer sprite;
    public bool isEmpty = true;

    private BuildingData currentBuilding = null;

    public int posX, posY;

    private int sortingOrder = 0;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        //sprite.sprite = null;
    }
    private void Start()
    {
        sprite.sortingOrder = (int)(1/(transform.position.y+50));
    }

    public void ChangeBuilding(BuildingData newBuilding)
    {
        isEmpty = false;
        sprite.sprite = newBuilding.sprite;
        currentBuilding = newBuilding;
    }

    public void RemoveBuilding()
    {

        isEmpty = true;
        sprite.sprite = null;
        currentBuilding = null;
    }

    public BuildingData CurrentBuilding
    {
        get => currentBuilding;
    }
}
