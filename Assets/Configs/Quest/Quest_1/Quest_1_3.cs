using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_1_3 : QuestStep
{
    private void OnEnable()
    {
        EventManager.AddEventListener("Quest_1_3Action", UpdateQuestStepInfo);
    }

    private void Start()
    {
        EventManager.EventTrigger("OnChangeQuestState", questStepConfig.questID, QuestState.CanFinish);
    }

    private void OnDisable()
    {
        EventManager.RemoveEventListener("Quest_1_3Action", UpdateQuestStepInfo);
    }

    protected override void FinishQuestStep()
    {
        EventManager.EventTrigger("OnFinishQuest", questStepConfig.questID);
    }
}
