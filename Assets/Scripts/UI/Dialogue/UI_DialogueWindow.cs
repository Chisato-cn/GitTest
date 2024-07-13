using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public class DialogueWindowProperties : WindowProperties
{
    public DialogueGroup dialogueGroup;
}

[UIElement("UI/Window/DialogueWindow")]
public class UI_DialogueWindow : WindowController<DialogueWindowProperties>
{
    [SerializeField] private Button autoButton;
    [SerializeField] private Button skipButton;
    [SerializeField] private Button switchButton;
    
    [SerializeField] private GameObject interactionItemPrefab;
    [SerializeField] private Transform optionItemRoot;
    [SerializeField] private TMP_Text dialogueContent;
    [SerializeField] private Transform nextArrow;
    [SerializeField] private float speed = 5f;
    
    private DialogueConfig currentDialogue;
    private int currentPieceIndex;
    private bool isDoingText = false;
    private List<UI_InteractItem> uiInteractItemList = new List<UI_InteractItem>();
    
    protected override void AddListener()
    {
        EventManager.AddEventListener<DialogueEventType, string>("ParseDialogueEvent", ParseDialogueEvent);
    }

    protected override void RemoveListener()
    {
        EventManager.RemoveEventListener<DialogueEventType, string>("ParseDialogueEvent", ParseDialogueEvent);
    }

    protected override void OnPropertiesSet()
    {
        base.OnPropertiesSet();
        
        Init(GetRandomDialogue());
    }

    private DialogueConfig GetRandomDialogue()
    {
        List<DialogueConfig> tempList = new List<DialogueConfig>();
        foreach (DialogueConfig dialogueConfig in Properties.dialogueGroup.canStartDialogueConfiglist)
        {
            if (dialogueConfig.dialogueType == DialogueType.Random)
            {
                tempList.Add(dialogueConfig);
            }
        }
        return tempList[Random.Range(0, tempList.Count)];
    }

    private void Init(DialogueConfig dialogue)
    {
        currentDialogue = dialogue;
        currentPieceIndex = 0;

        StartDialogue(currentDialogue, currentPieceIndex);
    }

    private void StartDialogue(DialogueConfig dialogue, int index)
    {
        isDoingText = true;
        dialogueContent.text = String.Empty;
        ClearAllInteraction();
        
        Piece piece = GetPiece(dialogue, index);
        // 文本长度/打字速度
        dialogueContent.DOText(piece.pieceContent, piece.pieceContent.Length / speed).OnComplete(() =>
        {
            FinishDoText(dialogue.dialogueType == DialogueType.Random);
        });
    }

    private void InstantiateInteractItem(Piece piece)
    {
        foreach (Option option in piece.optionList)
        {
            UI_InteractItem uiInteractItem = Instantiate(interactionItemPrefab, optionItemRoot).GetComponent<UI_InteractItem>();
            uiInteractItem.Init(option);
            uiInteractItemList.Add(uiInteractItem);
        }
    }

    private void SpeedupText()
    {
        if (!isDoingText) return;

        isDoingText = false;
        dialogueContent.DOKill();
        dialogueContent.text = GetPiece(currentDialogue, currentPieceIndex).pieceContent;
        FinishDoText(currentDialogue.dialogueType == DialogueType.Random);
    }

    private void InstantiateInteractItem()
    {
        foreach (DialogueConfig dialogueConfig in Properties.dialogueGroup.canStartDialogueConfiglist)
        {
            if (dialogueConfig.dialogueType != DialogueType.Random) 
            {
                UI_InteractItem uiInteractItem = Instantiate(interactionItemPrefab, optionItemRoot).GetComponent<UI_InteractItem>();
                
                uiInteractItem.Init(new Option()
                {
                    optionContent = dialogueConfig.dialogueName,
                    optionEventInfoList = new List<EventInfo>()
                    {
                        new EventInfo()
                        {
                            eventType = DialogueEventType.StartDialogue,
                            eventArg = dialogueConfig.dialogueID,
                        }
                    },
                });
                uiInteractItemList.Add(uiInteractItem);
            }
        }
    }

    private void FinishDoText(bool isDialogueContentChoiceInteract)
    {
        isDoingText = false;
        if (isDialogueContentChoiceInteract)
        {
            InstantiateInteractItem();
        }
        else
        {
            InstantiateInteractItem(GetPiece(currentDialogue, currentPieceIndex));
        }
    }

    private Piece GetPiece(DialogueConfig dialogue, int pieceIndex)
    {
        return dialogue.pieceList[pieceIndex];;
    }

    private void ClearAllInteraction()
    {
        foreach (UI_InteractItem uiInteractItem in uiInteractItemList)
        {
            Destroy(uiInteractItem.gameObject);
        }
        
        uiInteractItemList.Clear();
    }

    private void ParseDialogueEvent(DialogueEventType type, string arg)
    {
        switch (type)
        {
            case DialogueEventType.StartDialogue:
                StartDialogueEvent(arg);
                break;
            case DialogueEventType.NextDialogue:
                NextDialogueEvent();
                break;
            case DialogueEventType.JumpDialogue:
                JumpDialogueEvent(int.Parse(arg));
                break;
            case DialogueEventType.ExitDialogue:
                ExitDialogueEvent();
                break;
            case DialogueEventType.FinishQuest:
                break;
            case DialogueEventType.StartQuest:
                break;
        }
    }

    private void StartDialogueEvent(string id)
    {
        Init(Properties.dialogueGroup.dialogueGroupConfig.dialogueConfigDic[id]);
    }
    
    private void JumpDialogueEvent(int index)
    {
        currentPieceIndex = index;
        StartDialogue(currentDialogue, currentPieceIndex);
    }

    private void NextDialogueEvent()
    {
        currentPieceIndex++;

        if (currentPieceIndex >= currentDialogue.pieceList.Count)
        {
            ExitDialogueEvent();
        }
        else
        {
            StartDialogue(currentDialogue, currentPieceIndex);
        }
    }

    private void ExitDialogueEvent()
    {
        UIManager.Instance.Show("UI_MainWindow");
    }


    private void SwitchState(bool isHide = false)
    {
        
    }

    private void AutoPlay(bool isAuto = false)
    {
        
    }

    private void SkipDialogue()
    {
        
    }
}
