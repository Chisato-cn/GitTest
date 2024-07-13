using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
    Main,
    Branch,
    Achieve,
}

public enum QuestState
{
    Invalid,
    CanStart,
    InProgress,
    CanFinish,
    Finished,
}