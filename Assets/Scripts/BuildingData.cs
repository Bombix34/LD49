using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "new data")]
public class BuildingData : ScriptableObject
{
    public Sprite sprite;

    public BuildingTypes buildingType;

    public List<BuildingModificator> buildingModificator;
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
    appartments,
    office,
    mall,
    factory,
    powerPlant,
    cinema,
    casino,
    road
}
