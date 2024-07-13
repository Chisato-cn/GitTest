using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 面板接口, 面板类
/// </summary>
public interface IPanelController : IScreenController
{
    PanelPriority Priority { get; }
}
