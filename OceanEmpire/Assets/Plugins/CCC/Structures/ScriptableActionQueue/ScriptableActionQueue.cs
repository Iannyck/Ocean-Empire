using UnityEngine;

[CreateAssetMenu(menuName = "CCC/Structures/Scriptable Action Queue")]
public class ScriptableActionQueue : ScriptableObject, ISerializationCallbackReceiver
{
    public ActionQueue ActionQueue { get; private set; }


    public void OnAfterDeserialize()
    {
        Debug.Log("Lucas : Test Action queue");
        ActionQueue = new ActionQueue();
    }

    public void OnBeforeSerialize() { }
}
