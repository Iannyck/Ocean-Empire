using UnityEngine;
using System.Collections;

public class StatesIdle : States {

    public StatesIdle(Personnage personnage) : base(personnage)
    {
        nom = "Idle";
        this.personnage = personnage; // Personnage ayant cet état
    }

    public override void Enter()
    {

    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {

    }
}
