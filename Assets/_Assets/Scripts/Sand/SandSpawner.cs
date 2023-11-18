using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SandSpawner : MonoBehaviour
{
    [Header("In Game Sand Block")] [SerializeField]
    private int _startColumn;

    [SerializeField] private bool _shouldLog;
    [SerializeField] private int _objectValue = 1;
    [SerializeField] private SandController _sandPfb;
    [SerializeField] private Transform _sandRoot;
    [SerializeField] private List<SandController> _sands;

    [Space(20)] [Header("In Queue Sand Block")] [SerializeField]
    private bool _shouldCreateNewSand;

    [SerializeField] private QueueSandBlock _queueSandBlock;
    [SerializeField] private Transform _sandQueue;
    [SerializeField] private List<QueueSandBlock> _queueSandBlockList = new List<QueueSandBlock>();
    [SerializeField] private QueuePointManager _queuePointManager;

    private GameBoard _gameBoard;

    private float _lastTime;

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

    private int OnBoardRow => GameBoard.OnBoardRow;
    private int OnBoardColumn => GameBoard.OnBoardColumn;

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
        _startColumn = OnBoardColumn / 2;
    }

    private void CreateQueueSand()
    {
        foreach (Transform point in _queuePointManager.GetPointList())
        {
            QueueSandBlock queueSandBlock = Instantiate(_queueSandBlock, _sandQueue, true);
            queueSandBlock.transform.position = point.position;

            _queueSandBlockList.Add(queueSandBlock);
        }
    }

    private void CreateOnBoardVirtualShape(Shape shape, int startColumn)
    {
        for (var j = 0; j < shape.Column; j++)
        for (var i = shape.Row - 1; i >= 0; i--)
        {
            if (shape.Matrix[i, j] == 0)
                continue;
            var sand = CreateNewSand(OnBoardRow - i - 1, j + startColumn);
        }
    }

    private SandController CreateNewSand(int row, int column, bool isQueueBlock = false)
    {
        var sand = Instantiate(_sandPfb, _sandRoot);
        sand.gameObject.SetActive(true);

        if (!isQueueBlock)
        {
            sand.SetData(row, column, _objectValue);
            AddSand(sand);
        }
        // else
        // {
        //     sand.SetData(row, column, _objectValue, true);
        // }

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
        var row = OnBoardRow - 1;
        var column = RandomColumn();
        var sand = CreateNewSand(row, column);
    }

    private int RandomColumn()
    {
        return (int)(Random.Range(0, OnBoardColumn * 1000) % OnBoardColumn);
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

            CreateQueueSand();
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

    public void DestroySandAt(Vector2Int position)
    {
        var sand = Sands.Find(sand => sand.At(position));
        Sands.Remove(sand);
        sand.Destroy();
    }

    private void Log(string message)
    {
        if (!_shouldLog)
            return;
        print($"{message}");
    }

    public void CreateOnBoardVirtualShape(int startColumn)
    {
        var shape = GetShape();
        int _startColumn = startColumn - shape.Column / 2 - 1;

        for (var j = 0; j < shape.Column; j++)
        for (var i = shape.Row - 1; i >= 0; i--)
        {
            if (shape.Matrix[i, j] == 0)
                continue;
            var sand = CreateNewSand(OnBoardRow - i - 1, j + _startColumn);
        }
    }

    public List<QueueSandBlock> GetQueueSandBlockList()
    {
        return _queueSandBlockList;
    }
}