using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SoundPlayer_CommonMsg : SoundPlayer
{
    [Header("Unity Messages")]
    public bool onStart = false;
    public bool onEnable = false;
    public bool onDisable = false;

    void Start()
    {
        if (onStart)
            Play();
    }

    void OnEnable()
    {
        if (onEnable)
            Play();
    }
    void OnDisable()
    {
#if UNITY_EDITOR
        // Cette partie de code est présente pour s'assurer de ne pas faire jouer de son lorsque qu'on arrête le 'PlayMode'
        if (!EditorApplication.isPlayingOrWillChangePlaymode)
            return;
#endif
        if (onDisable)
            Play();
    }
}
