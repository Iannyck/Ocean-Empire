using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradeDescriptionBuilder<T> : ScriptableObject where T : UpgradeDescription
{
    public int upgradeLevel;
    [SerializeField]
    protected T data;

    public T BuildUpgradeDescription()
    {
        return data;
    }
}
