using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    public CanvasGroup gameOverPanel; // 游戏结束面板
    public GameManager gameManager; // 游戏管理器
    public TileGrid tileGrid; // 游戏格子管理
    public TileState[] tileStates; // 所有格子数字

    public Tile tilePrefab;

    public List<Tile> tileClones; // 复制体

    public int curScore = 0; // 当前分数

    // 0=close,1=open
    public void GameOverPanelSwitch(int state)
    {
        if (state == 0)
        {
            gameOverPanel.alpha = 0;
            gameOverPanel.interactable = false;
        }else
        {
            gameOverPanel.alpha = 1;
            gameOverPanel.interactable = true;
        }
    }

    public void InitGame()
    {
        Destroy();
        tileGrid.InitGrid();
        CreateRandomTile();
        CreateRandomTile();
    }

    public void Destroy()
    {
        for (int i = 0; i < tileClones.Count; i++)
        {
            Destroy(tileClones[i].gameObject);
        }
        tileClones.Clear();
    }

    public void CreateRandomTile()
    {
        TileCell cell = tileGrid.RandomEmptyTileCell();
        CreateNewTile(cell);
        TileState state = tileStates[RandomUtils.GetRandomInt(0,1)];
        cell.tile.SetTileInfo(state, cell);
        gameManager.UpdateScore(state.number);
    }

    public void CreateNewTile(TileCell cell)
    {
        Tile tile = Instantiate(tilePrefab, tileGrid.transform);
        tileClones.Add(tile);
        cell.tile = tile;
    }

    public TileState GetTileState(string txtNumber)
    {
        for (int i = 0; i < tileStates.Length; i++)
        {
            if (tileStates[i].number == int.Parse(txtNumber))
            {
               return tileStates[i];
            }
        }
        return null;
    }

    public void CheckGameOver()
    {
        if (tileGrid.emptyCells.Count > 0)
        {
            return;
        }
        if (CheckMove())
        {
            return;
        }
        gameManager.GameOver(curScore);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            bool moveResult = Move(tileGrid, new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(0,1));
            if (moveResult)
            {
                CreateRandomTile();
                CheckGameOver();
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            bool moveResult = Move(tileGrid, new Vector2Int(3,0), new Vector2Int(-1,0), new Vector2Int(0,1));
            if (moveResult)
            {
                 CreateRandomTile();
                 CheckGameOver();
            }
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            bool moveResult = Move(tileGrid, new Vector2Int(0,0),new Vector2Int(0,1), new Vector2Int(1,0));
            if (moveResult)
            {
                 CreateRandomTile();
                 CheckGameOver();
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            bool moveResult = Move(tileGrid, new Vector2Int(0,3), new Vector2Int(0,-1), new Vector2Int(1,0));
            if (moveResult)
            {
                 CreateRandomTile();
                 CheckGameOver();
            }
        }
    }

    public bool CheckMove()
    {
        TileGrid copyGrid = (TileGrid)tileGrid.Clone();
        bool checkW = Move(copyGrid, new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(0,1));
        if (checkW)
        {
            return true;
        }
        bool checkS = Move(copyGrid, new Vector2Int(3,0), new Vector2Int(-1,0), new Vector2Int(0,1));
        if (checkS)
        {
            return true;
        }
        bool checkA = Move(copyGrid, new Vector2Int(0,0),new Vector2Int(0,1), new Vector2Int(1,0));
        if (checkA)
        {
            return true;
        }
        bool checkD = Move(copyGrid, new Vector2Int(0,3), new Vector2Int(0,-1), new Vector2Int(1,0));
        if (checkD)
        {
            return true;
        }
        return false;
    }

    public bool Move(TileGrid tileGrid, Vector2Int start, Vector2Int direction, Vector2Int tab)
    {
        bool moveResult = false;
        Vector2Int begin = new Vector2Int(start.x, start.y);
        while (tileGrid.ChecKMoveVector2IntLimit(begin))
        {
            TileCell beginPoint = tileGrid.GetTileCell(begin);
            Vector2Int check = begin + direction;
            while (tileGrid.ChecKMoveVector2IntLimit(check))
            {
                TileCell checkPoint = tileGrid.GetTileCell(check);
                if (beginPoint.tile == null && checkPoint.tile != null)
                {
                    // 移动
                    CreateNewTile(beginPoint);
                    beginPoint.tile.SetTileInfo(GetTileState(checkPoint.tile.txtNumber.text), beginPoint);
                    tileGrid.emptyCells.Add(check);
                    tileGrid.emptyCells.Remove(begin);
                    tileClones.Remove(checkPoint.tile);
                    Destroy(checkPoint.tile.gameObject);
                    checkPoint.tile = null;
                    moveResult = true;
                }
                else if (beginPoint.tile != null && checkPoint.tile != null)
                {
                    if (beginPoint.tile.txtNumber.text == checkPoint.tile.txtNumber.text)
                    {
                        // 合并
                        int beginNum = int.Parse(beginPoint.tile.txtNumber.text);
                        int checkNum = int.Parse(checkPoint.tile.txtNumber.text);
                        int resultNum = beginNum + checkNum;

                        tileClones.Remove(checkPoint.tile);
                        Destroy(checkPoint.tile.gameObject);
                        checkPoint.tile = null;
                        tileGrid.emptyCells.Add(check);

                        tileClones.Remove(beginPoint.tile);
                        Destroy(beginPoint.tile.gameObject);
                        beginPoint.tile = null;
                        
                        CreateNewTile(beginPoint);
                        beginPoint.tile.SetTileInfo(GetTileState(resultNum.ToString()), beginPoint);
                        moveResult = true;

                        gameManager.UpdateScore(resultNum/2);
                    }
                    else {
                        break;
                    }
                }
                check += direction;
            }
            begin += direction;
        }
        start += tab;
        if (tileGrid.ChecKMoveVector2IntLimit(start))
        {
            bool loopMoveResult = Move(tileGrid, start, direction, tab);
            moveResult = moveResult == false ? loopMoveResult : moveResult;
        }
        return moveResult;
    }
}
