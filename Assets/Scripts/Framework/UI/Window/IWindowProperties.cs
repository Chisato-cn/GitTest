using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWindowProperties : IScreenProperties
{
    // 当前窗口若丢失前台状态是否需要隐藏
    bool HideOnForegroundLost { get; set; }
    // 当前窗口是否属于弹窗(需要置于顶层)
    bool IsPopup { get; set; }
    // 窗口行为层级
    WindowPriority WindowPriority { get; set; }
    // 传递属性时窗口是否可以覆盖预制体的属性
    bool SuppressPrefabProperties { get; set; }
}
