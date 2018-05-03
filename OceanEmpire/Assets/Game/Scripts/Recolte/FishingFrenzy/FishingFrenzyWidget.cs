using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FishingFrenzyWidget : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Text txt_available;
    [SerializeField] Text txt_error;
    [SerializeField] Text txt_inCooldown;
    [SerializeField] Text txt_active;
    [SerializeField] Text txt_title;
    [SerializeField] Image img_shine;
    [SerializeField] Image img_bg;

    [Header("Settings")]
    public bool autoUpdateState = true;
    [SerializeField, Suffix("frame")] int updateEvery;
    [SerializeField] Color col_activeShine;
    [SerializeField] Color col_availableShine;
    [SerializeField] Color col_inCooldownText;
    [SerializeField] Color col_inCooldownBG;

    public UnityEvent OnStateUpdated = new UnityEvent();

    Color col_notInCooldownText;
    Color col_notInCooldownBG;
    int frameCounter = 0;

    void Awake()
    {
        col_notInCooldownText = txt_title.color;
        col_notInCooldownBG = img_bg.color;
    }

    void Start()
    {
        // On se désactive si on a pas unlock le fishing frenzy dans le shop
        PersistentLoader.LoadIfNotLoaded(UpdateVisibility);

        if (autoUpdateState)
            AutoUpdateState();
    }

    public void UpdateVisibility()
    {
        if (FishingFrenzy.Instance)
            gameObject.SetActive(FishingFrenzy.Instance.shopCategory.IsAvailable);
    }

    void Update()
    {
        if (autoUpdateState)
        {
            frameCounter++;
            if (frameCounter == updateEvery)
            {
                AutoUpdateState();
                frameCounter = 0;
            }
        }
    }

    public void AutoUpdateState()
    {
        if (FishingFrenzy.Instance == null)
        {
            Set_Error();
        }
        else
        {
            var state = FishingFrenzy.Instance.State;
            switch (state)
            {
                case FishingFrenzy.EffectState.InCooldown:
                    {
                        var remaining = FishingFrenzy.Instance.GetRemainingCooldownDuration();
                        Set_InCooldown(remaining.Hours, remaining.Minutes, remaining.Seconds);
                        break;
                    }
                case FishingFrenzy.EffectState.Available:
                    Set_Available();
                    break;
                case FishingFrenzy.EffectState.CurrentlyActive:
                    {
                        var remaining = FishingFrenzy.Instance.GetRemainingActiveDuration();
                        Set_Active(remaining.Minutes, remaining.Seconds);
                        break;
                    }
            }
        }
    }

    private void Common()
    {
        txt_active.enabled = false;
        txt_available.enabled = false;
        txt_error.enabled = false;
        txt_inCooldown.enabled = false;
        img_shine.enabled = false;
        txt_title.color = col_notInCooldownText;
        img_bg.color = col_notInCooldownBG;
    }

    public void Set_Active(int remainingMinutes, int remainingSeconds)
    {
        Common();

        img_shine.enabled = true;
        img_shine.color = col_activeShine;

        bool isFirst = true;

        StringBuilder str = new StringBuilder(16);
        str.Append("ACTIVE (");
        if (remainingMinutes > 0)
        {
            str.Append(remainingMinutes.ToString().PadLeft(2, '0'));
            str.Append('m');
            str.Append(' ');
            isFirst = false;
        }
        str.Append(isFirst ? remainingSeconds.ToString() : remainingSeconds.ToString().PadLeft(2, '0'));
        str.Append('s');
        str.Append(')');

        txt_active.enabled = true;
        txt_active.text = str.ToString();

        // Event
        OnStateUpdated.Invoke();
    }
    public void Set_Available()
    {
        Common();

        img_shine.enabled = true;
        img_shine.color = col_availableShine;

        txt_available.enabled = true;

        // Event
        OnStateUpdated.Invoke();
    }
    public void Set_InCooldown(int remainingHours, int remainingMinutes, int remainingSeconds)
    {
        Common();

        bool isFirst = true;

        StringBuilder str = new StringBuilder(16);
        str.Append("Dans ");
        if (remainingHours > 0)
        {
            str.Append(remainingHours);
            str.Append('h');
            str.Append(' ');
            isFirst = false;
        }
        if (remainingMinutes > 0)
        {
            str.Append(isFirst ? remainingMinutes.ToString() : remainingMinutes.ToString().PadLeft(2, '0'));
            str.Append('m');
            str.Append(' ');
            isFirst = false;
        }
        str.Append(isFirst ? remainingSeconds.ToString() : remainingSeconds.ToString().PadLeft(2, '0'));
        str.Append('s');

        txt_inCooldown.enabled = true;
        txt_inCooldown.text = str.ToString();

        txt_title.color = col_inCooldownText;
        img_bg.color = col_inCooldownBG;

        // Event
        OnStateUpdated.Invoke();
    }
    public void Set_Error()
    {
        Common();
        txt_error.enabled = true;

        // Event
        OnStateUpdated.Invoke();
    }
}
