using UnityEngine;

public static class Utilities
{
    private static readonly Color[] _colors = { Color.white, Color.red, Color.blue, Color.green, Color.cyan };
    private static readonly string[] _colorStrings = { "white", "red", "blue", "green", "cyan" };

    public static Color ToColor(this int label)
    {
        return _colors[GetColorIndex(label)];
    }

    public static string ToColorString(this int label)
    {
        return _colorStrings[GetColorIndex(label)];
    }

    private static int GetColorIndex(int label)
    {
        return (label - 1) % _colors.Length;
    }
}