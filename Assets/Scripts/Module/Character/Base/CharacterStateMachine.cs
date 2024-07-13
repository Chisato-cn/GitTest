using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStateMachine<TCharacter> where TCharacter : CharacterBase<TCharacter>
{
    protected Dictionary<Type, CharacterStateBase<TCharacter>> stateDic = new Dictionary<Type, CharacterStateBase<TCharacter>>();
    
    /// <summary>
    /// 当前状态
    /// </summary>
    public CharacterStateBase<TCharacter> CurrentState { get; protected set; }
    
    /// <summary>
    /// 状态机宿主
    /// </summary>
    public TCharacter Owner { get; protected set; }


    public CharacterStateMachine(TCharacter owner)
    {
        Owner = owner;
    }

    /// <summary>
    /// 切换状态
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public void ChangeState<TState>() where TState : CharacterStateBase<TCharacter>, new()
    {
        Type type = typeof(TState);

        if (CurrentState != null)
        {
            CurrentState.Exit(Owner);
        }

        CurrentState = GetState<TState>();
        CurrentState.Enter(Owner);
    }
    
    /// <summary>
    /// 状态每帧执行
    /// </summary>
    public virtual void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update(Owner);
        }
    }

    /// <summary>
    /// 获得指定类型状态
    /// </summary>
    /// <typeparam name="TState">目标状态类型</typeparam>
    private CharacterStateBase<TCharacter> GetState<TState>() where TState : CharacterStateBase<TCharacter>, new() 
    {
        Type type = typeof(TState);

        if (!stateDic.TryGetValue(type, out CharacterStateBase<TCharacter> state))
        {
            state = new TState();
            stateDic.Add(type, state);
        }

        return state;
    }

    /// <summary>
    /// 当前状态是否为指定状态
    /// </summary>
    public virtual bool IsCurrentOfType(Type type)
    {
        if (CurrentState == null)
        {
            return false;
        }

        return CurrentState.GetType() == type;
    }
    
    /// <summary>
    /// 状态机停止
    /// </summary>
    public void Stop()
    {
        CurrentState.Exit(Owner);
        CurrentState = null; 
        stateDic.Clear();
    }
}
