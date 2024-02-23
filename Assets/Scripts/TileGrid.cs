using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileGrid : MonoBehaviour,ICloneable
{
    public TileCell[,] tileCells;
    public List<Vector2Int> emptyCells; // 空格子

    private static readonly int rowMax = 4;
    private static readonly int cellMax = 4;

    public void InitGrid()
    {
        TileCell[] cells = GetComponentsInChildren<TileCell>();
        tileCells = new TileCell[rowMax,cellMax];
        for (int i = 0; i < rowMax; i++)
        {
            for (int j = 0; j < cellMax; j++)
            {
                TileCell cell = cells[i*cellMax+j];
                 tileCells[i,j] = cell;
                 emptyCells.Add(new Vector2Int(i,j));
            }
        }
    }

    public TileCell RandomEmptyTileCell()
    {
        int randNum = RandomUtils.GetRandomInt(0,emptyCells.Count);
        Vector2Int vector2Int = emptyCells[randNum];
        emptyCells.RemoveAt(randNum);
        return tileCells[vector2Int.x,vector2Int.y];
    }

    public TileCell GetTileCell(Vector2Int point)
    {
        if (point.x >= 0 && point.x < rowMax && point.y >= 0 && point.y < cellMax)
        {
             return tileCells[point.x, point.y];
        }
        return null;
    }

    public bool ChecKMoveVector2IntLimit(Vector2Int point)
    {
        if (point.x < 0 || point.x >= rowMax || point.y < 0 || point.y >= cellMax)
        {
            return false;
        }
        return true;
    }

    public object Clone()
    {
        TileGrid clone = new TileGrid
        {
            tileCells = (TileCell[,])tileCells.Clone(),
            emptyCells = new List<Vector2Int>(emptyCells)
        };
        return clone;
    }
}
