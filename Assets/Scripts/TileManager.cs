using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileManager : MonoBehaviour
{
    private SpriteRenderer sprite;
    public bool isEmpty = true;

    private BuildingData currentBuilding;

    public int posX, posY;

    public List<TileFX> FXs;
    private GameObject currentFX;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        //sprite.sprite = null;
    }

    public void ChangeBuilding(BuildingData newBuilding)
    {
        isEmpty = false;
        sprite.sprite = newBuilding.RandomSprite;
        currentBuilding = newBuilding;
        this.transform.localScale = Vector3.zero;
        currentFX = FXs.Find(x => x.type == currentBuilding.buildingType).fxObject;
        this.transform.DOScale(1f, 1f).SetEase(Ease.InBounce)
            .OnComplete(() => currentFX?.SetActive(true));
    }

    public void RemoveBuilding()
    {
        isEmpty = true;
        sprite.sprite = null;
        currentBuilding = null;
        currentFX?.SetActive(false);
        this.transform.DOScale(0f, 1f).SetEase(Ease.OutElastic);
    }

    public BuildingData CurrentBuilding
    {
        get => currentBuilding;
    }
}

[System.Serializable]
public struct TileFX
{
    public GameObject fxObject;
    public BuildingTypes type;
}
