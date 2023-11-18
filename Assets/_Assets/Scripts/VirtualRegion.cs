using System;
using UnityEngine;

[Serializable]
public class VirtualRegion
{
    public Region Region;
    public Transform Transform;

    public VirtualRegion(Region region, Transform parent)
    {
        Region = region;
        var go = new GameObject(Region.Name);
        Transform = go.transform;
        Transform.parent = parent;
    }

    public void Collect()
    {
        Transform.name += "   -----   Collected!!!";
        Debug.Log("Region Collected!");
    }
}