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
    
    public ThrusterCategory()
    {
        nvcSpeed = new CCC.Math.NeverReachingCurve();
        nvcSpeed.a = 1;
        nvcSpeed.b = 5;
        nvcSpeed.speed = 0.03f;
        nvcSpeed.minX = 0;

        nvcAcceleration = new CCC.Math.NeverReachingCurve();
        nvcAcceleration.a = 1;
        nvcAcceleration.b = 5;
        nvcAcceleration.speed = 0.05f;
        nvcAcceleration.minX = 0;
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

        genCodeBuilder.Append("Moteur niveau ");
        genCodeBuilder.Append(level.ToString());
        genCodeBuilder.Append('@');
        genCodeBuilder.Append(level.ToString());
        genCodeBuilder.Append('@');
        genCodeBuilder.Append("Vitesse de ");
        genCodeBuilder.Append(speed.ToString("F"));
        genCodeBuilder.Append(" et acceleration de ");
        genCodeBuilder.Append(acceleration.ToString("F"));
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

        nextUpgGenCode = genCodeBuilder.ToString();
    }

    public override UpgradeDescription GenerateNextDescription(string nextUpgGenCode)
    {
        string[] stringSeparators = new string[] { "@" };
        string[] result = nextUpgGenCode.Split(stringSeparators, StringSplitOptions.None);

        ThrusterDescription description = new ThrusterDescription(result[0], int.Parse(result[1]), result[2], int.Parse(result[3]), 
            Convert.ToInt32(result[4]), Icons[ int.Parse(result[5]) ], float.Parse(result[6]), float.Parse(result[7]));

        return description;
    }
}


