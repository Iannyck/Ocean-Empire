using System.Collections.Generic;

/// <summary>
/// Semblable au type 'bool'. L'élément vaut 'true' par défaut. S'il y a une ou plusieurs clé d'inséré, il vaut 'false'.
/// </summary>
public class Locker
{
    public delegate void LockerEvent(bool state);
    public event LockerEvent onLockStateChange;
    private List<string> keys = new List<string>();
    public Locker() { }

    /// <summary>
    /// Enlève la première instance d'une clé de ce nom
    /// </summary>
    public void Unlock(string key)
    {
        bool wasUnlocked = IsUnlocked();

        keys.Remove(key);

        if (IsUnlocked() && !wasUnlocked && onLockStateChange != null)
            onLockStateChange(true);
    }


    /// <summary>
    /// Vérifie si la clé existe
    /// </summary>
    public bool HasKey(string key)
    {
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i] == key)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Enlève tous les instances de clé de ce nom
    /// </summary>
    public void UnlockAll(string key)
    {
        bool wasUnlocked = IsUnlocked();

        keys.RemoveAll(x => x.Equals(key));

        if (IsUnlocked() && !wasUnlocked && onLockStateChange != null)
            onLockStateChange(true);
    }

    /// <summary>
    /// Ajoute une clé de ce nom
    /// </summary>
    public void Lock(string key)
    {
        bool wasUnlocked = IsUnlocked();

        keys.Add(key);

        if (!IsUnlocked() && wasUnlocked && onLockStateChange != null)
            onLockStateChange(false);
    }

    /// <summary>
    /// Ajoute une clé de ce nom, si elle ne s'y trouve pas déjà
    /// </summary>
    public void LockUnique(string key)
    {
        bool wasUnlocked = IsUnlocked();

        if (!keys.Contains(key))
        {
            keys.Add(key);

            if (!IsUnlocked() && wasUnlocked && onLockStateChange != null)
                onLockStateChange(false);
        }
    }

    /// <summary>
    /// Tous les clé ont-elles été enlevées ? Faux si une ou plusieur clé sont encore présente
    /// </summary>
    public bool IsUnlocked()
    {
        return keys.Count == 0;
    }

    public override string ToString()
    {
        return this;
    }

    public static implicit operator bool(Locker locker)
    {
        return locker.IsUnlocked();
    }
    public static implicit operator string(Locker locker)
    {
        return locker.IsUnlocked().ToString();
    }
}