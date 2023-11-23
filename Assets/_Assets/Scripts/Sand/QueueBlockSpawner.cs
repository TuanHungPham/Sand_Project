using System;
using System.Collections.Generic;
using MarchingBytes;
using UnityEngine;

public class QueueBlockSpawner : MonoBehaviour
{
    [SerializeField] private int _objectValue;
    [SerializeField] private QueueSandBlock _queueSandBlock;
    [SerializeField] private SandController _sandPfb;
    [SerializeField] private Transform _sandRoot;
    [SerializeField] private PositionGridView _positionGridView;
    [SerializeField] private List<SandController> _queueSandList = new List<SandController>();
    private Transform[,] queueVirtualPositionGrid;
    private int _queueRow;
    private int _queueColumn;
    private int _startColumn;
    private EasyObjectPool EasyObjectPool => EasyObjectPool.instance;
    private ShapeReader ShapeReader => ShapeReader.Instance;

    private void Awake()
    {
        LoadComponents();
        InitQueueVirtualMatrix();
    }

    private void LoadComponents()
    {
        _queueSandBlock = GetComponentInParent<QueueSandBlock>();
    }

    private void InitQueueVirtualMatrix()
    {
        var positionGrid = _positionGridView.positionGrid;
        _queueRow = positionGrid.Count;
        _queueColumn = positionGrid[0].positions.Length;

        queueVirtualPositionGrid = new Transform[_queueRow, _queueColumn];

        for (var row = 0; row < _queueRow; row++)
        for (var column = 0; column < _queueColumn; column++)
            queueVirtualPositionGrid[row, column] = positionGrid[row].positions[column];
    }


    public void CreateQueueVirtualShape()
    {
        var shape = _queueSandBlock.GetShape();
        FindStartSpawningPosition(shape, out var startRow, out var startColumn);

        for (var j = 0; j < shape.Column; j++)
        for (var i = shape.Row - 1; i >= 0; i--)
        {
            if (shape.Matrix[i, j] == 0)
                continue;
            // var sand = CreateNewSand(_queueRow - i - 1, j + 1);
            var sand = CreateNewSand(startRow - i, startColumn + j);
        }
    }

    private void FindStartSpawningPosition(Shape shape, out int startRow, out int startColumn)
    {
        var shape_middlePoint = shape.GetMiddlePointOfShape();

        var grid_middlePoint = new Vector2Int(_queueRow / 2, _queueColumn / 2);

        startRow = grid_middlePoint.x + shape_middlePoint.x;
        startColumn = grid_middlePoint.y - shape_middlePoint.y;
    }

    private SandController CreateNewSand(int row, int column)
    {
        var sand = EasyObjectPool.GetObjectFromPool(PoolName.SAND_POOL, Vector3.zero, Quaternion.identity);
        sand.transform.SetParent(_sandRoot);
        sand.gameObject.SetActive(true);

        SandController sandController = sand.GetComponent<SandController>();

        sandController.SetData(row, column, _objectValue, true, queueVirtualPositionGrid);
        _queueSandList.Add(sandController);

        return sandController;
    }

    public void DestroyQueueSand()
    {
        for (int i = _queueSandList.Count - 1; i >= 0; i--)
        {
            _queueSandList[i].DestroySand();
            _queueSandList.RemoveAt(i);
        }
    }

    public void SetObjectValue(int objectValue)
    {
        _objectValue = objectValue;
    }

    public int GetObjectValue()
    {
        return _objectValue;
    }
}