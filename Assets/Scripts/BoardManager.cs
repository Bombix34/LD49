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
    public GameManager gameManager;

    [Header("BUILDINGS")]
    public List<BuildingData> buildingDatas;

    private void Awake()
    {
        tiles = new List<TileManager>();
        SpawnBoard();
    }
    void Update()
    {
        if (Time.frameCount % 4 == 0)
        {
            if (IsBoardFull())
            {
                GameManager.Instance.WinGame();
            }
        }
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
    }

    public bool IsBoardFull()
    {
        foreach (TileManager tile in tiles) {
            if (tile.isEmpty)
            {
                return false;
            }
        }

        return true;
    }

}


