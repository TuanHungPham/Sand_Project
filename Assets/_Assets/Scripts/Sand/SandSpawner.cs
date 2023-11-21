using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SandSpawner : TemporaryMonoSingleton<SandSpawner>
{
    [Header("In Game Sand Block")] [SerializeField]
    private int _startColumn;

    [SerializeField] private bool _shouldLog;
    [SerializeField] private int _objectValue = 1;
    [SerializeField] private SandController _sandPfb;
    [SerializeField] private Transform _sandRoot;
    [SerializeField] private List<SandController> _sands;
    private Dictionary<SandController, int> _sandDictionary;

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

    private GameBoard GameBoard => GameBoard.Instance;
    private SandMatrix SandMatrix => GameBoard.SandMatrix;

    private int OnBoardRow => GameBoard.OnBoardRow;
    private int OnBoardColumn => GameBoard.OnBoardColumn;

    private Dictionary<SandController, int> SandsDictionary
    {
        get
        {
            if (_sandDictionary == null) _sandDictionary = new Dictionary<SandController, int>();

            return _sandDictionary;
        }
        set => _sandDictionary = value;
    }

    private List<SandController> Sands
    {
        get
        {
            if (_sands == null) _sands = new List<SandController>();

            return _sands;
        }
        set => _sands = value;
    }

    public ShapeReader ShapeReader => ShapeReader.Instance;

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

        CheckCreateNewQueueSand();
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
            queueSandBlock.SetShape(ShapeReader.GetShape());

            _queueSandBlockList.Add(queueSandBlock);
        }
    }

    public void CreateOnBoardVirtualShape(Shape shape, int startColumn)
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

        return sand;
    }

    private void AddSand(SandController sand)
    {
        // sand.SetSandIndex(Sands.Count);
        SandsDictionary.Add(sand, 0);
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

    // private void UpdateSands()
    // {
    //     if (!HasPastInterval())
    //         return;
    //     foreach (var sand in SandsDictionary) sand.Key.CheckMoveDown();
    //     foreach (var sand in SandsDictionary) sand.Key.CheckMoveDiagonally();
    // }


    private void CheckCreateNewQueueSand()
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
        return SandMatrix.At(position);
    }

    public void DestroySand(SandController sand)
    {
        SandsDictionary.Remove(sand);
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