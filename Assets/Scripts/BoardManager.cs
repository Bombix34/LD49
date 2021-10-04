using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    public List<RoadData> roadDatas;

    [Header("QUARTIERS")]
    public GameObject hoodPrefab;
    public BoardData currentHood;
    private List<BoardData> allHoods;
    public List<Vector2> movementHoodPositions;
    public List<Vector2> movementSequence;
    private int currentIndexSequence = 0;
    private Vector2 initialOffset;

    private void Awake()
    {
        tiles = new List<TileManager>();
        allHoods = new List<BoardData>();
        allHoods.Add(currentHood);
        initialOffset = currentHood.board.transform.position;
        SpawnTiles();
    }

    private void NewBoard()
    {
        //droite bas gauche haut
        int curPosX = currentHood.posX;
        int curPosY = currentHood.posY;
        int dirX = (int)movementSequence[currentIndexSequence].x;
        int dirY = (int)movementSequence[currentIndexSequence].y;
        int finalPosX = curPosX + dirX;
        int finalPosY = curPosY + dirY;

        int nextIndex = currentIndexSequence+1;
        if (nextIndex > 3)
            nextIndex = 0;
        int nextDirX = (int)movementSequence[nextIndex].x;
        int nextDirY = (int)movementSequence[nextIndex].y;
        int nextFinalPosX = curPosX + nextDirX;
        int nextFinalPosY = curPosY + nextDirY;

        List<BoardData> concernedBoard = allHoods.FindAll(x => x.posX == finalPosX && x.posY == finalPosY);
        if( concernedBoard.Count == 0 )
        {
            List<BoardData> roationConcernedBoard = allHoods.FindAll(x => x.posX == nextFinalPosX && x.posY == nextFinalPosY);
            if(roationConcernedBoard.Count == 0 )
            {
                currentIndexSequence++;
                if (currentIndexSequence > 3)
                    currentIndexSequence = 0;
                finalPosX = nextFinalPosX;
                finalPosY = nextFinalPosY;
                dirX = nextDirX;
                dirY = nextDirY;
            }
        }

        float multiplicatorX = movementHoodPositions[currentIndexSequence].x;
        float multiplicatorY = movementHoodPositions[currentIndexSequence].y;

        tiles.Clear();

        int newSortingOrder = currentHood.sortingOrder;

        if(allHoods.Count == 1)
        {
            finalPosX = curPosX + 1;
            finalPosY = curPosY +0;
            dirX = 1;
            dirY = 0;
            currentIndexSequence = 0;
            multiplicatorX = movementHoodPositions[currentIndexSequence].x;
            multiplicatorY = movementHoodPositions[currentIndexSequence].y;
        }
        if (currentIndexSequence < 2)
            newSortingOrder++;
        else
            newSortingOrder--;

        Vector2 currentBoardWorldPosition = currentHood.board.transform.position;
        Vector2 newPosition = new Vector2(
            currentBoardWorldPosition.x + movementHoodPositions[currentIndexSequence].x,
            currentBoardWorldPosition.y + movementHoodPositions[currentIndexSequence].y);

        GameObject newBoard = Instantiate(hoodPrefab, this.transform);
        newBoard.name = "Hood_" + finalPosX + "x" + finalPosY;
        newBoard.transform.position = newPosition;
        newBoard.transform.localScale = Vector2.zero;
        newBoard.transform.DOScale(1f, 1f).OnComplete(()=>{ SpawnTiles();});
        currentHood = new BoardData(newBoard, finalPosX, finalPosY, newSortingOrder);
        allHoods.Add(currentHood);
        CameraManager.Instance.target = newBoard.transform;
    }

    private void SpawnTiles(bool isFirst=false)
    {
        for(int i = 0; i < column; ++i)
        {
            for(int j = 0; j < row; ++j)
            {
                GameObject currentTile = Instantiate(tilePrefab, this.currentHood.board.transform);
                if(isFirst)
                    currentTile.transform.position = new Vector2(i*padX + (j*indexPadX), i*padY+(j*indexPadY));
                else
                {
                    Vector2 hoodPosition = currentHood.board.transform.position;
                    currentTile.transform.position = new Vector2((hoodPosition.x- initialOffset.x) + (i * padX + (j * indexPadX)), (hoodPosition.y-initialOffset.y)+(i * padY + (j * indexPadY)));
                }
                Debug.Log(currentTile.transform.position);
                currentTile.GetComponent<TileManager>().posX = i;
                currentTile.GetComponent<TileManager>().posY = j;
                tiles.Add(currentTile.GetComponent<TileManager>());
            }
        }
        StartCoroutine(SpawnForest());
    }

    private IEnumerator SpawnForest()
    {
        int forestNumber = Random.Range(10, 20);
        List<TileManager> chosenTiles = new List<TileManager>();
        for(int i = 0; i < forestNumber; ++i)
        {
            int randX = Random.Range(0, column);
            int randY = Random.Range(0, row);
            TileManager curTile = GetTile(randX, randY);
            if(chosenTiles.Contains(curTile))
            {
                forestNumber++;
                continue;
            }
            chosenTiles.Add(curTile);
            curTile.ChangeBuilding(buildingDatas.Find(x => x.buildingType == BuildingTypes.forest));
            yield return new WaitForSeconds(0.05f);
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
        if (IsBoardFull())
        {
            NewBoard();
        }
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

    public TileManager GetTile(int posX, int posY)
    {
        return tiles.Find(x => x.posX == posX && x.posY == posY);
    }

    public void CheckRoads()
    {
        List<TileManager> roads = tiles.FindAll(x => x.CurrentBuilding != null && x.CurrentBuilding.buildingType == BuildingTypes.road);
        foreach (var tileRoad in roads)
        {
            int value = 0;
            TileManager upTile = GetTile(tileRoad.posX + 0, tileRoad.posY + 1);
            if (upTile != null && upTile.CurrentBuilding != null && upTile?.CurrentBuilding.buildingType == BuildingTypes.road)
                value += 5;
            else if (tileRoad.posY + 1 >= row)
                value += 5;

            TileManager downTile = GetTile(tileRoad.posX + 0, tileRoad.posY - 1);
            if (downTile != null && downTile.CurrentBuilding != null && downTile?.CurrentBuilding.buildingType == BuildingTypes.road)
                value += 1;
            else if (tileRoad.posY - 1 < 0)
                value += 1;

            TileManager leftTile = GetTile(tileRoad.posX - 1, tileRoad.posY + 0);
            if (leftTile != null && leftTile.CurrentBuilding != null && leftTile?.CurrentBuilding.buildingType == BuildingTypes.road)
                value += 2;
            else if (tileRoad.posX - 1 < 0 )
                value += 2;

            TileManager rightTile = GetTile(tileRoad.posX + 1, tileRoad.posY + 0);
            if (rightTile != null && rightTile.CurrentBuilding != null && rightTile?.CurrentBuilding.buildingType == BuildingTypes.road)
                value += 9;
            else if (tileRoad.posX + 1 >= column)
                value += 9;

            List<RoadData> data = roadDatas.FindAll(x => x.value == value);
            if (data.Count > 0)
                tileRoad.GetComponentInChildren<SpriteRenderer>().sprite = data[0].sprite;
            else
                tileRoad.GetComponentInChildren<SpriteRenderer>().sprite = roadDatas[5].sprite;
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

[System.Serializable]
public class BoardData
{
    public GameObject board;
    public int posX, posY;
    public int sortingOrder;

    public BoardData(GameObject board, int x, int y, int sortingOrder)
    {
        this.board = board;
        posX = x;
        posY = y;
        this.sortingOrder = sortingOrder;
        board.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
    }
}

