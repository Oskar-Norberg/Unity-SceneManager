using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogWarning("Instance of " + typeof(T).Name + " is null");
            }
            
            return _instance;
        }
    }

    protected static T _instance;

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this as T;
    }

    protected void OnApplicationQuit()
    {
        _instance = null;
        Destroy(gameObject);
    }
}

public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
