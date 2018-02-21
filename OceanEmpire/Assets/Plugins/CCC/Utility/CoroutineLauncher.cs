using UnityEngine;

public class CoroutineLauncher : MonoBehaviour
{
    private static  CoroutineLauncher instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public static CoroutineLauncher Instance
    {
        get
        {
            if(instance == null)
            {
                var obj = new GameObject("Couroutine Launcher");
                DontDestroyOnLoad(obj);
                obj.AddComponent<CoroutineLauncher>();
            }
            return instance;
        }
    }
}
