using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace CCC.EditorUtil
{
    public class AdvEditor : Editor
    {
        protected GUIStyle bold;
        protected GUIStyle centered;
        protected GUIStyle righted;


        protected virtual void Awake()
        {
            bold = new GUIStyle();
            bold.fontStyle = FontStyle.Bold;

            centered = new GUIStyle();
            centered.alignment = TextAnchor.MiddleCenter;

            righted = new GUIStyle();
            righted.alignment = TextAnchor.MiddleRight;
        }
    }
}
#endif