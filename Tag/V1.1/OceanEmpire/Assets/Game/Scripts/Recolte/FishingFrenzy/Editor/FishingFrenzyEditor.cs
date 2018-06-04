using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FishingFrenzy))]
public class FishingFrenzyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        FishingFrenzy ff = (FishingFrenzy)target;


        GUI.enabled = ff.State == FishingFrenzy.EffectState.Available && Application.isPlaying;
        if (GUILayout.Button("Activate"))
        {
            ff.Activate();
        }

        GUI.enabled = ff.State == FishingFrenzy.EffectState.InCooldown && Application.isPlaying;
        if (GUILayout.Button("Cheat: Skip Cooldown"))
        {
            ff.Cheat_SkipCooldown();
        }
        GUI.enabled = true;
    }

    public override bool RequiresConstantRepaint()
    {
        return true;
    }
}
