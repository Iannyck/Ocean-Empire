using UnityEngine;

[CreateAssetMenu(menuName = "CCC/Structures/Scriptable Action Queue")]
public class ScriptableActionQueue : ScriptableObject, ISerializationCallbackReceiver
{
    public ActionQueue ActionQueue { get; private set; }

    public void OnAfterDeserialize()
    {
        ActionQueue = new ActionQueue();
    }

    public void OnBeforeSerialize() { }
}
