using UnityEngine;

[DisallowMultipleComponent]
public abstract class BaseKillableUnit : MonoBehaviour
{
    public delegate void KillableEvent(BaseKillableUnit killable);

    public event KillableEvent OnNextDeath;
    public event KillableEvent OnAllDeaths;
    public Locker CanBeKilled = new Locker();

    protected bool isDead = false;
    public bool IsDead { get { return isDead; } }

    public void Kill()
    {
        if (isDead || !CanBeKilled)
            return;

        isDead = true;

        OnDeath();

        if (OnNextDeath != null)
            OnNextDeath(this);
        OnNextDeath = null;

        if (OnAllDeaths != null)
            OnAllDeaths(this);

        Capturable cap;
        if (cap = GetComponent<Capturable>())
            cap.ClearLifeEvents();
    }

    public void ClearLifeEvents()
    {
        OnNextDeath = null;
    }

    protected abstract void OnDeath();
}
