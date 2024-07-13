using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : SingletonMono<QuestManager>
{
    public Dictionary<string, Quest> questDic;
    private Dictionary<string, QuestData> questData;
    
    private void OnEnable()
    {
        EventManager.AddEventListener("OnTryToActiveQuest", TryToActiveQuest);
        
        EventManager.AddEventListener<string>("OnStartQuest", StartQuest);
        EventManager.AddEventListener<string>("OnAdvanceQuest", AdvanceQuest);
        EventManager.AddEventListener<string>("OnFinishQuest", FinishQuest);
        EventManager.AddEventListener<string, int, QuestStepState>("OnQuestStepStateChange", QuestStepStateChange);
        
        EventManager.AddEventListener<string, QuestState>("OnChangeQuestState", ChangeQuestState);
    }

    private void Start()
    {
        CreateQuestDic();
        TryToActiveQuest();
    }

    private void OnDisable()
    {
        EventManager.RemoveEventListener("OnTryToActiveQuest", TryToActiveQuest);
        
        EventManager.RemoveEventListener<string>("OnStartQuest", StartQuest);
        EventManager.RemoveEventListener<string>("OnAdvanceQuest", AdvanceQuest);
        EventManager.RemoveEventListener<string>("OnFinishQuest", FinishQuest);
        EventManager.RemoveEventListener<string, int, QuestStepState>("OnQuestStepStateChange", QuestStepStateChange);
        
        EventManager.RemoveEventListener<string, QuestState>("OnChangeQuestState", ChangeQuestState);
    }

    private void CreateQuestDic()
    {
        List<QuestConfig> questConfigList = ConfigManager.Instance.GetConfigForID<QuestConfig>("任务组");
        questDic = new Dictionary<string, Quest>();

        foreach (QuestConfig questConfig in questConfigList)
        {
            if (questDic.ContainsKey(questConfig.questID))
            {
                Debug.LogError("任务字典初始化:已存在相同id,初始化失败!任务ID:" + questConfig.questID);
            }
            questDic.Add(questConfig.questID, new Quest(questConfig));
        }
    }

    private void TryToActiveQuest()
    {
        foreach (Quest quest in questDic.Values)
        {
            if (quest.questState == QuestState.Invalid && CheckRequirementsMet(quest))
            {
                quest.InstantiateStepPrefab(this.transform);
            }
        }
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        bool meetRequirements = true;

        foreach (QuestConfig questConfig in quest.questConfig.questRequisiteList)
        {
            if (GetQuestByID(questConfig.questID).questState != QuestState.Finished)
            {
                meetRequirements = false;
            }
        }

        return meetRequirements;
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestByID(id);
        quest.questState = state;
        
        EventManager.EventTrigger("OnQuestStateChange", quest);
    }

    private void StartQuest(string id)
    {
        Quest quest = GetQuestByID(id);
        quest.MoveToNextStep();
        
        if (quest.CurrentStepExits())
        {
            quest.InstantiateStepPrefab(this.transform);
        }
       
        ChangeQuestState(quest.questConfig.questID, QuestState.InProgress);
    }

    private void AdvanceQuest(string id)
    {
        Quest quest = GetQuestByID(id);
        quest.MoveToNextStep();

        if (quest.CurrentStepExits())
        {
            quest.InstantiateStepPrefab(this.transform);
        }
    }

    private void FinishQuest(string id)
    {
        Quest quest = GetQuestByID(id);
        ClaimReward(quest);
        ChangeQuestState(quest.questConfig.questID, QuestState.Finished);
        
    }

    private void QuestStepStateChange(string id, int questStepIndex, QuestStepState questStepState)
    {
        Quest quest = GetQuestByID(id);
        quest.StoreQuestStepState(questStepIndex, questStepState);
        ChangeQuestState(quest.questConfig.questID, quest.questState);
    }

    private void ClaimReward(Quest quest)
    {
        
    }

    private Quest GetQuestByID(string id)
    {
        Quest quest = questDic[id];
       
        if (quest == null)
        {
            Debug.LogError("任务字典中找不到任务!ID:" + id);
        }

        return quest;
    }

    private void SaveQuest(Quest quest)
    {
        QuestData questData = quest.GetQuestData();
        
    }

    private Quest LoadQuest()
    {
        return null;
    }
}
