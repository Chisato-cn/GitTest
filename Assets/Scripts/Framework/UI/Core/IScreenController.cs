using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 顶层接口
/// IScreen接口是界面UI接口，规定了界面UI必须有的属性ScreenID和IsVisible，以及方法Show和Hide
/// 还有界面过渡前后的事件回调，关闭请求事件回调，以及界面销毁事件回调
/// 除了IsVisible是由界面UI自己控制只给外部get，其他都是外部可以get和set
/// </summary>
public interface IScreenController
{
    string ScreenID { get; set; }
    bool IsVisible { get; }
    
    Action<IScreenController> InTransitionFinished { get; set; }
    Action<IScreenController> OutTransitionFinished { get; set; }
    Action<IScreenController> ScreenDestroyed { get; set; }
    
    void Show(IScreenProperties properties = null);
    void Refresh(IScreenProperties properties = null);
    void Hide(bool animate = true);
}
