using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Step Config" , menuName = "Config/Quest Module/Quest Step")]
public class QuestStepConfig : ConfigBase
{
    [HideInInspector] public string questID;
    [HideInInspector] public int questStepIndex;
    public string dialogueID;
    public string timelineID;
    
    public string questStepName;
    [TextArea] public string questStepDescription;
    [TextArea] public string questStepText;
    public int questStepTargetNumber;

    public GameObject questStepPrefab;
}
