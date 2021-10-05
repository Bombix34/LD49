using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

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

    private void Start()
    {
       // StartCoroutine(ChangePositionCoroutine());
    }

    public void ChangeBuilding(BuildingData newBuilding)
    {
        isEmpty = false;
        sprite.sprite = newBuilding.RandomSprite;
        currentBuilding = newBuilding;
        this.transform.localScale = Vector3.zero;
        currentFX = FXs.Find(x => x.type == currentBuilding.buildingType).fxObject;
        this.transform.DOScale(1f, 1f).SetEase(Ease.InQuart)
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


    public string GetPositionText(int posX)
    {
        switch (posX)
        {
            case 0:
                return "A";
            case 1:
                return "B";
            case 2:
                return "C";
            case 3:
                return "D";
            case 4:
                return "E";
            case 5:
                return "F";
            case 6:
                return "G";
            case 7:
                return "H";
            case 8:
                return "I";
            case 9:
                return "J";
            case 10:
                return "K";
            case 11:
                return "L";
        }
        return "A";
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
