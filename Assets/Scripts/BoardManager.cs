using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private List<TileManager> tiles;
    public int row, column;

    public GameObject tilePrefab;

    [Header("BUILDINGS")]
    public List<BuildingData> buildingDatas;

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
                currentTile.transform.position = new Vector2(i, j);
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
        curTile.ChangeBuilding(buildingDatas.Find(x => x.buildingType == type));
        curTile.gameObject.name = type.ToString();
    }
    
}


