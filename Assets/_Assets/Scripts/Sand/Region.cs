using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Region
{
    public int name;
    public int Label;
    public bool isMoving;
    private Dictionary<int, SandController> _sandsInRegion;
    [SerializeField] private bool _isInShape;
    [SerializeField] private int left;
    [SerializeField] private int right;

    private GameBoard GameBoard => GameBoard.Instance;
    private Dictionary<SandController, SandController> movingSandInRegion;

    public Region(int label)
    {
        name = GetHashCode();
        Label = label;
        left = int.MaxValue;
        right = int.MinValue;
        _sandsInRegion = new Dictionary<int, SandController>();
        movingSandInRegion = new Dictionary<SandController, SandController>();
    }

    public void SetShapeState(bool set)
    {
        _isInShape = set;
    }

    public void SetMovingState(bool set, SandController sandController)
    {
        isMoving = isMoving || set;

        if (MovingSandInRegion.TryGetValue(sandController, out _)) return;

        MovingSandInRegion.Add(sandController, sandController);
    }

    public void SetMovingState(bool set)
    {
        isMoving = set;
        MovingSandInRegion.Clear();
    }

    public string Name => ToString();

    public string Color => Label.ToColorString();

    // public List<Vector2Int> Points => _points;
    public Dictionary<int, SandController> SandsInRegion => _sandsInRegion;

    public int Left
    {
        get => left;
        private set => left = value;
    }

    public int Right
    {
        get => right;
        private set => right = value;
    }

    public Dictionary<SandController, SandController> MovingSandInRegion => movingSandInRegion;

    public void AddPoint(Vector2Int newPoint)
    {
        if (newPoint.y < Left)
            Left = newPoint.y;
        if (newPoint.y > Right)
            Right = newPoint.y;
        // Points.Add(newPoint);

        var sand = GameBoard.At(newPoint);
        SandsInRegion.Add(SandsInRegion.Count, sand);
    }

    public void UpdateRegionMargin()
    {
        foreach (var sand in SandsInRegion)
        {
            var position = sand.Value.Position;

            if (position.y < Left)
                Left = position.y;
            if (position.y > Right)
                Right = position.y;
        }
    }

    public void RemovePoint(SandController sandController)
    {
        int index = 0;

        foreach (var sand in SandsInRegion)
        {
            if (sand.Value != sandController) continue;

            index = sand.Key;
        }

        SandsInRegion.Remove(index);
    }

    public bool ContainSandInRegion(SandController sand)
    {
        return SandsInRegion.ContainsValue(sand);
    }

    public bool ContainSandInRegion(Vector2Int position)
    {
        var sand = GameBoard.At(position);
        return SandsInRegion.ContainsValue(sand);
    }

    public bool IsInShaped()
    {
        return _isInShape;
    }

    public override string ToString()
    {
        return $"region: {Label} - {Color} - ({Left} - {Right})";
    }

    public override bool Equals(object obj)
    {
        if (obj is Region region)
            return region.Label == Label && region.Left == Left && region.Right == Right &&
                   region.SandsInRegion.Count == SandsInRegion.Count;

        return false;
    }
}