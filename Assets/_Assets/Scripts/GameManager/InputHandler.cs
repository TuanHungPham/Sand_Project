using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Vector3 _mousePos;
    [SerializeField] private GameBoard _gameBoard;
    [SerializeField] private SandSpawner _sandSpawner;
    [SerializeField] private QueueSandBlock _currentSelectedQueueSandBlock;
    [SerializeField] private int targetColumn = -1;
    [SerializeField] private bool _isMouseHolding;

    private GameBoard GameBoard => GameBoard.Instance;

    private float MaxBoardLength
    {
        get
        {
            int maxRow = GameBoard.OnBoardRow - 1;
            int maxColumn = GameBoard.OnBoardColumn - 1;

            return GameBoard.OnBoardVirtualPositionGrid[maxRow, maxColumn].position.y;
        }
    }

    private float MaxBoardWidth
    {
        get
        {
            int maxRow = GameBoard.OnBoardRow - 1;
            int maxColumn = GameBoard.OnBoardColumn - 1;

            return GameBoard.OnBoardVirtualPositionGrid[maxRow, maxColumn].position.x;
        }
    }

    private float MinBoardWidth
    {
        get
        {
            int maxRow = GameBoard.OnBoardRow - 1;
            int maxColumn = 0;

            return GameBoard.OnBoardVirtualPositionGrid[maxRow, maxColumn].position.x;
        }
    }

    private void Update()
    {
        GetMousePos();
        CheckOnBoardPosition();

        if (Input.GetMouseButtonDown(0))
        {
            ClickLeftMouse();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ReleaseLeftMouse();
        }
    }

    public void CheckOnBoardPosition()
    {
        if (!_isMouseHolding) return;

        DragBlock();
        GetOnBoardColumnByMousePos();
    }

    public void GetMousePos()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePos.z = 0;
    }

    public void DragBlock()
    {
        Debug.Log($"(INPUT) HOLD MOUSE BUTTON");

        if (_currentSelectedQueueSandBlock == null) return;
        _currentSelectedQueueSandBlock.BeDragged(_mousePos);
    }

    public void ClickLeftMouse()
    {
        Debug.Log($"(INPUT) CLICK MOUSE BUTTON");

        _isMouseHolding = true;
        GetMousePos();

        foreach (var sandBlock in _sandSpawner.GetQueueSandBlockList())
        {
            if (!sandBlock.CanInteract(_mousePos)) continue;

            _currentSelectedQueueSandBlock = sandBlock;
            Debug.Log($"(INPUT) You have just interacted with {sandBlock.gameObject.name}");
            return;
        }

        Debug.Log("(INPUT) Nothing Selected");
        _currentSelectedQueueSandBlock = null;
    }

    public void ReleaseLeftMouse()
    {
        Debug.Log($"(INPUT) RELEASE MOUSE BUTTON");
        _isMouseHolding = false;

        if (_currentSelectedQueueSandBlock == null) return;
        _currentSelectedQueueSandBlock.ResetBlockPosition();
        _currentSelectedQueueSandBlock = null;
    }

    public void GetOnBoardColumnByMousePos()
    {
        if (_mousePos.y < MaxBoardLength)
        {
            targetColumn = -1;
            return;
        }

        if (_mousePos.x > MaxBoardWidth || _mousePos.x < MinBoardWidth) return;

        targetColumn = _gameBoard.GetOnBoardColumnByPosition(_mousePos);
        Debug.Log($"(INPUT) Column get by mouse: {targetColumn}");
    }
}