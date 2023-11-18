using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Vector3 _mousePos;
    [SerializeField] private GameBoard _gameBoard;

    private void Update()
    {
        CheckPosition();
    }

    private void CheckPosition()
    {
        if (!Input.GetMouseButton(0)) return;

        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePos.z = 0;
        GetOnBoardColumnByMousePos();
    }

    private void GetOnBoardColumnByMousePos()
    {
        Debug.Log($"(INPUT) Column get by mouse: {_gameBoard.GetOnBoardColumnByPosition(_mousePos)}");
    }
}