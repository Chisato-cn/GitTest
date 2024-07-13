using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : SingletonMono<DialogueManager>
{
    public Dictionary<string, DialogueGroup> dialogueGroupDic;
    private DialogueConfig currentDialogueConfig;
    
    private void OnEnable()
    {
        EventManager.AddEventListener<Quest>("OnQuestStateChange", QuestStateChange);
        EventManager.AddEventListener<string>("OnShowDialogueWindow", ShowDialogueWindow);
    }

    private void Start()
    {
        CreateDialogueGroupDic();
    }

    private void OnDisable()
    {
        EventManager.RemoveEventListener<Quest>("OnQuestStateChange", QuestStateChange);
        EventManager.RemoveEventListener<string>("OnShowDialogueWindow", ShowDialogueWindow);
    }

    private void CreateDialogueGroupDic()
    {
        List<DialogueGroupConfig> questConfigList = ConfigManager.Instance.GetConfigForID<DialogueGroupConfig>("对话组");
        dialogueGroupDic = new Dictionary<string, DialogueGroup>();

        foreach (DialogueGroupConfig dialogueGroupConfig in questConfigList)
        {
            if (dialogueGroupDic.ContainsKey(dialogueGroupConfig.npcID))
            {
                Debug.LogError("对话组字典初始化:已存在相同id,初始化失败!对话组的npcID:" + dialogueGroupConfig.npcID);
            }
            dialogueGroupDic.Add(dialogueGroupConfig.npcID, new DialogueGroup(dialogueGroupConfig));
        }
    }

    private void QuestStateChange(Quest quest)
    {
        foreach (DialogueGroup dialogueGroup in dialogueGroupDic.Values)
        {
            string dialogueID = quest.questConfig.questStepConfigList[quest.currentQuestStepIndex].dialogueID;
            if (dialogueGroup.dialogueGroupConfig.dialogueConfigDic.ContainsKey(dialogueID))
            {
                dialogueGroup.AddDialogueConfigToCanStartList(dialogueID);
                break;
            }
        }
    }

    
    private void ShowDialogueWindow(string npcID)
    {
        UIManager.Instance.Show("UI_DialogueWindow", new DialogueWindowProperties()
        {
            dialogueGroup = dialogueGroupDic[npcID],
        });
    }

    private void FinishQuestDialogue()
    {
        
    }
}
