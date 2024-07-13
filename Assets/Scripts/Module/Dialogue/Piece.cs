using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Piece
{
    public string pieceContent;
    public List<EventInfo> pieceEventInfoList = new List<EventInfo>();
    public List<Option> optionList;
}
