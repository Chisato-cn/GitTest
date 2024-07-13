using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    protected QuestStepConfig questStepConfig;
    public int currentNumber { get; protected set; }

    public void Init(QuestStepConfig questStepConfig)
    {
        this.questStepConfig = questStepConfig;

        currentNumber = 0;
    }

    protected void UpdateQuestStepInfo()
    {
        currentNumber++;
        if (currentNumber >= questStepConfig.questStepTargetNumber)
        {
            FinishQuestStep();
            Destroy(gameObject);
        }
    }

    protected abstract void FinishQuestStep();
}
