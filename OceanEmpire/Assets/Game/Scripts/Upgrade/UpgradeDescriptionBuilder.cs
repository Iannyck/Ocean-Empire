using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class UpgradeDescriptionBuilder<T> : ScriptableObject where T : UpgradeDescription
{
    public int upgradeLevel;

    [SerializeField]
    protected T data;

    [SerializeField]
    protected PrebuiltSpriteKit prebuiltSpriteKit;

    public T BuildUpgradeDescription()
    {
        data.spriteKit = prebuiltSpriteKit;
        return data;
    }
}
