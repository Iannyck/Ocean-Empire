public class CoroutineLauncher : SelfSpawningSingleton<CoroutineLauncher>
{
    protected override string GameObjectName()
    {
        return "Couroutine Launcher";
    }

    public static CoroutineLauncher Instance
    {
        get
        {
            return _Instance;
        }
    }
}
