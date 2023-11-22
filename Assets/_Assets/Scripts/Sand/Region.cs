using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Region
{
    public int Label;
    public List<Vector2Int> _points;
    public bool isMoving;

    public Region(int label)
    {
        Label = label;
        Left = int.MaxValue;
        Right = int.MinValue;
        _points = new List<Vector2Int>();
    }

    public void SetMovingState(bool set)
    {
        isMoving = set;
    }

    public string Name => ToString();

    public string Color => Label.ToColorString();

    public List<Vector2Int> Points => _points;

    public int Left { get; private set; }

    public int Right { get; private set; }

    public void AddPoint(Vector2Int newPoint)
    {
        if (newPoint.y < Left)
            Left = newPoint.y;
        if (newPoint.y > Right)
            Right = newPoint.y;
        Points.Add(newPoint);
    }

    public override string ToString()
    {
        return $"region: {Label} - {Color} - ({Left} - {Right})";
    }

    public override bool Equals(object obj)
    {
        if (obj is Region region)
            return region.Label == Label && region.Left == Left && region.Right == Right &&
                   region.Points.Count == Points.Count;

        return false;
    }
}