using System.Collections.Generic;
using UnityEngine;

public class QueuePointManager : MonoBehaviour
{
    [SerializeField] private List<Transform> _pointList;

    private void Awake()
    {
        GetPoint();
    }

    private void GetPoint()
    {
        foreach (Transform point in transform)
        {
            if (_pointList.Contains(point)) continue;
            _pointList.Add(point);
        }
    }

    public List<Transform> GetPointList()
    {
        return _pointList;
    }
}