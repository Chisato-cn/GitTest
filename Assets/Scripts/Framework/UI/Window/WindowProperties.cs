using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 窗口通用属性
/// </summary>
[Serializable]
public class WindowProperties : IWindowProperties
{
    [SerializeField] protected bool hideOnForegroundLost = true;
    [SerializeField] protected WindowPriority windowPriority = WindowPriority.ForceForeground;
    [SerializeField] protected bool isPopup = false;
    [SerializeField] protected bool dontDestroyOnHide = true;
    
    /// <summary>
    /// 窗口行为层级枚举
    /// 若存在已打开的窗口，此窗口应该如何表现? Force Foreground 会立即打开它，Enqueue 会将它排队，以便在当前窗口关闭后立即打开
    /// </summary>
    public WindowPriority WindowPriority
    {
        get => windowPriority;
        set => windowPriority = value;
    }
    
    /// <summary>
    ///  当前窗口若丢失前台状态是否需要隐藏
    /// </summary>
    public bool HideOnForegroundLost
    {
        get => hideOnForegroundLost;
        set => hideOnForegroundLost = value;
    }
    
    /// <summary>
    /// 当前窗口是否属于弹窗(需要置于顶层)
    /// </summary>
    public bool IsPopup
    {
        get => isPopup;
        set => isPopup = value;
    }
    
    /// <summary>
    /// 是否需要缓存
    /// </summary>
    public bool DontDestroyOnHide
    {
        get => dontDestroyOnHide;
        set => dontDestroyOnHide = value;
    }
    
    
    /// <summary>
    /// 窗口打开一般附带参数，这个参数是否可以覆盖窗口预制体的属性
    /// </summary>
    public bool SuppressPrefabProperties { get; set; }


    public WindowProperties()
    {
        hideOnForegroundLost = true;
        windowPriority = WindowPriority.ForceForeground;
        isPopup = false;
        dontDestroyOnHide = true;
    }
    
   
}
