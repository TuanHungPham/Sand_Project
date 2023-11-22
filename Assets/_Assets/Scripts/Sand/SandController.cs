using MarchingBytes;
using UnityEngine;

public enum eSandType
{
    WHITE = 1,
    RED,
    BLUE,
    GREEN,
    CYAN,

    MAX_COUNT,
}

public class SandController : MonoBehaviour
{
    [SerializeField] private Vector2Int _position;
    [SerializeField] private bool _shouldLog;

    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private eSandType _sandType;
    [SerializeField] private bool _isQueueSand;
    private int _emptyValue; // The value representing an empty position in the logical matrix

    private float _lastTime;
    private int _objectValue = 1; // The value representing the object in the logical matrix

    private bool HasMoved;

    public int[,] LogicalMatrix => GameBoard.LogicalMatrix;

    private GameBoard GameBoard => GameBoard.Instance;
    private EasyObjectPool EasyObjectPool => EasyObjectPool.instance;

    public Transform[,] OnBoardVirtualPositionGrid => GameBoard.OnBoardVirtualPositionGrid;
    public Transform[,] QueueVirtualPosititonGrid;

    public Vector2Int Position
    {
        get => _position;
        private set => _position = value;
    }

    public void SetData(int currentRow, int currentColumn, int objectValue)
    {
        _position.x = currentRow;
        _position.y = currentColumn;
        _objectValue = objectValue;
        _sandType = (eSandType)objectValue;
        _isQueueSand = false;

        Init();
    }

    public void SetData(int currentRow, int currentColumn, int objectValue, bool isQueueSand, Transform[,] queueGrid)
    {
        _position.x = currentRow;
        _position.y = currentColumn;
        _objectValue = objectValue;
        _sandType = (eSandType)objectValue;
        _isQueueSand = isQueueSand;
        QueueVirtualPosititonGrid = queueGrid;

        Init();
    }

    private void Init()
    {
        _lastTime = Time.time;

        UpdateColor();
        UpdatePosition();
    }

    private void UpdateColor()
    {
        _renderer.color = GetColor();
    }

    private Color GetColor()
    {
        return _objectValue.ToColor();
    }

    public bool At(Vector2Int position)
    {
        return Position.Equals(position);
    }

    private void LogPosition()
    {
        Log($"position: ({Position.x} - {Position.y})");
    }

    private void LogPositionState(int row, int column)
    {
        Log($"position: ({row} - {column}) - {LogicalMatrix[column, column]}");
    }

    // private void Move()
    // {
    //     // Check if there is an empty position below in the logical matrix
    //     var exPosition = Position;
    //     if (Position.x <= 0)
    //     {
    //         Log("cannot move - 0");
    //         return;
    //     }
    //
    //     HasMoved = false;
    //
    //     if (CanMoveDown())
    //         MoveDown();
    //     else
    //         MoveDiagonally();
    //
    //     UpdateBoardMatrix(exPosition);
    // }

    public void CheckMoveDown()
    {
        HasMoved = false;
        // Check if there is an empty position below in the logical matrix
        var exPosition = Position;
        if (Position.x <= 0)
        {
            Log("cannot move - 0");
            return;
        }


        if (CanMoveDown())
        {
            MoveDown();
            UpdateBoardMatrix(exPosition);
        }
    }

    public void CheckMoveDiagonally()
    {
        if (HasMoved)
            return;
        if (Position.x <= 0)
        {
            Log("cannot move - 0");
            return;
        }

        // MoveDiagonally();
        MoveDiagonally2();
    }

    private void MoveDiagonally2()
    {
        var exPosition = Position;
        // Check if there are available positions to the right and left in the logical matrix
        var canMoveRight = CanMoveRight();
        var canMoveLeft = CanMoveLeft();

        // Move right if possible, otherwise move left
        if (canMoveLeft)
        {
            MoveDownLeft();
        }
        else
        {
            if (canMoveRight)
                MoveDownRight();
            else
                Log("cannot move!");
        }

        if (canMoveLeft || canMoveRight)
            UpdateBoardMatrix(exPosition);
    }


    private void MoveDiagonally()
    {
        var exPosition = Position;
        // Check if there are available positions to the right and left in the logical matrix
        var canMoveRight = CanMoveRight();
        var canMoveLeft = CanMoveLeft();

        // Move right if possible, otherwise move left
        if (canMoveRight)
        {
            MoveDownRight();
        }
        else
        {
            if (canMoveLeft)
                MoveDownLeft();
            else
                Log("cannot move!");
        }

        if (canMoveLeft || canMoveRight)
            UpdateBoardMatrix(exPosition);
    }

    private void MoveDownLeft()
    {
        LogPositionState(Position.x - 1, Position.y - 1);
        _position.y--;
        _position.x--;
        Log("move down - left");
        UpdatePosition();
    }

    private void MoveDownRight()
    {
        LogPositionState(Position.x - 1, Position.y + 1);
        _position.y++;
        _position.x--;
        Log("move down - right");
        UpdatePosition();
    }

    private void MoveDown()
    {
        LogPositionState(Position.x - 1, Position.y);
        _position.x--;
        Log("move down");
        UpdatePosition();
    }

    private bool CanMoveLeft()
    {
        return Position.y > 0 &&
               LogicalMatrix[Position.x - 1, Position.y - 1] == _emptyValue;
    }

    private bool CanMoveRight()
    {
        return Position.y < LogicalMatrix.GetLength(1) - 1 &&
               LogicalMatrix[Position.x - 1, Position.y + 1] == _emptyValue;
    }

    private bool CanMoveDown()
    {
        return Position.x > 0 && LogicalMatrix[Position.x - 1, Position.y] == _emptyValue;
    }

    private void Log(string message)
    {
        if (!_shouldLog)
            return;
        // #if DEBUG_LOG
        print(message);
        // #endif
    }

    private void UpdateBoardMatrix(Vector2Int exPosition)
    {
        HasMoved = true;
        GameBoard.UpdateLogicalMatrix(exPosition, Position,
            _objectValue);
        GameBoard.UpdateSandMatrix(this, exPosition);
    }

    private Vector3 GetPosition(int row, int column)
    {
        if (_isQueueSand)
        {
            return QueueVirtualPosititonGrid[row, column].transform.position;
        }

        return OnBoardVirtualPositionGrid[row, column].transform.position;
    }

    private void UpdatePosition()
    {
        var newPosition = GetPosition(Position.x, Position.y);
        UpdatePosition(newPosition);
    }

    private void UpdatePosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public void SetParent(Transform newParent)
    {
        transform.parent = newParent;
    }

    public void DestroySand()
    {
        if (!_isQueueSand)
        {
            GameBoard.UpdateLogicalMatrix(Position, Position, 0);
            GameBoard.UpdateSandMatrix(null, Position);
        }

        EasyObjectPool.ReturnObjectToPool(gameObject);
        // gameObject.SetActive(false);
    }
}