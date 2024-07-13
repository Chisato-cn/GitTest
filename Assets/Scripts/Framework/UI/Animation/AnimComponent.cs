using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 界面动画组件
/// </summary>
public abstract class AnimComponent : MonoBehaviour
{
    [SerializeField] protected bool isOut = false;
    
    public abstract void Animate(Transform target, Action callWhenFinished);
}
