using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CCC.Utility;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class Detector : SlowBehaviour
{

    public List<Personnage> allyList = new List<Personnage>();
    public List<Personnage> enemyList = new List<Personnage>();
    List<Personnage> unapprovedUnits = new List<Personnage>();

    public LayerMask terrainMask;

    public Personnage.PersonnageEvent onEnemyEnter = new Personnage.PersonnageEvent();
    public Personnage.PersonnageEvent onEnemyExit = new Personnage.PersonnageEvent();

    public Personnage.PersonnageEvent onAllyEnter = new Personnage.PersonnageEvent();
    public Personnage.PersonnageEvent onAllyExit = new Personnage.PersonnageEvent();

    System.Type[] enemies;
    System.Type[] allies;

    bool hasInit = false;
    bool needAllyVision = false;
    bool needEnemyVision = false;

    void Awake()
    {
        if (!hasInit) gameObject.SetActive(false);
    }

    public void Init(System.Type[] enemy, bool needEnemyVision, System.Type[] ally, bool needAllyVision)
    {
        gameObject.SetActive(true);
        this.enemies = enemy;
        this.allies = ally;
        hasInit = true;
        this.needAllyVision = needAllyVision;
        this.needEnemyVision = needEnemyVision;
    }

    protected override void SlowUpdate()
    {
        base.SlowUpdate();
        UpdateUnitLists();
    }

    public bool CanSee(Transform target)
    {
        return !Physics.Linecast(target.position, transform.position, terrainMask);
    }

    public Personnage GetClosestAlly(System.Type filter = null)
    {
        return GetClosestFrom(allyList, filter);
    }

    public Personnage GetClosestEnemy(System.Type filter = null)
    {
        return GetClosestFrom(enemyList, filter);
    }

    void UpdateUnitLists()
    {
        //Check unapproved
        for (int i=0; i<unapprovedUnits.Count; i++)
        {
            if (i >= unapprovedUnits.Count) break;

            Personnage personnage = unapprovedUnits[i];

            //si est un ennemi
            if (IsIn(personnage.GetType(), enemies))
            {
                if (!needEnemyVision || CanSee(personnage.transform))
                {
                    AddTo(unapprovedUnits, enemyList, personnage);
                    i--;
                    onEnemyEnter.Invoke(personnage);
                }
            }
            //Si est un allié
            else if (IsIn(personnage.GetType(), allies))
            {
                if (!needAllyVision || CanSee(personnage.transform))
                {
                    AddTo(unapprovedUnits, allyList, personnage);
                    i--;
                    onAllyEnter.Invoke(personnage);
                }
            }
            //N'est ni un allié ni un ennemi, on s'en fou de lui...
            else Remove(personnage);
        }

        //Check enemies
        if (needEnemyVision)
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (i >= enemyList.Count) break;

                Personnage enemy = enemyList[i];

                if (!CanSee(enemy.transform))
                {
                    AddTo(enemyList,unapprovedUnits,enemy);
                    onEnemyExit.Invoke(enemy);
                    i--;
                }
            }

        //Check allies
        if (needAllyVision)
            for (int i = 0; i < allyList.Count; i++)
            {
                if (i >= allyList.Count) break;

                Personnage ally = allyList[i];

                if (!CanSee(ally.transform))
                {
                    AddTo(allyList,unapprovedUnits, ally);
                    onAllyExit.Invoke(ally);
                    i--;
                }
            }
    }

    Personnage GetClosestFrom(List<Personnage> liste, System.Type filter)
    {

        Personnage closest = null;
        float smallestDist = Mathf.Infinity;
        foreach (Personnage personnage in liste)
        {
            if (filter != null && personnage.GetType() != filter)
                continue;

            float dist = (personnage.transform.position - transform.position).magnitude;
            if (dist < smallestDist)
            {
                closest = personnage;
                smallestDist = dist;
            }
        }
        return closest;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform == transform.parent) return;

        Personnage personnage = col.GetComponent<Personnage>();
        if (personnage == null) return;

        AddTo(unapprovedUnits, personnage);
        UpdateUnitLists();
    }

    void OnTriggerExit(Collider col)
    {
        Personnage personnage = col.GetComponent<Personnage>();
        if (personnage == null) return;

        Remove(personnage);
    }

    bool IsIn(System.Type type, System.Type[] array)
    {
        if (array == null) return false;

        for (int i = 0; i < array.Length; i++)
        {
            if (type == array[i]) return true;
        }
        return false;
    }

    void AddTo(List<Personnage> from, List<Personnage> list, Personnage personnage)
    {
        from.Remove(personnage);
        list.Add(personnage);
    }

    void AddTo(List<Personnage> list, Personnage personnage)
    {
        list.Add(personnage);
        personnage.onDeath.AddListener(Remove);
    }

    void Remove(Personnage personnage)
    {
        personnage.onDeath.RemoveListener(Remove);
        if (allyList.Remove(personnage)) return;
        if (enemyList.Remove(personnage)) return;
        unapprovedUnits.Remove(personnage);
    }

}