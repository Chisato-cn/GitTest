using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITest : MonoBehaviour
{
    private void Start()
    {
       
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            UIManager.Instance.Show("UI_MainWindow");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            EventManager.EventTrigger("Quest_1_1Action");
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            EventManager.EventTrigger("OnShowDialogueWindow", "NPC_Anny");
        }
    }

}
