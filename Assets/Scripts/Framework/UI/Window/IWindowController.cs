using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 窗口接口, 窗口类
/// </summary>
public interface IWindowController : IScreenController
{
    bool HideOnForegroundLost { get; }
    bool IsPopup { get; }
    bool DontDestroyOnHide { get; }
    WindowPriority WindowPriority { get; }
}
