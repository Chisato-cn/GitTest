using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 窗口控制器基类
/// </summary>
public abstract class WindowController<TProperties> : UIScreenController<TProperties>, IWindowController where TProperties : IWindowProperties
{
    public bool HideOnForegroundLost => Properties.HideOnForegroundLost;
    public bool IsPopup => Properties.IsPopup;
    public bool DontDestroyOnHide => Properties.DontDestroyOnHide;
    public WindowPriority WindowPriority => Properties.WindowPriority;
        
    protected sealed override void SetProperties(TProperties properties) 
    {
        if (properties != null) 
        {
            if (!properties.SuppressPrefabProperties) 
            {
                properties.HideOnForegroundLost = Properties.HideOnForegroundLost;
                properties.WindowPriority = Properties.WindowPriority;
                properties.IsPopup = Properties.IsPopup;
                properties.DontDestroyOnHide = Properties.DontDestroyOnHide;
            }

            Properties = properties;
        }
    }

    protected override void HierarchyFixOnShow() 
    {
        transform.SetAsLastSibling();
    }
    
}
