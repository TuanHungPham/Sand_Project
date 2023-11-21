using UnityEngine;

public abstract class TemporaryMonoSingleton<T> : MonoBehaviour where T : TemporaryMonoSingleton<T>
{
    private static T _instance;

    private static bool _shuttingDown;

    public static T Instance
    {
        get
        {
            if (_instance == null && !_shuttingDown && Application.isPlaying)
            {
                _instance = FindObjectOfType(typeof(T)) as T;
                if (_instance == null)
                {
                    Debug.LogWarning("No instance of " + typeof(T) + ", a temporary one is created.");
                    _instance = new GameObject("Temp Instance of " + typeof(T), typeof(T)).GetComponent<T>();
                }
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            Init();
            return;
        }

        if (_instance != this)
        {
            var arg_51_0 = "Another instance of ";
            var expr_40 = GetType();
            Debug.LogError(arg_51_0 + (expr_40 != null ? expr_40.ToString() : null) +
                           " is already exist! Destroying self...");
            DestroyImmediate(gameObject);
        }
        else
        {
            Init();
        }
    }

    protected virtual void OnDestroy()
    {
        if (this == _instance) _instance = default;
    }

    private void OnApplicationQuit()
    {
        _instance = default;
        _shuttingDown = true;
    }

    /// <summary>
    ///     this is the place for calling method from Awake instead of overriding the Awake method
    /// </summary>
    protected virtual void Init()
    {
    }
}