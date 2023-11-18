using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SandSpawner : MonoBehaviour
{
    [SerializeField] private SandController _sandPfb;

    [SerializeField] private int _startColumn;

    [SerializeField] private Transform _sandRoot;

    [SerializeField] private bool _shouldCreateNewSand;

    [SerializeField] private bool _shouldLog;

    [SerializeField] private int _objectValue = 1;

    [SerializeField] private List<SandController> _sands;
    private GameBoard _gameBoard;

    private float _lastTime;

    // private Random _ran;


    private ShapeReader _shapeReader;

    private Timer _timer;

    private GameBoard GameBoard
    {
        get
        {
            if (!_gameBoard) _gameBoard = FindObjectOfType<GameBoard>();
            return _gameBoard;
        }
    }

    private int Row => GameBoard.Row;
    private int Column => GameBoard.Column;

    private List<SandController> Sands
    {
        get
        {
            if (_sands == null) _sands = new List<SandController>();

            return _sands;
        }
        set => _sands = value;
    }

    public ShapeReader ShapeReader
    {
        get
        {
            if (!_shapeReader)
                _shapeReader = FindObjectOfType<ShapeReader>();
            return _shapeReader;
        }
        private set => _shapeReader = value;
    }

    private Timer Timer
    {
        get
        {
            if (_timer == null) _timer = new Timer();
            return _timer;
        }
    }

    private void Start()
    {
        Init();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _shouldCreateNewSand = true;
        }
        else
        {
            _shouldCreateNewSand = false;
        }

        CheckCreateNewSand();
    }

    private void Init()
    {
        _startColumn = Column / 2;
    }

    private void CreateVirtualShape(Shape shape, int startColumn)
    {
        for (var j = 0; j < shape.Column; j++)
        for (var i = shape.Row - 1; i >= 0; i--)
        {
            if (shape.Matrix[i, j] == 0)
                continue;
            var sand = CreateNewSand(Row - i - 1, j + startColumn);
        }
    }

    private SandController CreateNewSand(int row, int column)
    {
        var sand = Instantiate(_sandPfb, _sandRoot);
        sand.SetData(row, column, _objectValue);
        sand.gameObject.SetActive(true);
        AddSand(sand);
        return sand;
    }

    private void AddSand(SandController sand)
    {
        Sands.Add(sand);
        GameBoard.AddSandToMatrix(sand);
    }

    private Shape GetShape()
    {
        return ShapeReader.GetShape();
    }

    private void CreateNewSandRandomly()
    {
        var row = Row - 1;
        var column = RandomColumn();
        var sand = CreateNewSand(row, column);
    }

    private int RandomColumn()
    {
        return (int)(Random.Range(0, Column * 1000) % Column);
    }


    private void CreateNewSands(int numberOfSands)
    {
        for (var i = 0; i < numberOfSands; i++) CreateNewSandRandomly();
    }

    private void UpdateSands()
    {
        if (!HasPastInterval())
            return;
        foreach (var sand in Sands) sand.CheckMoveDown();
        foreach (var sand in Sands) sand.CheckMoveDiagonally();
    }


    private void CheckCreateNewSand()
    {
        if (_shouldCreateNewSand)
        {
            _shouldCreateNewSand = false;
            // CreateNewSands(_numberOfSandsToCreate);
            var shape = GetShape();
            CreateVirtualShape(shape, _startColumn - shape.Column / 2 - 1);
        }
    }

    private bool HasPastInterval()
    {
        return Timer.HasPastInterval();
    }

    public SandController GetSandAt(Vector2Int position)
    {
        return Sands.Find(sand => sand.At(position));
    }

    private void Log(string message)
    {
        if (!_shouldLog)
            return;
        print($"{message}");
    }
}