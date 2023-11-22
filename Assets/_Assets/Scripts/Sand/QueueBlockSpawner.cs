using UnityEngine;

public class QueueBlockSpawner : MonoBehaviour
{
    [SerializeField] private QueueSandBlock _queueSandBlock;
    [SerializeField] private SandController _sandPfb;
    [SerializeField] private Transform _sandRoot;
    [SerializeField] private int _objectValue = 1;
    [SerializeField] private PositionGridView _positionGridView;
    private Transform[,] queueVirtualPositionGrid;
    private int _queueRow;
    private int _queueColumn;
    private ShapeReader _shapeReader;

    private void Awake()
    {
        LoadComponents();
        InitQueueVirtualMatrix();
    }

    private void LoadComponents()
    {
        _queueSandBlock = GetComponentInParent<QueueSandBlock>();
    }

    private void Start()
    {
        CreateQueueVirtualShape(1);
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


    public void CreateQueueVirtualShape(int startColumn)
    {
        var shape = _queueSandBlock.GetShape();

        for (var j = 0; j < shape.Column; j++)
        for (var i = shape.Row - 1; i >= 0; i--)
        {
            if (shape.Matrix[i, j] == 0)
                continue;
            var sand = CreateNewSand(_queueRow - i - 1, j + startColumn);
        }
    }

    private SandController CreateNewSand(int row, int column)
    {
        var sand = Instantiate(_sandPfb, _sandRoot);
        sand.gameObject.SetActive(true);

        sand.SetData(row, column, _objectValue, true, queueVirtualPositionGrid);

        return sand;
    }
}