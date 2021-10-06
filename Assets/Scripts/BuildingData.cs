using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "new data")]
public class BuildingData : ScriptableObject
{
    public List<Sprite> sprites;

    public BuildingTypes buildingType;

    public List<BuildingModificator> buildingModificator;

    public Sprite RandomSprite
    {
        get => sprites[Random.Range(0, sprites.Count)];
    }
}

[System.Serializable]
public struct BuildingModificator
{
    public ResourcesTypes type;
    public float modificator;
}

public enum BuildingTypes
{
    house,
    apartments,
    office,
    mall,
    factory,
    powerPlant,
    cinema,
    casino,
    road,
    forest
}
