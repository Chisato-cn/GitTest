using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class QuestWindowProperties : WindowProperties
{

}

[UIElement("UI/Window/QuestWindow")]
public class UI_QuestWindow : WindowController<QuestWindowProperties>
{
    [SerializeField] private ToggleGroup group;
    [SerializeField] private Button closeButton;
    [SerializeField] private Transform uiQuestItemRoot;
    [SerializeField] private GameObject uiQuestItemPrefab;

    [Header("详细界面")]
    [SerializeField] private GameObject questStepInfo;
    [SerializeField] private TMP_Text questStepDestination;
    [SerializeField] private TMP_Text questStepName;
    [SerializeField] private TMP_Text questStepText;
    [SerializeField] private TMP_Text questStepDescription;
    
    [Header("简略界面")]
    [SerializeField] private GameObject questInfo;
    [SerializeField] private TMP_Text questDescription;
    [SerializeField] private TMP_Text questName;
    
    protected override void AddListener()
    {
        closeButton.onClick.AddListener(CloseQuestWindow);
        
        EventManager.AddEventListener<Quest>("OnQuestStateChange", QuestStateChange);
    }

    protected override void HierarchyFixOnShow()
    {
        base.HierarchyFixOnShow();
        
        UpdateQuestItemToggleList();
    }

    protected override void RemoveListener()
    {
        closeButton.onClick.RemoveListener(CloseQuestWindow);
        
        EventManager.RemoveEventListener<Quest>("OnQuestStateChange", QuestStateChange);
    }

    private void QuestStateChange(Quest quest)
    {
        UpdateQuestItemToggleList();
    }

    private void UpdateQuestItemToggleList()
    {
        for (int i = 0; i < uiQuestItemRoot.childCount; i++)
        {
            Destroy(uiQuestItemRoot.GetChild(i).gameObject);
        }
        
        foreach (Quest quest in QuestManager.Instance.questDic.Values)
        {
            if (quest.questState != QuestState.Invalid || quest.questState != QuestState.Finished)
            {
                UI_QuestItem questItem = Instantiate(uiQuestItemPrefab, uiQuestItemRoot).GetComponent<UI_QuestItem>();
                questItem.Init(quest, group, UpdateInfo);
            }
        }
    }

    private void UpdateInfo(Quest quest, bool isStepSelected = false)
    {
        QuestStepConfig stepConfig = quest.questConfig.questStepConfigList[quest.currentQuestStepIndex];
        
        questInfo.SetActive(!isStepSelected);
        questStepInfo.SetActive(isStepSelected);

        if (!isStepSelected && quest.questState != QuestState.CanStart)
        {
            // questName.text = quest.questConfig.questName;
            questDescription.text = quest.questConfig.questDescription;
        }
        else
        {
            // questStepDestination.text = questStepConfig.questStepDescription;
            questStepName.text = stepConfig.questStepName;
            questStepText.text = stepConfig.questStepText;
            questStepDescription.text = stepConfig.questStepDescription;
        }
    }

    #region UI相关

    private void CloseQuestWindow()
    {
        UIManager.Instance.Show("UI_MainWindow");
    }

    #endregion
}
