using System;
using UnityEngine;

public class PositionRowView:MonoBehaviour
{
    public PositionRow row;
#if UNITY_EDITOR
    private void Reset()
    {
        var debugPositions = GetComponentsInChildren<DebugPosition>();
        row.positions = new Transform[debugPositions.Length];
        for (int i = 0; i < debugPositions.Length; i++)
        {
            row.positions[i] = debugPositions[i].transform;
            row.positions[i].localPosition = Vector3.zero + Vector3.right * i;
        }
    }

    private void OnValidate()
    {
        Reset();
    }

#endif
}

[Serializable]
public class PositionRow
{
    public Transform[] positions;
}