using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 窗口记录和队列
/// </summary>
[Serializable]
public struct WindowHistoryEntry
{
    public readonly IWindowController Screen;
    public readonly IWindowProperties Properties;

    public WindowHistoryEntry(IWindowController screen, IWindowProperties properties)
    {
        Screen = screen;
        Properties = properties;
    }

    public void Show()
    {
        Screen.Show(Properties);
    }
}
