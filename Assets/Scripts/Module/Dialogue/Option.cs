using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Option
{
    public string optionContent;
    public List<EventInfo> optionEventInfoList = new List<EventInfo>();
}

[Serializable]
public class EventInfo
{
    public DialogueEventType eventType;
    public string eventArg;
}
