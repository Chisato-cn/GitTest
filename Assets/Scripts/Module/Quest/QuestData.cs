using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestData
{
    public QuestState questState;
    public int currentQuestStepIndex;
    public List<QuestStepState> questStepStateList;

    public QuestData(QuestState questState, int currentQuestStepIndex, List<QuestStepState> questStepStateList)
    {
        this.questState = questState;
        this.currentQuestStepIndex = currentQuestStepIndex;
        this.questStepStateList = questStepStateList;
    }
}
