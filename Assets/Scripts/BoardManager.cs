using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private List<TileManager> tiles;
    public int row, column;

    public float padX, padY;
    public float indexPadX, indexPadY;

    public GameObject tilePrefab;

    [Header("BUILDINGS")]
    public List<BuildingData> buildingDatas;

    public List<RoadData> roadDatas;

    private void Awake()
    {
        tiles = new List<TileManager>();
        SpawnBoard();
    }

    private void SpawnBoard()
    {
        for(int i = 0; i < column; ++i)
        {
            for(int j = 0; j < row; ++j)
            {
                GameObject currentTile = Instantiate(tilePrefab, this.transform);
                currentTile.transform.position = new Vector2(i*padX + (j*indexPadX), i*padY+(j*indexPadY));
                currentTile.GetComponent<TileManager>().posX = i;
                currentTile.GetComponent<TileManager>().posY = j;
                tiles.Add(currentTile.GetComponent<TileManager>());
            }
        }
    }

    public void SpawnBuilding(Vector2 position, BuildingTypes type)
    {
        int posX = (int)position.x;
        int posY = (int)position.y;
        TileManager curTile = tiles.Find(x => x.posX == posX && x.posY == posY);
        if (curTile == null || !curTile.isEmpty)
            return;
        BuildingData curData = buildingDatas.Find(x => x.buildingType == type);
        curTile.ChangeBuilding(curData);
        curTile.gameObject.name = type.ToString();
        ResourcesManager.Instance.AddBuilding(curData);
        CheckRoads();
    }

    public void RemoveBuilding(Vector2 position)
    {
        int posX = (int)position.x;
        int posY = (int)position.y;
        TileManager curTile = tiles.Find(x => x.posX == posX && x.posY == posY);
        if (curTile == null || curTile.isEmpty)
            return;
        BuildingData currentBuilding = curTile.CurrentBuilding;
        curTile.RemoveBuilding();
        ResourcesManager.Instance.RemoveBuilding(currentBuilding);
        CheckRoads();
    }

    public TileManager GetTile(int posX, int posY)
    {
        return tiles.Find(x => x.posX == posX && x.posY == posY);
    }

    public List<TileManager> GetVoisins(int posX, int posY)
    {
        List<TileManager> voisins = new List<TileManager>();
        for(int i =-1; i < 2; ++i)
        {
            for(int j = -1; j < 2; ++j)
            {
                if ((i == 0 && j == 0)||(i==-1 && j==-1) || (i == -1 && j == 1) || (i == 1 && j == -1) || (i == 1 && j == 1))
                    continue;
                TileManager tile = GetTile(posX + i, posY + j);
                if(tile!=null)
                    voisins.Add(tile);
            }
        }
        return voisins;
    }

    public void CheckRoads()
    {
        List<TileManager> roads = tiles.FindAll(x =>x.CurrentBuilding != null && x.CurrentBuilding.buildingType == BuildingTypes.road);
        foreach(var tileRoad in roads)
        {
            int value = 0;
            TileManager upTile = GetTile(tileRoad.posX + 0, tileRoad.posY + 1);
            if (upTile != null && upTile.CurrentBuilding!=null && upTile?.CurrentBuilding.buildingType == BuildingTypes.road)
                value += 5;
            TileManager downTile = GetTile(tileRoad.posX + 0, tileRoad.posY - 1);
            if (downTile != null && downTile.CurrentBuilding != null && downTile?.CurrentBuilding.buildingType == BuildingTypes.road)
                value += 1;
            TileManager leftTile = GetTile(tileRoad.posX - 1, tileRoad.posY + 0);
            if (leftTile != null && leftTile.CurrentBuilding != null && leftTile?.CurrentBuilding.buildingType == BuildingTypes.road)
                value += 2;
            TileManager rightTile = GetTile(tileRoad.posX + 1, tileRoad.posY + 0);
            if (rightTile != null && rightTile.CurrentBuilding != null && rightTile?.CurrentBuilding.buildingType == BuildingTypes.road)
                value += 9;

            List<RoadData> data = roadDatas.FindAll(x => x.value == value);
            if(data.Count > 0)
            {
                tileRoad.GetComponentInChildren<SpriteRenderer>().sprite = data[0].sprite;
                //tileRoad.transform.localScale = new Vector2(tileRoad.transform.localScale.x * data[0].scaleX, tileRoad.transform.localScale.x);
            }
            else
            {
                tileRoad.GetComponentInChildren<SpriteRenderer>().sprite = roadDatas[5].sprite;
                //tileRoad.transform.localScale = new Vector2(tileRoad.transform.localScale.x * roadDatas[5].scaleX, tileRoad.transform.localScale.x);
            }
        }
    }
}

[System.Serializable]
public struct RoadData
{
    public int value;
    public Sprite sprite;
    public int scaleX;
}


