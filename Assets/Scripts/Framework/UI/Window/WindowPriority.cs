using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 窗口优先级, 用于定义窗口在打开时, 在历史记录和队列中的行为
/// </summary>
public enum WindowPriority
{
    ForceForeground,
    Enqueue,
}
