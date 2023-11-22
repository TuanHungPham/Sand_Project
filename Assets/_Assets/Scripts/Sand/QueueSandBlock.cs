using UnityEngine;

public class QueueSandBlock : MonoBehaviour
{
    private Shape _shape;
    [SerializeField] private float _interactingRadius;
    [SerializeField] private Vector3 _initialPosition;
    [SerializeField] private QueueBlockSpawner _queueBlockSpawner;
    [SerializeField] private bool _isEmpty;

    private void Awake()
    {
        LoadComponents();
    }

    private void LoadComponents()
    {
        _queueBlockSpawner = GetComponentInChildren<QueueBlockSpawner>();
    }

    private void Start()
    {
        Initialize();
        SpawnQueueSandBlock();
    }

    private void Initialize()
    {
        _initialPosition = transform.position;
    }

    public void DropBlockToBoard()
    {
        _queueBlockSpawner.DestroyQueueSand();
        _shape = null;
        _isEmpty = true;
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

    public void BeDragged(Vector3 position)
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
}