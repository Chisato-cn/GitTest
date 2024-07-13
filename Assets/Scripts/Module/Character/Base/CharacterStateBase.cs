using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStateBase<TCharacter> where TCharacter : CharacterBase<TCharacter>
{
    public float TimeSinceEntered { get; protected set; }

    public void Enter(TCharacter character)
    {
        OnEnter(character);
        TimeSinceEntered = 0;     
    }

    public void Update(TCharacter character)
    {
        OnUpdate(character); 
        TimeSinceEntered += Time.deltaTime;
    }


    public void Exit(TCharacter character)
    {
        OnExit(character);    
    }
    
    protected abstract void OnEnter(TCharacter character);
    protected abstract void OnUpdate(TCharacter character);
    protected abstract void OnExit(TCharacter character);
}
