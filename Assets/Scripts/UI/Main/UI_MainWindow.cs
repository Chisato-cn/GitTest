using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class MainWindowProperties : WindowProperties
{
    
}

[UIElement("UI/Window/MainWindow")]
public class UI_MainWindow : WindowController<MainWindowProperties>
{
    [SerializeField] private Button questButton;
    
    protected override void AddListener()
    {
        questButton.onClick.AddListener(ShowQuestWindow);
    }

    protected override void RemoveListener()
    {
        questButton.onClick.RemoveListener(ShowQuestWindow);
    }


    #region UI相关

    private void ShowQuestWindow()
    {
        UIManager.Instance.Show("UI_QuestWindow");
    }

    #endregion
}


