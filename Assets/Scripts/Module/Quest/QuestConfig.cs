using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Config", menuName = "Config/Quest Module/Quest")]
public class QuestConfig : ConfigBase
{
    [field: SerializeField] public string questID { get; private set; }

    public string questName;
    [TextArea] public string questDescription;

    public List<QuestStepConfig> questStepConfigList;
    
    public int playerLevelRequirement;
    public List<QuestConfig> questRequisiteList;

    public int questRewardExperience;
    public List<GameObject> questRewardItemList;

    private void OnValidate()
    {
        questID = this.name;
        for (int i = 0; i < questStepConfigList.Count; i++)
        {
            QuestStepConfig questStepConfig = questStepConfigList[i];
            questStepConfig.questID = questID;
            questStepConfig.questStepIndex = i;
            EditorUtility.SetDirty(questStepConfig);
        }
        EditorUtility.SetDirty(this);
    }
}

