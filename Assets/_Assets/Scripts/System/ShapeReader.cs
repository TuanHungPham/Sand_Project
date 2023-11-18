using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShapeReader : MonoBehaviour
{
    [SerializeField] private TextAsset[] _shapeTextAssets;

    [SerializeField] private bool _shoudRandom;

    [SerializeField] private int defaultShape;

    private List<Shape> Shapes { get; set; }

    private int RandomShapeIndex => (int)(Random.Range(0, Shapes.Count * 1000) % Shapes.Count);

    private void Start()
    {
        TestReadShape();
    }

    public Shape GetShape()
    {
        if (_shoudRandom)
            return Shapes[RandomShapeIndex];
        return Shapes[defaultShape];
    }

    public Shape GetShape(int shape)
    {
        return Shapes[shape];
    }

    private void TestReadShape()
    {
        Random.InitState((int)DateTime.Now.Ticks);
        var csvReader = new CsvReader();
        Shapes = new List<Shape>();
        foreach (var shapeTextAsset in _shapeTextAssets)
        {
            var shapeData = csvReader.ReadCsv(shapeTextAsset.text);
            Shapes.Add(new Shape(shapeData));
        }

        print(GetShape());
    }
}