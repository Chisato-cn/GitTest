using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public QuestConfig questConfig;
    public QuestState questState;
    public int currentQuestStepIndex;
    public List<QuestStepState> questStepStateList;

    /// <summary>
    /// 初始化
    /// </summary>
    public Quest(QuestConfig questConfig)
    {
        this.questConfig = questConfig;
        questState = QuestState.Invalid;
        currentQuestStepIndex = 0;
        questStepStateList = new List<QuestStepState>(this.questConfig.questStepConfigList.Count);
        for (int i = 0; i < this.questConfig.questStepConfigList.Count; i++)
        {
            questStepStateList.Add(new QuestStepState());
        }
    }

    /// <summary>
    /// 加载数据
    /// </summary>
    public Quest(QuestConfig questConfig, QuestState questState, int currentQuestStepIndex, List<QuestStepState> questStepStateList)
    {
        this.questConfig = questConfig;
        this.questState = questState;
        this.currentQuestStepIndex = currentQuestStepIndex;
        this.questStepStateList = questStepStateList;
    }

    public void MoveToNextStep()
    {
        currentQuestStepIndex++;
    }

    public bool CurrentStepExits()
    {
        return currentQuestStepIndex < questConfig.questStepConfigList.Count;
    }

    public void InstantiateStepPrefab(Transform parent)
    {
        GameObject prefab = GetQuestStepPrefab();
        if (prefab != null)
        {
            QuestStep questStep = GameObject.Instantiate(prefab, parent).GetComponent<QuestStep>();
            questStep.Init(questConfig.questStepConfigList[currentQuestStepIndex]);
        }
    }

    public void StoreQuestStepState(int questStepStateIndex, QuestStepState questStepState)
    {
        if (questStepStateIndex < questStepStateList.Count)
        {
            questStepStateList[questStepStateIndex] = questStepState;
        }
        else
        {
            Debug.LogError("任务步骤状态存储时索引超出范围!任务ID:" + questConfig.questID + "任务步骤索引:" + questStepStateIndex);
        }
    }

    private GameObject GetQuestStepPrefab()
    {
        GameObject prefab = null;

        if (CurrentStepExits())
        {
            prefab = questConfig.questStepConfigList[currentQuestStepIndex].questStepPrefab;
        }
        else
        {
            Debug.LogError("任务步骤预制体不存在! 任务ID:" + questConfig.questID + "任务步骤:" + currentQuestStepIndex);
        }
        
        return prefab;
    }

    public QuestData GetQuestData()
    {
        return new QuestData(questState, currentQuestStepIndex, questStepStateList);
    }
}
