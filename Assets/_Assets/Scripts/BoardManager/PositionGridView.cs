using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PositionGridView : MonoBehaviour
{
    // public Transform[,] _positionGrid;
    private List<PositionRowView> _positionGrid;
    public List<PositionRow> positionGrid;
#if UNITY_EDITOR
    private void Reset()
    {
        var positionRows = GetComponentsInChildren<PositionRowView>();
        // _positionGrid = new Transform[debugPositions.GetLength(0), debugPositions.GetLength(1)];
        // for (int i = 0; i < debugPositions.Length; i++)
        // {
        //     for (int j = 0; j < debugPositions[i].positions.Length; j++)
        //     {
        //         _positionGrid[i,j] = debugPositions[i].positions[j];
        //     }
        //
        //     debugPositions[i].transform.localPosition = Vector3.zero + Vector3.up * i;
        // }
        _positionGrid = new List<PositionRowView>(positionRows);
        positionGrid = new List<PositionRow>();
        for (int i= 0; i<positionRows.Length;i++)
        {
            positionGrid.Add(positionRows[i].row);
            positionRows[i].transform.localPosition = Vector3.down*i;
        }
    }

    private void OnValidate()
    {
        Reset();
    }

#endif
}