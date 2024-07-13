using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 具体动画实现, 渐入渐出动画
/// </summary>
public class FadeAnim : AnimComponent
{
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private bool fadeOut = false;

    private CanvasGroup canvasGroup;
    private Transform currentTarget;
    private Action currentAction;
    private float timer;

    private float startValue;
    private float endValue;

    private bool shouldAnimate;

    public override void Animate(Transform target, Action callWhenFinished)
    {
        if (currentAction != null)
        {
            canvasGroup.alpha = endValue;
            currentAction.Invoke();
        }

        currentTarget = target;
        canvasGroup = currentTarget.GetComponent<CanvasGroup>()
            ? currentTarget.GetComponent<CanvasGroup>()
            : currentTarget.transform.AddComponent<CanvasGroup>();

        startValue = fadeOut ? 1f : 0f;
        endValue = fadeOut ? 0f : 1f;

        currentAction = callWhenFinished;
        timer = fadeDuration;

        canvasGroup.alpha = startValue;
        shouldAnimate = true;
    }

    private void Update()
    {
        if (!shouldAnimate) return;

        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(endValue, startValue, timer / fadeDuration);
        }
        else
        {
            canvasGroup.alpha = 1f;
            currentAction?.Invoke();
   
            shouldAnimate = false;
            currentAction = null;
        }
    }
}
