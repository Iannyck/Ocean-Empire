using System.Collections;
using System.Collections.Generic;
using CCC;
using System;
using System.Text;
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


    protected override string OwnedUpgradeKey
    {
        get
        {
            return "tt1";
        }
    }
    protected override string NextUpgGenCodeKey
    {
        get
        {
            return "tt2";
        }
    }
    protected override string OwnedUpgGenKey
    {
        get
        {
            return "tt3";
        }
    }

    public ThrusterCategory()
    {
        nvcSpeed = new CCC.Math.NeverReachingCurve
        {
            a = 1,
            b = 5,
            speed = 0.03f,
            minX = 0
        };

        nvcAcceleration = new CCC.Math.NeverReachingCurve
        {
            a = 1,
            b = 5,
            speed = 0.05f,
            minX = 0
        };
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

        int picture = UnityEngine.Random.Range(0, Icons.Count);     //Ä ENLEVER UN JOUR!!


        StringBuilder genCodeBuilder = new StringBuilder();

        String description = "Vitesse: " + speed.ToString("F") + "\nAcceleration :" + acceleration.ToString("F") + "\nDeceleration :" + acceleration.ToString("F"); ;

        genCodeBuilder.Append("Moteur niveau ");
        genCodeBuilder.Append(level.ToString());
        genCodeBuilder.Append('@');
        genCodeBuilder.Append(level.ToString());
        genCodeBuilder.Append('@');
        genCodeBuilder.Append(description);
        genCodeBuilder.Append('@');
        genCodeBuilder.Append(coin);
        genCodeBuilder.Append('@');
        genCodeBuilder.Append(ticket);
        genCodeBuilder.Append('@');
        genCodeBuilder.Append(picture);
        genCodeBuilder.Append('@');
        genCodeBuilder.Append(speed.ToString("F"));
        genCodeBuilder.Append('@');
        genCodeBuilder.Append(acceleration.ToString("F"));
        genCodeBuilder.Append('@');
        genCodeBuilder.Append(acceleration.ToString("F"));

        nextUpgGenCode = genCodeBuilder.ToString();
    }

    public override ThrusterDescription GenerateNextDescription(string nextUpgGenCode)
    {
        string[] stringSeparators = new string[] { "@" };
        string[] result = nextUpgGenCode.Split(stringSeparators, StringSplitOptions.None);

        ThrusterDescription description = new ThrusterDescription(result[0], int.Parse(result[1]), result[2], int.Parse(result[3]), 
            Convert.ToInt32(result[4]), Icons[ int.Parse(result[5]) ], float.Parse(result[6]), float.Parse(result[7]), float.Parse(result[8]));

        return description;
    }
}


