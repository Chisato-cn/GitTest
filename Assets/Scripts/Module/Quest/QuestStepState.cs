using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestStepState
{
    public string questStepState;

    public QuestStepState()
    {
        questStepState = String.Empty;
    }
    
    public QuestStepState(string questStepState)
    {
        this.questStepState = questStepState;
    }
}
