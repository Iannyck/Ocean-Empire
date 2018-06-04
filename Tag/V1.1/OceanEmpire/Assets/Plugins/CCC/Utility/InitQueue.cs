using DG.Tweening;
using System;

public class InitQueue
{
    Action onComplete;
    int count = 0;
    private bool endSpecified = false;
    private bool isOver = false;
    public bool IsOver { get { return isOver; } }

    public InitQueue(Action onComplete)
    {
        this.onComplete = onComplete;
    }
    public Action Register()
    {
        count++;
        return OnCompleteAnyInit;
    }
    public TweenCallback RegisterTween()
    {
        count++;
        return OnCompleteAnyInit;
    }
    public void MarkEnd()
    {
        endSpecified = true;
        CheckCompletion();
    }
    void OnCompleteAnyInit()
    {
        count--;
        CheckCompletion();
    }

    void CheckCompletion()
    {
        if (count <= 0 && endSpecified)
            Complete();
    }

    private void Complete()
    {
        isOver = true;
        if (onComplete != null)
        {
            onComplete();
            onComplete = null;
        }
    }
}
