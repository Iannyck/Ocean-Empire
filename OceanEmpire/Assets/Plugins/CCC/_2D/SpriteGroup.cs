using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CCC._2D
{
    public class SpriteGroup : MonoBehaviour
    {
        public SpriteRenderer[] childRenderers;
        public float[] defaultAlphas;
        public float Alpha
        {
            get { return alpha; }
            set
            {
                alpha = value;
                ApplyAlphaToChildren();
            }
        }

        [SerializeField, Range(0,1)]
        private float alpha = 1;

        public void GatherChildData()
        {
            childRenderers = GetComponentsInChildren<SpriteRenderer>();
            defaultAlphas = new float[childRenderers.Length];

            if (alpha > 0)
            {
                for (int i = 0; i < childRenderers.Length; i++)
                {
                    defaultAlphas[i] = (childRenderers[i].color.a / alpha).Capped(1);
                }
                ApplyAlphaToChildren();
            }
            else
                Debug.LogWarning("Cannot gather child data if alpha = 0. It must be > 0");

        }

        public void ApplyAlphaToChildren()
        {
            if (childRenderers != null)
                for (int i = 0; i < childRenderers.Length; i++)
                {
                    childRenderers[i].SetAlpha((defaultAlphas[i] * alpha).Clamped(0, 1));
                }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SpriteGroup))]
    public class SpriteGroupEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            base.OnInspectorGUI();

            if (EditorGUI.EndChangeCheck())
                (target as SpriteGroup).ApplyAlphaToChildren();


            if (GUILayout.Button("Gather Child Data"))
            {
                (target as SpriteGroup).GatherChildData();
            }
        }
    }
#endif

    public static class SpriteGroupExtension
    {
        public static Tweener DOFade(this SpriteGroup value, float to, float duration)
        {
            return DOTween.To(() => value.Alpha, (x) => value.Alpha = x, to, duration);
        }
    }
}
