using System.Collections;
using System.Collections.Generic;
using CCC;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Ocean Empire/Shop/Upgrade Category/Thruster Category")]
public class ThrusterCategory : UpgradeCategory<ThrusterDescBuilder, ThrusterDescription>
{
    [SerializeField]
    private CCC.Math.NeverReachingCurve nvcSpeed;
    [SerializeField]
    public CCC.Math.NeverReachingCurve nvcAcceleration;
    [SerializeField]
    private List<Sprite> Icons;
    
    public ThrusterCategory()
    {
        nvcSpeed = new CCC.Math.NeverReachingCurve();
        nvcSpeed.a = 0;
        nvcSpeed.b = 5;
        nvcSpeed.speed = 0.1f;
        nvcSpeed.minX = 1;

        nvcAcceleration = new CCC.Math.NeverReachingCurve();
        nvcAcceleration.a = 0;
        nvcAcceleration.b = 5;
        nvcAcceleration.speed = 0.1f;
        nvcAcceleration.minX = 1;
    }

    private int GenCoinCost(int level)
    {
        return level * level * 15;
    }
    private int GenTicketCost(int level)
    {
        return level * level * 3;
    }

    //Gen Code:
    //   []itemName[][]upgradeLevel[][]itemDescription[][]moneyCost[][]ticketCost[][]iconNumber[][]Speed[][]Acceleration[]

    public override void MakeNextGenCode(int level)
    {
        float speed = nvcSpeed.Evalutate((float)level);
        float acceleration = nvcAcceleration.Evalutate((float)level);
        int coin = GenCoinCost(level);
        int ticket = GenTicketCost(level);


        acceleration.ToString("c1");

        string genCode = "[]" + "Moteur niveau " + level.ToString() + "[]"
            + "[]" +level.ToString() + "[]"
            + "[]" + "Vitesse de " + speed + " et acceleration de " + acceleration + "[]"
            + "[]" + coin + "[]"
            + "[]" + ticket + "[]"
            + "[]" + Icons.PickRandom<Sprite>() + "[]"
            + "[]" + speed.ToString("c1") + "[]"
            + "[]" + acceleration.ToString("c1") + "[]";

        nextUpgGenCode = genCode;
        Save();

    }

    public override UpgradeDescription GenerateNextDescription(string nextUpgGenCode)
    {
        string[] stringSeparators = new string[] { "[]" };
        string[] result = nextUpgGenCode.Split(stringSeparators, StringSplitOptions.None);

        ThrusterDescription description = new ThrusterDescription(result[0], Convert.ToInt32(result[1]), result[2],  Convert.ToInt32(result[3]), 
            Convert.ToInt32(result[4]), Icons[Convert.ToInt32(result[5])], (float)Convert.ToDouble(result[6]), (float)Convert.ToDouble(result[7]));

        return description;
    }
}


