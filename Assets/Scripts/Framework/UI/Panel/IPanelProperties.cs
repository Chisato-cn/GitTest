using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPanelProperties : IScreenProperties
{
    // 面板层级
    PanelPriority Priority { get; set; }
}
