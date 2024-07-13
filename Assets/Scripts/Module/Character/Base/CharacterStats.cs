using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStats<TCharacter> : ConfigBase where TCharacter : CharacterBase<TCharacter>
{
    [Header("基础配置")]
    public float gravity = 38f;
    public float slideForce = 10f;
    public float fallGravity = 65f;
    public float gravityMaxSpeed = 50f;
    
    [Header("运动配置")]
    public float deceleration = 28f;
    public float friction = 28f;
    public float slopeFriction = 18f;
}
