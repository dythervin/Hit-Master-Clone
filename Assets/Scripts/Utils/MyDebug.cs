using UnityEngine;

public static class MyDebug
{
    static bool debug = true;

    public static void Log(object message)
    {
        if (debug)
            Debug.Log(message);
    }
    public static void LogWarning(object message)
    {
        if (debug)
            Debug.LogWarning(message);
    }
    public static void LogError(object message)
    {
        if (debug)
            Debug.LogError(message);
    }

    public static void DrawLine(Vector3 start, Vector3 end)
    {
        if (debug)
            Debug.DrawLine(start, end);
    }
    public static void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        if (debug)
            Debug.DrawLine(start, end, color);
    }
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
    {
        if (debug)
            Debug.DrawLine(start, end, color, duration);
    }
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest)
    {
        if (debug)
            Debug.DrawLine(start, end, color, duration, depthTest);
    }


}
