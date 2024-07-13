using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueGroup
{
    public DialogueGroupConfig dialogueGroupConfig;
    public List<DialogueConfig> canStartDialogueConfiglist;

    public DialogueGroup(DialogueGroupConfig dialogueGroupConfig)
    {
        this.dialogueGroupConfig = dialogueGroupConfig;

        canStartDialogueConfiglist = new List<DialogueConfig>();
        foreach (DialogueConfig dialogueConfig in this.dialogueGroupConfig.dialogueConfigDic.Values)
        {
            if (dialogueConfig.dialogueType != DialogueType.Quest)
            {
                canStartDialogueConfiglist.Add(dialogueConfig);
            }
        }
    }

    public DialogueConfig GetRandomDialogueConfig()
    {
        List<DialogueConfig> tempList = new List<DialogueConfig>();

        foreach (DialogueConfig config in canStartDialogueConfiglist)
        {
            if (config.dialogueType == DialogueType.Random)
            {
                tempList.Add(config);
            }
        }
        
        int index = Random.Range(0, tempList.Count);
        return tempList[index];
    }

    public void AddDialogueConfigToCanStartList(string id)
    {
        DialogueConfig dialogueConfig = GetDialogueConfigByID(id);
        
        if (canStartDialogueConfiglist.Contains(dialogueConfig))
        {
            Debug.LogError("可对话列表中已存在该对话!ID:" + id);
        }
        else
        {
            canStartDialogueConfiglist.Add(dialogueConfig);
        }
    }

    private DialogueConfig GetDialogueConfigByID(string id)
    {
        DialogueConfig dialogueConfig = dialogueGroupConfig.dialogueConfigDic[id];

        if (dialogueConfig == null)
        {
            Debug.LogError("对话组字典中找不到对话!ID:" + id);
        }

        return dialogueConfig;
    }
}
