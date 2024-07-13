using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Group Config", menuName = "Config/Dialogue Module/Dialogue Group")]
public class DialogueGroupConfig : ConfigBase
{
    public string npcID;
    public Dictionary<string, DialogueConfig> dialogueConfigDic;

    private void OnValidate()
    {
        dialogueConfigDic = dialogueConfigDic
            .Where(pair => pair.Value != null)
            .GroupBy(pair => pair.Value.dialogueID)
            .ToDictionary(pairs => pairs.Key, pairs => pairs.Last().Value);
        
        EditorUtility.SetDirty(this);
    }
}