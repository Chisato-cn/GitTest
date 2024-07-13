using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 面板控制器基类
/// </summary>
public abstract class PanelController<T> : UIScreenController<T>, IPanelController where T : IPanelProperties
{
    public PanelPriority Priority => Properties != null ? Properties.Priority : PanelPriority.None;

    protected sealed override void SetProperties(T properties)
    {
        base.SetProperties(properties);
    }
}
