using CCC.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ContinuousRewardWindow : MonoBehaviour
{
    [SerializeField] public Text ticketText;

    TweenCallback onComplete;

    public void FillContent(int ticketAmount, TweenCallback onComplete)
    {
        ticketText.text = "+ " + ticketAmount;
        this.onComplete = onComplete;
    }

    public void OkClick()
    {
        GetComponent<WindowAnimation>().Close(onComplete);
    }
}
