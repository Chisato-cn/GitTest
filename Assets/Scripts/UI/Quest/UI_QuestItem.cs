using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestItem : MonoBehaviour
{
    [SerializeField] private Image questSelected;
    [SerializeField] private Image questStepSelected;
    
    private Action<Quest, bool> onSelectedAction;
    private Quest quest;

    public Quest Quest => quest;

    public void Init(Quest quest, ToggleGroup group, Action<Quest, bool> onSelectedAction)
    {
        this.quest = quest;
        this.onSelectedAction = onSelectedAction;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.gameObject.SetActive(i == 0 || this.quest.questState != QuestState.CanStart);
            child.GetComponent<Toggle>().group = group;
            child.GetComponent<Toggle>().onValueChanged.AddListener(i==0 ? OnQuestItemSelected : OnQuestStepItemSelected);
            child.GetComponentInChildren<TMP_Text>().text = i == 0
                ? this.quest.questConfig.questName
                : this.quest.questConfig.questStepConfigList[this.quest.currentQuestStepIndex].questStepName;
        }
    }
    
    private void OnQuestItemSelected(bool isOn)
    {
        questSelected.DOFade(isOn ? 1f : 0f, 0.2f).OnComplete(() => questSelected.DOKill());

        if (isOn) onSelectedAction?.Invoke(quest, false);
    }

    private void OnQuestStepItemSelected(bool isOn)
    {
        questStepSelected.transform.DOScaleY(isOn ? 0.9f : 0f, 0.2f).OnComplete(() => questStepSelected.DOKill());

        if (isOn)
        {
            questSelected.DOKill();
            questSelected.DOFade(1f, 0.2f).OnComplete(() => questSelected.DOKill());
            onSelectedAction?.Invoke(quest, true);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.GetComponent<Toggle>().onValueChanged.AddListener(i==0 ? OnQuestItemSelected : OnQuestStepItemSelected);
        }
    }

 
}
