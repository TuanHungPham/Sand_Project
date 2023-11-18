using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPanel : MonoBehaviour
{
    private SandSpawner _sandSpawner;

    private SandSpawner SandSpawner
    {
        get
        {
            if (!_sandSpawner) _sandSpawner = FindObjectOfType<SandSpawner>();
            return _sandSpawner;
        }
        set => _sandSpawner = value;
    }

    public void SpawnOneLine()
    {
        SandSpawner.CreateOnBoardVirtualShape(10);
        SandSpawner.CreateOnBoardVirtualShape(35);
        SandSpawner.CreateOnBoardVirtualShape(60);
    }
}