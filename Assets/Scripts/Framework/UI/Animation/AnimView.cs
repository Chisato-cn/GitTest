using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 具体动画实现，这是基于animationClip的形式
/// </summary>
public class AnimView : AnimComponent
{
    [SerializeField, Tooltip("动画片段")] private AnimationClip clip = null;
    [SerializeField, Tooltip("倒叙播放")] private bool playReverse = false;
    
    private Action previousCallbackWhenFinished;

    public override void Animate(Transform target, Action callWhenFinished)
    {
        FinishPrevious();
        Animation anim = target.GetComponent<Animation>();
     
        if (anim == null)
        {
            Debug.LogError("目标对象并未持有Animation组件!target:" + target);
            callWhenFinished?.Invoke();
            return;
        }

        anim.clip = clip;
        StartCoroutine(DoPlayAnimation(anim, callWhenFinished));
    }

    private IEnumerator DoPlayAnimation(Animation anim, Action callWhenFinished)
    {
        previousCallbackWhenFinished = callWhenFinished;
        // 反转播放
        foreach (AnimationState state in anim)
        {
            state.time = playReverse ? state.clip.length : 0f;
            state.speed = playReverse ? -1f : 1f;
        }

        anim.Play(PlayMode.StopAll);
        yield return new WaitForSeconds(anim.clip.length);
        FinishPrevious();
    }

    private void FinishPrevious()
    {
        previousCallbackWhenFinished?.Invoke();
        previousCallbackWhenFinished = null;
        StopAllCoroutines();
    }
}
