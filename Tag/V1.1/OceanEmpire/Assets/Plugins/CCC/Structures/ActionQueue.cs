using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueue
{
    private struct Order : IComparable
    {
        public int priority;
        public Action action;
        public Order(Action action, int priority)
        {
            this.action = action;
            this.priority = priority;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Order))
                return 0;
            return priority.CompareTo(((Order)obj).priority);
        }
    }
    private List<Order> pendingActions;
    private Action ongoingAction;
    private bool _paused = false;
    public bool Paused
    {
        get { return _paused; }
        set { _paused = value; CheckLaunchNextAction(); }
    }

    public ActionQueue()
    {
        pendingActions = new List<Order>();
    }
    public ActionQueue(int size)
    {
        pendingActions = new List<Order>(size);
    }

    /// <summary>
    /// The action will be inserted AFTER any other action with the same priority.
    /// </summary>
    /// <param name="completionHandle">The completionHandle needs to be called at the completion of the action</param>
    public void AddAction(Action action, int priority, out TweenCallback completionHandle)
    {
        completionHandle = OnActionComplete;
        AddAction(action, priority);
    }
    /// <summary>
    /// The action will be inserted AFTER any other action with the same priority.
    /// </summary>
    /// <param name="completionHandle">The completionHandle needs to be called at the completion of the action</param>
    public void AddAction(Action action, int priority, out Action completionHandle)
    {
        completionHandle = OnActionComplete;
        AddAction(action, priority);
    }

    private void AddAction(Action action, int priority)
    {
        if (action == null)
        {
            Debug.LogError("Action cannot be null");
            return;
        }

        // But: Ajouter la commande dans la liste en gardant l'ordonnacement de priorité descendente
        // NB: Si la priorité est égal, elle sera inséré à la fin par défaut
        Order order = new Order(action, priority);
        bool hasBeenAdded = false;
        for (int i = 0; i < pendingActions.Count; i++)
        {
            if (pendingActions[i].priority < priority)
            {
                pendingActions.Insert(i, order);
                hasBeenAdded = true;
                break;
            }
        }
        if (!hasBeenAdded)
            pendingActions.Add(order);

        CheckLaunchNextAction();
    }

    public object OngoingTarget { get { return ongoingAction != null ? ongoingAction.Target : null; } }

    public int PendingCount { get { return pendingActions.Count; } }

    public bool IsAnActionOngoing
    {
        get { return ongoingAction != null; }
    }

    private void OnActionComplete()
    {
        ongoingAction = null;
        CheckLaunchNextAction();
    }

    bool CheckLaunchNextAction()
    {
        if (pendingActions.Count == 0 || Paused | IsAnActionOngoing)
            return false;

        // Lancement de la prochaine action
        Order order = pendingActions[0];
        pendingActions.RemoveAt(0);
        ongoingAction = order.action;
        ongoingAction();
        return true;
    }
}
