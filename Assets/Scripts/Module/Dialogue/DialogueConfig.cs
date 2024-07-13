using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Config", menuName = "Config/Dialogue Module/Dialogue")]
public class DialogueConfig : ConfigBase
{
    [field: SerializeField]
    public string dialogueID { get; private set; }
    public string questID;
    public string dialogueName;

    public DialogueType dialogueType;
    public List<Piece> pieceList;

    private void OnValidate()
    {
        dialogueID = this.name;

        EditorUtility.SetDirty(this);
    }
}
