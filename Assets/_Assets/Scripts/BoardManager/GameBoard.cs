using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

public class GameBoard : TemporaryMonoSingleton<GameBoard>
{
    public PositionGridView onBoardPositionGridView;
    public int Left;
    public int Right;
    [SerializeField] private bool _isUpdateMatrixFromHierarchy = true;

    public List<LogicalRow> matrix;

    [SerializeField] private SandMatrix _sandMatrix;
    [SerializeField] private int[,] _logicalMatrix; // Logical matrix representing different kinds of objects
    private readonly int _emptyValue = 0; // The value representing an empty position in the logical matrix

    private int emptyValue = 0; // The value representing an empty position in the logical matrix

    public int[,] LogicalMatrix
    {
        get => _logicalMatrix;
        private set => _logicalMatrix = value;
    }

    public Transform[,] OnBoardVirtualPositionGrid { get; set; }
    public int OnBoardRow { get; private set; }
    public int OnBoardColumn { get; private set; }
    public int QueueRow { get; private set; }
    public int QueueColumn { get; private set; }

    public SandMatrix SandMatrix
    {
        get => _sandMatrix;
        private set => _sandMatrix = value;
    }

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        if (_isUpdateMatrixFromHierarchy)
        {
            _isUpdateMatrixFromHierarchy = false;
            InitLogicalMatrix();
        }
    }

    private void Initialize()
    {
        InitOnBoardVirtualMatrix();

        InitSandMatrix();
        InitLogicalMatrix();

        if (Right <= 0) Right = OnBoardColumn - 1;
        if (Left < 0)
            Left = 0;
    }

    private void InitOnBoardVirtualMatrix()
    {
        var positionGrid = onBoardPositionGridView.positionGrid;
        OnBoardRow = positionGrid.Count;
        OnBoardColumn = positionGrid[0].positions.Length;

        OnBoardVirtualPositionGrid = new Transform[OnBoardRow, OnBoardColumn];

        for (var row = 0; row < OnBoardRow; row++)
        for (var column = 0; column < OnBoardColumn; column++)
            OnBoardVirtualPositionGrid[row, column] = positionGrid[row].positions[column];
    }

    private void InitSandMatrix()
    {
        SandMatrix = new SandMatrix(OnBoardRow, OnBoardColumn);
    }

    private void InitLogicalMatrix()
    {
        // Initialize the logical matrix with empty positions
        LogicalMatrix = new int[OnBoardRow, OnBoardColumn];
        for (var row = 0; row < OnBoardRow; row++)
        for (var column = 0; column < OnBoardColumn; column++)
            LogicalMatrix[row, column] = matrix[row].points[column];

        LogLogicalMatrix();
    }

    private void LogLogicalMatrix()
    {
        var log = new StringBuilder();
        for (var row = 0; row < OnBoardRow; row++)
        {
            log.Append("(");
            for (var column = 0; column < OnBoardColumn; column++) log.Append($"{LogicalMatrix[row, column]}, ");

            log.Remove(log.Length - 2, 2);
            log.Append(")\n");
        }

        print(log.ToString());
    }

    public void AddSandToMatrix(SandController sand)
    {
        SandMatrix.AddSand(sand);
    }

    public void UpdateSandMatrix(SandController sand, Vector2Int exPosition)
    {
        SandMatrix.UpdateSand(sand, exPosition);
    }

    public void UpdateLogicalMatrix(Vector2Int exPosition, Vector2Int currentPosition, int value)
    {
        LogicalMatrix[exPosition.x, exPosition.y] = _emptyValue;
        LogicalMatrix[currentPosition.x, currentPosition.y] = value;
    }

    public SandController At(Vector2Int position)
    {
        return SandMatrix.At(position);
    }

    public List<SandController> AtRow(int row)
    {
        return SandMatrix.AtRow(row);
    }

    public List<SandController> AtColumn(int column)
    {
        return SandMatrix.AtColumn(column);
    }

    public int GetOnBoardColumnByPosition(Vector3 position)
    {
        for (int i = 0; i < OnBoardVirtualPositionGrid.GetLength(1); i++)
        {
            float distance = position.x - OnBoardVirtualPositionGrid[0, i].position.x;
            if (distance > 0.05f)
                continue;
            return i;
        }

        return 0;
    }
}