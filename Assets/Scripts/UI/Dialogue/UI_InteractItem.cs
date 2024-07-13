using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InteractItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text content;
    [SerializeField] private GameObject highlightTip;
    [SerializeField] private GameObject highlight;
    [SerializeField] private GameObject highlightSlot;
    [SerializeField] private float fadeDuration = 0.5f;

    private Button button;
    private Option option;
    private Sequence sequence;

    private void Awake()
    {
        button = GetComponent<Button>();
        
        button.onClick.AddListener(ClickEvent);
    }

    public void Init(Option option)
    {
        this.option = option;

        content.text = option.optionContent;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DoAnimate(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DoAnimate(true);
    }

    private void ClickEvent()
    {
        for (int i = 0; i < option.optionEventInfoList.Count; i++)
        {
            EventManager.EventTrigger("ParseDialogueEvent", option.optionEventInfoList[i].eventType, option.optionEventInfoList[i].eventArg);
        }
    }

    private void DoAnimate(bool isOut = false)
    {
        sequence = DOTween.Sequence();
        
        float endValue = isOut ? 0f : 1f;
        
        sequence.Append(highlightSlot.GetComponent<Image>().DOColor(new Color(1, 1, 1, endValue), fadeDuration));
        sequence.Insert(0f, highlight.GetComponent<Image>().DOColor(new Color(1, 1, 1, endValue), fadeDuration));
        sequence.Insert(0f, highlightTip.transform.DOScaleY(endValue, fadeDuration));
    }

    private void OnDestroy()
    {
        sequence.Kill();
        button.onClick.RemoveListener(ClickEvent);
    }
}
