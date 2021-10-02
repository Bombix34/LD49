using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int row, column;

    public GameObject tilePrefab;

    private void Awake()
    {
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
            }
        }
    }
}
