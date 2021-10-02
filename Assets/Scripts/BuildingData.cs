using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "new data")]
public class BuildingData : ScriptableObject
{
    public Sprite sprite;

    public List<BuildingModificator> buildingModificator;
}

[System.Serializable]
public struct BuildingModificator
{
    public ResourcesTypes type;
    public int modificator;
}
