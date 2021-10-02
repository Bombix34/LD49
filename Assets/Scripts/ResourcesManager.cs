using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesManager : MonoBehaviour
{
    public List<ResourceData> resourcesDatas;


}

[System.Serializable]
public struct ResourceData
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
