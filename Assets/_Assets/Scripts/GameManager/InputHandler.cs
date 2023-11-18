using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Vector3 _mousePos;
    [SerializeField] private GameBoard _gameBoard;
    [SerializeField] private SandSpawner _sandSpawner;
    [SerializeField] private QueueSandBlock _currentSelectedQueueSandBlock;
    [SerializeField] private bool _isMouseHolding;

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

    private void CheckOnBoardPosition()
    {
        if (!_isMouseHolding) return;

        DragBlock(_mousePos);
        GetOnBoardColumnByMousePos();
    }

    private void GetMousePos()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePos.z = 0;
    }

    private void DragBlock(Vector3 pos)
    {
        if (_currentSelectedQueueSandBlock == null) return;
        _currentSelectedQueueSandBlock.BeDragged(pos);
    }

    private void ClickLeftMouse()
    {
        _isMouseHolding = true;

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

    private void ReleaseLeftMouse()
    {
        _isMouseHolding = false;

        if (_currentSelectedQueueSandBlock == null) return;
        _currentSelectedQueueSandBlock.ResetBlockPosition();
        _currentSelectedQueueSandBlock = null;
    }

    private void GetOnBoardColumnByMousePos()
    {
        Debug.Log($"(INPUT) Column get by mouse: {_gameBoard.GetOnBoardColumnByPosition(_mousePos)}");
    }
}