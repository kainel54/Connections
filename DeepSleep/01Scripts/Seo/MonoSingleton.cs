using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    private static bool IsDestroyed = false;

    public static T Instance
    {
        get
        {
            if (IsDestroyed)
            {
                _instance = null;
            }
            if (_instance == null)
            {
                _instance = GameObject.FindAnyObjectByType<T>();
                if (_instance == null)
                {
                    Debug.Assert(_instance != null, $"{typeof(T).Name} singletone is not exist");
                }
                else
                {
                    IsDestroyed = false;
                }
            }
            return _instance;
        }
    }

    private void OnDestroy()
    {
        IsDestroyed = true;
    }
}
