using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;

public class QuestWindowAnim : AnimComponent
{
    [SerializeField] private float fadeDuration = 0.75f;
    
    [SerializeField] private Ease slideEase = Ease.InOutQuad;
    
    private Sequence sequence;
    private float fadeEndValue;
    
    public override void Animate(Transform target, Action callWhenFinished)
    {
        sequence = DOTween.Sequence();
        sequence.Pause();
        
        // 引用获取
        RectTransform rectTransform = target as RectTransform;
        CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>() != null
            ? target.GetComponent<CanvasGroup>()
            : target.gameObject.AddComponent<CanvasGroup>();
        
        // 参数初始化
        canvasGroup.alpha = isOut ? 1f : 0f;
        fadeEndValue = isOut ? 0f : 1f;
        
        sequence.Append(rectTransform.DOAnchorPosY(isOut ? 0f : rectTransform.rect.height, 0f));
        
        // 动画添加
        sequence.Append(rectTransform.DOAnchorPosY(isOut ? -rectTransform.rect.height : 0f, fadeDuration)).SetEase(slideEase);
        sequence.Insert(0f, canvasGroup.DOFade(fadeEndValue, fadeDuration).OnComplete(callWhenFinished.Invoke));

        sequence.Play();
    }
    
}
