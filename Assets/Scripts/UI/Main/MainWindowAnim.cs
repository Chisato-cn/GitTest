using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;

public class MainWindowAnim : AnimComponent
{
    [SerializeField] private float fadeDuration = 0.75f;
    
    [SerializeField] private RectTransform systemButtonContainer;
    [SerializeField] private Ease slideEase = Ease.InOutQuad;
    
    private Sequence sequence;
    private float fadeEndValue;
    
    public override void Animate(Transform target, Action callWhenFinished)
    {
        sequence = DOTween.Sequence();
        sequence.Pause();
        
        // 引用获取
        CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>() != null
            ? target.GetComponent<CanvasGroup>()
            : target.gameObject.AddComponent<CanvasGroup>();
        
        // 参数初始化
        canvasGroup.alpha = isOut ? 1f : 0f;
        fadeEndValue = isOut ? 0f : 1f;
        sequence.Append(systemButtonContainer.DOAnchorPosX(isOut ? 0f : systemButtonContainer.rect.width, 0f));
        
        // 动画添加
        sequence.Append(systemButtonContainer.DOAnchorPosX(isOut ? systemButtonContainer.rect.width : 0f, fadeDuration)).SetEase(slideEase);
        sequence.Insert(0f, canvasGroup.DOFade(fadeEndValue, fadeDuration).OnComplete(callWhenFinished.Invoke));
        
        sequence.Play();
    }
    
}
