using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_1_2 : QuestStep
{
    private void OnEnable()
    {
        EventManager.AddEventListener("Quest_1_2Action", UpdateQuestStepInfo);
    }

    private void OnDisable()
    {
        EventManager.RemoveEventListener("Quest_1_2Action", UpdateQuestStepInfo);
    }

    protected override void FinishQuestStep()
    {
        EventManager.EventTrigger("OnAdvanceQuest", questStepConfig.questID);
    }
}
