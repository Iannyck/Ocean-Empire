using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct ResourceSprite
{
    [SerializeField]
    private string spriteName;

    [SerializeField, HideInInspector]
    private string prefix;

    [NonSerialized]
    private Sprite cachedSprite;

    public ResourceSprite(string prefix)
    {
        this.prefix = prefix;
        cachedSprite = null;
        spriteName = "";
    }

    public Sprite GetSprite()
    {
        if (cachedSprite != null)
            return cachedSprite;

        UnityEngine.Object obj = Resources.Load(prefix + spriteName, typeof(Sprite));
        if (obj != null)
        {
            cachedSprite = obj as Sprite;
        }
        return cachedSprite;
    }
}
