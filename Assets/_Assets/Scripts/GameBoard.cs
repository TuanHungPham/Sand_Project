using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public PositionGridView positionGridView;

    public int Left;
    [SerializeField] private bool _isUpdateMatrixFromHierarchy = true;

    public List<LogicalRow> matrix;
    public int Right;

    [SerializeField] private SandMatrix _sandMatrix;

    private readonly int _emptyValue = 0; // The value representing an empty position in the logical matrix

    [SerializeField] private int[,] _logicalMatrix; // Logical matrix representing different kinds of objects

    private int emptyValue = 0; // The value representing an empty position in the logical matrix

    public int[,] LogicalMatrix
    {
        get => _logicalMatrix;
        private set => _logicalMatrix = value;
    }

    public Transform[,] VirtualPositionGrid { get; set; }

    public int Row { get; private set; }

    public int Column { get; private set; }

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
        InitVirtualMatrix();
        InitSandMatrix();
        InitLogicalMatrix();
        if (Right <= 0) Right = Column - 1;
        if (Left < 0)
            Left = 0;
    }

    private void InitVirtualMatrix()
    {
        var positionGrid = positionGridView.positionGrid;
        Row = positionGrid.Count;
        Column = positionGrid[0].positions.Length;

        VirtualPositionGrid = new Transform[Row, Column];

        for (var row = 0; row < Row; row++)
        for (var column = 0; column < Column; column++)
            VirtualPositionGrid[row, column] = positionGrid[row].positions[column];
    }

    private void InitSandMatrix()
    {
        SandMatrix = new SandMatrix(Row, Column);
    }

    private void InitLogicalMatrix()
    {
        // Initialize the logical matrix with empty positions
        LogicalMatrix = new int[Row, Column];
        for (var row = 0; row < Row; row++)
        for (var column = 0; column < Column; column++)
            LogicalMatrix[row, column] = matrix[row].points[column];

        LogLogicalMatrix();
    }

    private void LogLogicalMatrix()
    {
        var log = new StringBuilder();
        for (var row = 0; row < Row; row++)
        {
            log.Append("(");
            for (var column = 0; column < Column; column++) log.Append($"{LogicalMatrix[row, column]}, ");

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

    public List<SandController> AtRow(int row)
    {
        return SandMatrix.AtRow(row);
    }

    public List<SandController> AtColumn(int column)
    {
        return SandMatrix.AtColumn(column);
    }
}