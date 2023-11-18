using System.Collections.Generic;
using UnityEngine;

public class QueueSandBlock : MonoBehaviour
{
    private Shape _shape;
    [SerializeField] private float _interactingRadius;
    [SerializeField] private List<SandController> _sandList = new List<SandController>();

    public void Initialize(Shape shape)
    {
        _shape = shape;
    }

    public void AddSandToQueueBlock(SandController sandController)
    {
        _sandList.Add(sandController);
    }
}