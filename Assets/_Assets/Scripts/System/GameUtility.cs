using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class GameUtility
{
    private const float APPROXIMATE_ZERO = 0.01f;

    private static readonly Color DefaultColor = new(0, 0, 1, 1);


    public static void Log<T>(this IEnumerable<T> caller, Color color = default)
    {
        var messageBuilder = new StringBuilder();
        foreach (var item in caller) messageBuilder.AppendLine($"item: {item}");
        Debug.Log($"List: {caller}:\n{GetColoredMessage(messageBuilder.ToString(), color)}");
    }

    public static void Log<T>(this object caller, IEnumerable<T> collection, Color color = default)
    {
        var messageBuilder = new StringBuilder();
        foreach (var item in collection) messageBuilder.AppendLine($"item: {item}");
        Debug.Log($"List: {caller}:\n{GetColoredMessage(messageBuilder.ToString(), color)}");
    }

    public static void Log<T>(this object caller, string message, IEnumerable<T> collection, Color color = default)
    {
        var messageBuilder = new StringBuilder();
        messageBuilder.AppendLine(message);
        foreach (var item in collection) messageBuilder.AppendLine($"item: {item}");
        Debug.Log($"List: {caller}:\n{GetColoredMessage(messageBuilder.ToString(), color)}");
    }

    public static void Log(this object caller, string message, Color color = default)
    {
        message = GetColoredMessage(message, color);
        Debug.Log($"[{caller}]: {message}");
    }

    public static string GetColoredMessage(List<string> messages)
    {
        var messageBuilder = new StringBuilder();
        foreach (var message in messages) messageBuilder.AppendLine(message);

        return messageBuilder.ToString();
    }

    public static void Log(this MonoBehaviour caller, string message, Color color = default)
    {
        message = GetColoredMessage(message, color);
        Debug.Log($"[{caller?.name}]: {message}");
    }

    private static string GetColoredMessage(string message, Color color = default)
    {
        if (color == default)
            color = DefaultColor;
        return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{message}</color>";
    }

    public static void LogError(this object caller, string message)
    {
        Debug.LogError($"[{caller}]: {GetColoredMessage(message)}");
    }

    public static void LogError(this MonoBehaviour caller, string message)
    {
        Debug.LogError($"[{caller.name}]: {GetColoredMessage(message)}");
    }

    public static bool IsMoving(this Rigidbody2D rig)
    {
        return !rig.IsStopping();
    }

    public static bool IsStopping(this Rigidbody2D rig)
    {
        // Debug.Log($"rig.velocity.magnitude: {rig.velocity.magnitude} ------ {APPROXIMATE_ZERO}");
        return rig.velocity.magnitude <= APPROXIMATE_ZERO;
    }

    public static void DrawGizmosSphere(this object caller, List<Vector3> points,
        Color color = default, float radius = 1f)
    {
        if (points == null)
            return;
        foreach (var point in points) DrawGizmosSphere(caller, point, color, radius);
    }

    public static void DrawGizmosLine(this object caller, Vector3 start, Vector3 end,
        Color color = default, float radius = 1f)
    {
        if (color == default) color = DefaultColor;
        Gizmos.color = color;
        Gizmos.DrawLine(start, end);
    }

    public static void DrawGizmosSphere(this object caller, Vector3 point,
        Color color = default, float radius = 1f)
    {
        if (color == default) color = DefaultColor;
        Gizmos.color = color;
        Gizmos.DrawSphere(point, radius);
    }

    public static void DrawGizmosCircle(this object caller, Vector3 centerPoint,
        float radius, Color color = default)
    {
        if (color == default) color = DefaultColor;
        Gizmos.color = color;
        Gizmos.DrawWireSphere(centerPoint, radius);
    }

    public static bool RandomBool(this object caller, float rate)
    {
        var chance = Random.value;
        return chance <= rate;
    }

    public static bool EqualToZero(this float value)
    {
        return Mathf.Abs(value) <= APPROXIMATE_ZERO;
    }

    public static bool IsTheSamePoint(this Vector3 caller, Vector3 other)
    {
        return Vector3.Distance(caller, other).EqualToZero();
    }

    public static void Enqueue<T>(this Queue<T> queue, Queue<T> otherQueue)
    {
        if (queue == null)
            return;
        if (otherQueue == null)
            return;
        while (otherQueue.Count > 0) queue.Enqueue(otherQueue.Dequeue());
    }

    public static bool RandomBool(this object caller)
    {
        var ran = Random.Range(0, 1000000) % 2;
        return ran == 1;
    }

    public static void DrawRectangle(this object caller, Vector3 topLeft, Vector3 bottomRight, Color color = default)
    {
        var topRight = topLeft;
        topRight.x = bottomRight.x;
        var bottomLeft = bottomRight;
        bottomLeft.x = topLeft.x;

        DrawGizmosLine(caller, topLeft, topRight, color);
        DrawGizmosLine(caller, bottomLeft, bottomRight, color);
        DrawGizmosLine(caller, topLeft, bottomLeft, color);
        DrawGizmosLine(caller, topRight, bottomRight, color);

        DrawGizmosSphere(caller, topLeft, color);
        DrawGizmosSphere(caller, topRight, color);
        DrawGizmosSphere(caller, bottomLeft, color);
        DrawGizmosSphere(caller, bottomRight, color);
    }

    public static int RandomRange(this object caller, int minInclusive, int maxExclusive)
    {
        return Random.Range(minInclusive, maxExclusive);
    }

    public static float RandomRange(this object caller, float minInclusive, float maxExclusive)
    {
        return Random.Range(minInclusive, maxExclusive);
    }

    public static T RandomRange<T>(this object caller, IEnumerable<T> collection)
    {
        var count = collection.Count();
        var ran = RandomRange(caller, 0, count * 1000) % count;
        return collection.ElementAt(ran);
    }
}