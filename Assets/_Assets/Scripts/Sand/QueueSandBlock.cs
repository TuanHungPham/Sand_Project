using System.Collections.Generic;
using UnityEngine;

public class QueueSandBlock : MonoBehaviour
{
    private Shape _shape;
    [SerializeField] private Vector3 _initialPosition;
    [SerializeField] private float _interactingRadius;
    [SerializeField] private List<SandController> _sandList = new List<SandController>();

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _initialPosition = transform.position;
    }

    public void AddSandToQueueBlock(SandController sandController)
    {
        _sandList.Add(sandController);
    }

    public void ReleaseBlockToBoard()
    {
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, new Vector3(_interactingRadius, _interactingRadius, _interactingRadius));
        Gizmos.color = Color.white;
    }

    public void SetShape(Shape shape)
    {
        _shape = shape;
    }

    public Shape GetShape()
    {
        return _shape;
    }
}