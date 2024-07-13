using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 面板属性参数类
/// </summary>
[Serializable]
public class PanelProperties : IPanelProperties
{
    [SerializeField, Tooltip("面板层级")] private PanelPriority priority;

    public PanelPriority Priority { get; set; }
    public bool DontDestroyOnHide { get; set; }
}
