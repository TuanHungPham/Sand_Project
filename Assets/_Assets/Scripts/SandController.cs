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
    [SerializeField] private float _moveSpeed = 1f;

    [SerializeField] private Vector2Int _position;

    [SerializeField] private float _interval = 5f;

    [SerializeField] private bool _shouldLog;

    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private eSandType _sandType;
    private int _emptyValue; // The value representing an empty position in the logical matrix
    private GameBoard _gameBoard;

    private float _lastTime;
    private int _objectValue = 1; // The value representing the object in the logical matrix

    private bool HasMoved;

    public int[,] LogicalMatrix => GameBoard.LogicalMatrix;

    private GameBoard GameBoard
    {
        get
        {
            if (!_gameBoard) _gameBoard = FindObjectOfType<GameBoard>();
            return _gameBoard;
        }
    }

    public Transform[,] VirtualPositionGrid => GameBoard.VirtualPositionGrid;

    public Vector2Int Position
    {
        get => _position;
        private set => _position = value;
    }


    private void Start()
    {
        Init();
    }

    public void SetData(int currentRow, int currentColumn, int objectValue)
    {
        _position.x = currentRow;
        _position.y = currentColumn;
        _objectValue = objectValue;
    }

    private void Init()
    {
        _lastTime = Time.time;

        // _position.x = VirtualPositionGrid.GetLength(0) - 1;
        // _position.y = Random.Range(0, VirtualPositionGrid.GetLength(1));
        // _position.y =0;

        UpdateColor();
        UpdatePosition();
    }

    private void UpdateColor()
    {
        _objectValue = (int)_sandType;
        _renderer.color = GetColor();
    }

    private Color GetColor()
    {
        return _objectValue.ToColor();
    }


    public void UpdateCallback()
    {
        if (!HasPastInterval()) return;
        Move();
    }

    private bool HasPastInterval()
    {
        return true;
        if (Time.time > _interval + _lastTime)
        {
            _lastTime += _interval;
            LogPosition();
            Log("pass interval");
            return true;
        }

        return false;
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

    private void Move()
    {
        // Check if there is an empty position below in the logical matrix
        var exPosition = Position;
        if (Position.x <= 0)
        {
            Log("cannot move - 0");
            return;
        }

        HasMoved = false;

        if (CanMoveDown())
            MoveDown();
        else
            MoveDiagonally();

        UpdateBoardMatrix(exPosition);
    }

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
        return VirtualPositionGrid[row, column].transform.position;
    }

    private void UpdatePosition()
    {
        var newPosition = GetPosition(Position.x, Position.y);
        UpdatePosition(newPosition);
    }

    private void UpdatePosition(Vector3 newPosition)
    {
        transform.position = newPosition;
        // transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        // transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        // transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    public void SetParent(Transform newParent)
    {
        transform.parent = newParent;
    }
}