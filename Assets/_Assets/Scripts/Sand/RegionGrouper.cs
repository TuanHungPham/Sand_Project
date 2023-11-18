using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RegionGrouper : MonoBehaviour
{
    [SerializeField] private bool _shouldGroupRegions;

    [SerializeField] private List<Region> _regions;

    [SerializeField] private List<VirtualRegion> _virtualRegions;

    [SerializeField] private bool _shoudlGroupVirtualRegions;
    [SerializeField] private bool _shouldLogRegions;
    [SerializeField] private bool _shouldPauseGameOnGrouping;

    private int _colCount;
    private GameBoard _gameBoard;
    private int _groupCount;
    private int[,] _matrix;
    private int _rowCount;

    private SandSpawner _sandSpawner;
    private bool[,] _visited;

    private int BoardLeft => GameBoard.Left;
    private int BoardRight => GameBoard.Right;


    private GameBoard GameBoard
    {
        get
        {
            if (!_gameBoard)
                _gameBoard = FindObjectOfType<GameBoard>();
            return _gameBoard;
        }
    }

    private int[,] Matrix => GameBoard.LogicalMatrix;

    private List<VirtualRegion> VirtualRegions
    {
        get => _virtualRegions;
        set => _virtualRegions = value;
    }

    private SandSpawner SandSpawner
    {
        get
        {
            if (!_sandSpawner) _sandSpawner = FindObjectOfType<SandSpawner>();
            return _sandSpawner;
        }
        set => _sandSpawner = value;
    }

    private void Start()
    {
        InitializeGroupSetting();
    }

    private void InitializeGroupSetting()
    {
        _shoudlGroupVirtualRegions = true;
    }

    // Example usage
    private void Update()
    {
        Test();
    }

    private void Test()
    {
        if (!_shouldGroupRegions) return;
        _shouldGroupRegions = false;
        var inputMatrix = GetInputMatrix();

        Debug.Log($"Grouping Regions...");
        GroupRegions(inputMatrix);

        if (_shoudlGroupVirtualRegions)
            GroupVirtualRegions();

        if (_shouldLogRegions)
            LogGroups();

        CollectLineRegions();

        if (_shouldPauseGameOnGrouping)
            PauseGame();
    }

    /// <summary>
    ///     using log error to pause the Unity Editor
    /// </summary>
    private void PauseGame()
    {
        Debug.LogError("_regions are grouped!");
    }


    private void LogGroups()
    {
        for (var i = 0; i < _groupCount; i++)
        {
            var group = _regions[i];
            var message = new StringBuilder();
            message.AppendLine($"Region: {i} - {group.Color}");
            foreach (var point in group.Points) message.AppendLine($"Position: ({point.x} {point.y})");
            Log(message.ToString());
        }
    }

    private int[,] GetInputMatrix()
    {
        return Matrix;
        int[,] inputMatrix =
        {
            { 1, 1, 2, 2, 2, 2 },
            { 1, 0, 0, 2, 0, 2 },
            { 3, 3, 3, 0, 0, 2 },
            { 4, 4, 0, 0, 0, 2 }
        };
        return inputMatrix;
    }

    private void ClearVirtualGroups()
    {
        for (var i = VirtualRegions.Count - 1; i >= 0; i--)
        {
            VirtualRegions[i].Transform.DetachChildren();
            Destroy(VirtualRegions[i].Transform.gameObject);
        }

        VirtualRegions.Clear();
    }

    private void GroupVirtualRegions()
    {
        ClearVirtualGroups();
        CollectVirtualRegions();
    }

    private void CollectVirtualRegions()
    {
        foreach (var region in _regions)
        {
            var virtualRegion = CreateVirtualRegion(region);
            GroupVirtualRegion(region, virtualRegion);
        }
    }

    private void GroupVirtualRegion(Region region, VirtualRegion virtualRegion)
    {
        foreach (var position in region.Points)
        {
            var sand = SandSpawner.GetSandAt(position);
            if (sand == null)
            {
                Debug.Log("SAND NULLLLLLLLLLLLLLLLLLLLL");
                return;
            }

            sand.SetParent(virtualRegion.Transform);
        }
    }


    private VirtualRegion CreateVirtualRegion(Region region)
    {
        var virtualRegion = new VirtualRegion(region, transform);
        VirtualRegions.Add(virtualRegion);
        return virtualRegion;
    }

    public void GroupRegions(int[,] inputMatrix)
    {
        _rowCount = GetRows(inputMatrix);
        _colCount = GetColumns(inputMatrix);
        _visited = new bool[_rowCount, _colCount];
        _regions = new List<Region>();
        _groupCount = 0;

        for (var i = 0; i < _rowCount; i++)
        for (var j = 0; j < _colCount; j++)
            if (!_visited[i, j] && inputMatrix[i, j] != 0)
            {
                var label = inputMatrix[i, j];
                var group = new Region(label);
                Dfs(i, j, group);
                _regions.Add(group);
                _groupCount++;
            }
    }

    private void CollectLineRegions()
    {
        Debug.Log($"Collecting Line Regions...");
        for (int i = 0; i < _regions.Count; i++)
        {
            var region = _regions[i];

            if (IsLineRegion(region))
                CollectRegion(region);
            else
                Log($"{region}");
        }
    }

    private void CollectRegion(Region region)
    {
        CollectLogicalRegion(region);
        CollectVirtualRegion(region);
    }

    private void CollectLogicalRegion(Region region)
    {
        for (int i = region._points.Count - 1; i >= 0; i--)
        {
            SandSpawner.DestroySandAt(region._points[i]);
            region._points.Remove(region._points[i]);
        }

        _regions.Remove(region);
        Log($"Collect Logical Region: {region}");
    }

    private void CollectVirtualRegion(Region region)
    {
        var virtualRegion = VirtualRegions.Find(x => x.Region.Equals(region));
        virtualRegion?.Collect();
        Destroy(virtualRegion?.Transform.gameObject);
        VirtualRegions.Remove(virtualRegion);
        Log($"Collect Virtual Region: {region}");
    }


    private bool IsLineRegion(Region region)
    {
        return region.Left <= BoardLeft && region.Right >= BoardRight;
    }

    private static int GetColumns(int[,] inputMatrix)
    {
        return inputMatrix.GetLength(1);
    }

    private static int GetRows(int[,] inputMatrix)
    {
        return inputMatrix.GetLength(0);
    }

    private void Dfs(int i, int j, Region region)
    {
        _visited[i, j] = true;
        region.AddPoint(new Vector2Int(i, j));

        int[] dx = { 0, 0, 1, -1, 1, 1, -1, -1 };
        int[] dy = { 1, -1, 0, 0, 1, -1, 1, -1 };

        for (var k = 0; k < 8; k++)
        {
            var nx = i + dx[k];
            var ny = j + dy[k];

            if (nx >= 0 && nx < _rowCount && ny >= 0 && ny < _colCount && !_visited[nx, ny] &&
                Matrix[nx, ny] == Matrix[i, j]) Dfs(nx, ny, region);
        }
    }

    private void Log(string message)
    {
        print($"{message}");
    }
}