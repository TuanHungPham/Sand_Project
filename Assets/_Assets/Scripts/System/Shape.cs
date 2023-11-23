using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Shape
{
    public readonly int Column;

    public readonly int Row;
    public int[,] Matrix;

    public Shape(List<List<string>> shapeData)
    {
        Row = shapeData.Count;
        Column = shapeData[0].Count;
        Matrix = new int[Row, Column];
        for (var i = 0; i < Row; i++)
        for (var j = 0; j < Column; j++)
            Matrix[i, j] = 1;
    }

    public Vector2Int GetMiddlePointOfShape()
    {
        int middleRow = Row / 2;
        int middleColumn = Column / 2;
        return new Vector2Int(middleRow, middleColumn);
    }

    public override string ToString()
    {
        var row = new StringBuilder();
        for (var i = 0; i < Matrix.GetLength(0); i++)
        {
            row.Append("| ");
            for (var j = 0; j < Matrix.GetLength(1); j++)
                row.Append($"{Matrix[i, j]}, ");
            row.Remove(row.Length - 2, 2);
            row.Append(" |");
            row.AppendLine();
        }

        return row.ToString();
    }
}