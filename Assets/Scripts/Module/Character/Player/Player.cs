using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase<Player>
{
    protected override void Awake()
    {
        base.Awake();

    }



    private void Start()
    {
        // StateMachine.ChangeState<>();
    }
    
    public override void InitStateMachine()
    {
        StateMachine = new PlayerStateMachine(this);
    }
}
