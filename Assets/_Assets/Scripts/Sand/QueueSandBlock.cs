using System;
using MarchingBytes;
using UnityEngine;

public class QueueSandBlock : MonoBehaviour
{
    private Shape _shape;
    [SerializeField] private float _interactingRadius;
    [SerializeField] private Vector3 _initialPosition;
    [SerializeField] private QueueBlockSpawner _queueBlockSpawner;
    [SerializeField] private bool _isEmpty;

    private GameBoard GameBoard => GameBoard.Instance;
    private EasyObjectPool EasyObjectPool => EasyObjectPool.instance;

    private void Awake()
    {
        LoadComponents();
    }

    private void LoadComponents()
    {
        _queueBlockSpawner = GetComponentInChildren<QueueBlockSpawner>();
    }

    public void Init(Vector3 position, int objectValue)
    {
        _initialPosition = position;
        SetObjectValue(objectValue);
    }

    public void SetObjectValue(int objectValue)
    {
        _queueBlockSpawner.SetObjectValue(objectValue);
    }

    public void DropBlockToBoard()
    {
        _queueBlockSpawner.DestroyQueueSand();
        _shape = null;
        _isEmpty = true;
        EasyObjectPool.ReturnObjectToPool(gameObject);
    }

    public void ResetBlockPosition()
    {
        transform.position = _initialPosition;
    }

    public bool CanInteract(Vector3 position)
    {
        float distance = Vector3.Distance(transform.position, position);
        if (distance > _interactingRadius) return false;

        return true;
    }

    public void BeDragged(Vector3 position, int column)
    {
        if (column == -1)
        {
            FollowMousePos(position);
            return;
        }

        FollowColumnPos(column);
    }

    private void FollowColumnPos(int column)
    {
        var maxRow = GameBoard.OnBoardRow;
        var shape_middlePoint = _shape.GetMiddlePointOfShape();

        Transform pos = GameBoard.OnBoardVirtualPositionGrid[maxRow - shape_middlePoint.x - 1, column];

        transform.position = pos.position;
    }

    private void FollowMousePos(Vector3 position)
    {
        transform.position = position;
    }

    public void SpawnQueueSandBlock()
    {
        _queueBlockSpawner.CreateQueueVirtualShape();
        _isEmpty = false;
    }

    public void SetShape(Shape shape)
    {
        _shape = shape;
        SpawnQueueSandBlock();
    }

    public Shape GetShape()
    {
        return _shape;
    }

    public bool IsEmpty()
    {
        return _isEmpty;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, new Vector3(_interactingRadius, _interactingRadius, _interactingRadius));
        Gizmos.color = Color.white;
    }

    public int GetObjectValue()
    {
        return _queueBlockSpawner.GetObjectValue();
    }
}