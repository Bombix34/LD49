using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourcesManager : Singleton<ResourcesManager>
{
    public int currentPopulation = 0;
    public TextMeshProUGUI populationText;

    public TextMeshProUGUI activePlayerText;

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
        if (building.buildingType == BuildingTypes.house)
            UpdatePopulation(10);
        else if (building.buildingType == BuildingTypes.apartments)
            UpdatePopulation(30);
        CheckResources();
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
        CheckResources();
    }

    //Calls LoseGame function if any resource value is at 0 or 1 (min or max)
    public void CheckResources()
    {
        foreach (var resource in resourcesDatas)
        {
            if (resource.currentAmount >= 1)
            {
                GameManager.Instance.LoseGame(resource.type, true);
            }
            else if (resource.currentAmount <= 0)
            {
                GameManager.Instance.LoseGame(resource.type, false);
            }
        }
    }

    private void UpdateSlider(ResourcesTypes type, float newValue)
    {
        ResourceSlider curSlider = resourcesSliders.Find(x => x.type == type);
        curSlider?.UpdateSlider(newValue);
    }

    public void UpdateActivePlayers(int amount)
    {
        activePlayerText.text = amount.ToString();
    }

    public void UpdatePopulation(int amount)
    {
        currentPopulation += amount;
        populationText.text = currentPopulation.ToString("### ### ##0");
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
