using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_1_1 : QuestStep
{
    private void OnEnable()
    {
        EventManager.AddEventListener("Quest_1_1Action", UpdateQuestStepInfo);
    }

    private void Start()
    {
        EventManager.EventTrigger("OnChangeQuestState", questStepConfig.questID, QuestState.CanStart);
    }

    private void OnDisable()
    {
        EventManager.RemoveEventListener("Quest_1_1Action", UpdateQuestStepInfo);
    }

    protected override void FinishQuestStep()
    {
        EventManager.EventTrigger("OnStartQuest", questStepConfig.questID);
    }
}
