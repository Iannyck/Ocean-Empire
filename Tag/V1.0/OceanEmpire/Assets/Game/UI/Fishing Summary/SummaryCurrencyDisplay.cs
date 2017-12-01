using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CCC.Manager;
using DG.Tweening;

public class SummaryCurrencyDisplay : MonoBehaviour
{

    public Text moneyAmount;
    public Text tickeyAmount;

    private int money;
    private int ticket;

    private int targetMoney;
    private int targetTicket;

    public float fillTime = 2;
    private float timeCoutner = 0;

    private bool animating = false;


    void OnEnable()
    {
        if (PlayerCurrency.instance == null)
            MasterManager.Sync(Init);
        else
            Init();
    }

    void Init()
    {
        SetCurrencyValues();
    }

    private void Update()
    {
        if (animating == false)
            return;
        else
        {
            if (timeCoutner > fillTime)
            {
                animating = false;
                money = targetMoney;
                ticket = targetTicket;
            }
            else
            {
                timeCoutner += Time.deltaTime;
                UpdateCurrency();
                return;
            }

        }
    }

    void UpdateCurrency()
    {
        int newMoney = money + ((float)(targetMoney - money) * (timeCoutner / fillTime)).RoundedToInt();
        int newTicket = ticket + ((float)(targetTicket - ticket) * (timeCoutner / fillTime)).RoundedToInt();

        if (moneyAmount != null)
            moneyAmount.text = newMoney.ToString();
        if (tickeyAmount != null)
            tickeyAmount.text = newTicket.ToString();
    }
    
    void SetCurrencyValues()
    {
        money = PlayerCurrency.GetCoins();
        ticket = PlayerCurrency.GetTickets();

        if (moneyAmount != null)
            moneyAmount.text = money.ToString();
        if (tickeyAmount != null)
            tickeyAmount.text = ticket.ToString();
    }


    public void IncrementValues(int addedMoney, int addedTicket)
    {
        targetMoney = money + addedMoney;
        targetTicket = ticket + addedTicket;

        timeCoutner = 0;
        animating = true;
    }
}
