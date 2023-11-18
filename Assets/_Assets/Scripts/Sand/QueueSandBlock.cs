using System;
using System.Collections.Generic;
using UnityEngine;

public class QueueSandBlock : MonoBehaviour
{
    private Shape _shape;
    [SerializeField] private float _interactingRadius;
    [SerializeField] private List<SandController> _sandList = new List<SandController>();

    public void AddSandToQueueBlock(SandController sandController)
    {
        _sandList.Add(sandController);
    }

    public void ReleaseBlockToBoard()
    {
    }

    public void ResetBlockPosition()
    {
        transform.position = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, new Vector3(_interactingRadius, _interactingRadius, _interactingRadius));
        Gizmos.color = Color.white;
    }
}