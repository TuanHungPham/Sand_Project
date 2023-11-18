using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SandMatrix
{
    public int _column;
    public int _row;
    public SandController[,] Matrix;

    public SandMatrix(int row, int column)
    {
        _row = row;
        _column = column;
        Matrix = new SandController[row, column];
        for (var i = 0; i < row; i++)
        for (var j = 0; j < column; j++)
            Matrix[i, j] = null;
    }


    public SandController At(Vector2Int position)
    {
        return Matrix[position.x, position.y];
    }

    public void Set(Vector2Int position, SandController sand)
    {
        Matrix[position.x, position.y] = sand;
    }

    public List<SandController> AtRow(int row)
    {
        var selectedRow = new List<SandController>();
        var left = 0;
        var right = _column - 1;
        var count = 0;
        // while (left < right)
        // {
        //     count++;
        //     while (!Matrix[row, right] && left < right) right--;
        //     if (left >= right)
        //         break;
        //     if (Matrix[row, right])
        //         selectedRow.Add(Matrix[row, right]);
        //
        //     while (!Matrix[row, left] && left < right) left++;
        //     if (left >= right)
        //         break;
        //     if (Matrix[row, left])
        //         selectedRow.Add(Matrix[row, left]);
        //
        //     if (count > _column)
        //         break;
        // }

        // for (var i = 0; i < _column; i++)
        for (var i = _column - 1; i >= 0; i--)
            if (Matrix[row, i])
                selectedRow.Add(Matrix[row, i]);
        return selectedRow;
    }

    public List<SandController> AtColumn(int column)
    {
        var selectedColumn = new List<SandController>();
        for (var i = 0; i < _row; i++)
            if (Matrix[i, column])
                selectedColumn.Add(Matrix[i, column]);
        return selectedColumn;
    }

    public void AddSand(SandController sand)
    {
        Matrix[sand.Position.x, sand.Position.y] = sand;
    }

    public void UpdateSand(SandController sand, Vector2Int exPosition)
    {
        Matrix[exPosition.x, exPosition.y] = null;
        if (sand == null) return;
        Matrix[sand.Position.x, sand.Position.y] = sand;
    }
}