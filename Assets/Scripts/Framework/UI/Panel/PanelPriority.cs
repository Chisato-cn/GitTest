using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 规定面板属于哪个层的，便于管理
/// </summary>
public enum PanelPriority
{
    None,
    Priority,
    Tutorial,
    Blocker,
}
