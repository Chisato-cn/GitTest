using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueEventType
{
    StartDialogue,
    NextDialogue,
    JumpDialogue,
    ExitDialogue,
    
    FinishQuest,
    StartQuest,
}

public enum DialogueType
{
    Random,
    Daily,
    Quest,
}