using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesManager : Singleton<ResourcesManager>
{
    public List<ResourceData> resourcesDatas;

    public List<ResourceSlider> resourcesSliders;

    public void AddBuilding(BuildingData building)
    {
        foreach(var modif in building.buildingModificator)
        {
            ResourcesTypes type = modif.type;
            ResourceData data = resourcesDatas.Find(x => x.type == type);
            data.currentAmount += modif.modificator;
            UpdateSlider(type, data.currentAmount);
        }
    }

    public void RemoveBuilding(BuildingData building)
    {
        foreach (var modif in building.buildingModificator)
        {
            ResourcesTypes type = modif.type;
            ResourceData data = resourcesDatas.Find(x => x.type == type);
            data.currentAmount -= modif.modificator;
            UpdateSlider(type, data.currentAmount);
        }
    }

    private void UpdateSlider(ResourcesTypes type, float newValue)
    {
        ResourceSlider curSlider = resourcesSliders.Find(x => x.type == type);
        curSlider?.UpdateSlider(newValue);
    }
}

[System.Serializable]
public class ResourceData
{
    public ResourcesTypes type;
    public float currentAmount;
}

public enum ResourcesTypes
{
    POPULATION,
    MONEY,
    INDUSTRY,
    ACTIVITY
}
