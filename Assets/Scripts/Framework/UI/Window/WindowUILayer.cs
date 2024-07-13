using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 这个layer层控制所有的窗口.
/// 有显示记录和队列的，并且一次只显示一个
/// </summary>
public class WindowUILayer : UILayer<IWindowController>
{
    [SerializeField] private WindowParaLayer priorityParaLayer = null;

    private Queue<WindowHistoryEntry> windowQueue;
    private Stack<WindowHistoryEntry> windowHistory;
    private HashSet<IScreenController> screenTransitioning;

    public event Action RequestScreenBlock;
    public event Action RequestScreenUnblock;

    public bool IsScreenTransitionProgress => screenTransitioning.Count != 0;
    public IWindowController CurrentWindow { get; private set; }

    public override void Init()
    {
        base.Init();
        registeredScreenDic = new Dictionary<string, IWindowController>();
        windowQueue = new Queue<WindowHistoryEntry>();
        windowHistory = new Stack<WindowHistoryEntry>();
        screenTransitioning = new HashSet<IScreenController>();
    }

    protected override void ProcessScreenRegister(string screenID, IWindowController screen)
    {
        base.ProcessScreenRegister(screenID, screen);
        screen.InTransitionFinished += OnInAnimationFinished;
        screen.OutTransitionFinished += OnOutAnimationFinished;
    }
    
    /// <summary>
    /// 渐入动画完成事件
    /// </summary>
    private void OnInAnimationFinished(IScreenController screen) 
    {
        RemoveTransition(screen);
    }

    /// <summary>
    /// 渐出动画完成事件
    /// </summary>
    private void OnOutAnimationFinished(IScreenController screen) 
    {
        RemoveTransition(screen);
        var window = screen as IWindowController;
        if (window.IsPopup) 
        {
            priorityParaLayer.RefreshDarken();
        }
    }

    protected override void ProcessScreenUnregister(string screenID, IWindowController screen)
    {
        base.ProcessScreenUnregister(screenID, screen);
        screen.InTransitionFinished -= OnInAnimationFinished;
        screen.OutTransitionFinished -= OnOutAnimationFinished;
    }

    public override void ShowScreen(IWindowController screen)
    {
        ShowScreen<WindowProperties>(screen, null);
    }

    public override void ShowScreen<K>(IWindowController screen, K properties)
    {
        IWindowProperties windowProperties = properties as IWindowProperties;

        if (ShouldEnqueue(screen, windowProperties))
        {
            // 进队列
            EnqueueWindow(screen, windowProperties);
        }
        else
        {
            // 进栈
            DoShow(screen, windowProperties);
        }
    }
    
    public override void ResetScreenParent(IScreenController screen, Transform screenTransform)
    {
        IWindowController window = screen as IWindowController;

        if (window == null)
        {
            Debug.LogError("当前界面: " + screenTransform.name + " 不是窗口!");
        }
        else 
        {
            if (window.IsPopup)
            {
                priorityParaLayer.AddScreen(screenTransform);
                return;
            }
        }

        base.ResetScreenParent(screen, screenTransform);
    }
    
    private bool ShouldEnqueue(IWindowController window, IWindowProperties properties) 
    {
        if (CurrentWindow == null && windowQueue.Count == 0) 
        {
            return false;
        }

        if (properties != null && properties.SuppressPrefabProperties) 
        {
            return properties.WindowPriority != WindowPriority.ForceForeground;
        }

        if (window.WindowPriority != WindowPriority.ForceForeground) 
        {
            return true;
        }

        return false;
    }
    
    private void EnqueueWindow<T>(IWindowController screen, T properties) where T : IScreenProperties
    {
        windowQueue.Enqueue(new WindowHistoryEntry(screen, (IWindowProperties)properties));
    }

    public override void HideAll(bool shouldAnimateWhenHiding = true)
    {
        base.HideAll(shouldAnimateWhenHiding);
        CurrentWindow = null;
        priorityParaLayer.RefreshDarken();
        windowHistory.Clear();
    }
    
    public override void HideScreen(IWindowController screen)
    {
        if (screen == CurrentWindow)
        {
            windowHistory.Pop();
            AddTransition(screen);
            screen.Hide();

            CurrentWindow = null;

            if (windowQueue.Count > 0) 
            {
                ShowNextInQueue();
            }
            else if (windowHistory.Count > 0) 
            {
                ShowPreviousInHistory();
            }
            
        }
        else
        {
            Debug.LogError("需要关闭的窗口不是当前窗口!窗口ID: " + screen.ScreenID + "当前窗口ID: " + CurrentWindow.ScreenID);
        }
    }
    
    public void ShowNextInQueue() 
    {
        if (windowQueue.Count > 0) 
        {
            WindowHistoryEntry window = windowQueue.Dequeue();
            DoShow(window);
        }
    }
    
    private void ShowPreviousInHistory() 
    {
        if (windowHistory.Count > 0) 
        {
            WindowHistoryEntry window = windowHistory.Pop();
            DoShow(window);
        }
    }
    
    private void DoShow(IWindowController screen, IWindowProperties properties) 
    {
        DoShow(new WindowHistoryEntry(screen, properties));
    }
    
    private void DoShow(WindowHistoryEntry windowEntry)
    {
        if (CurrentWindow == windowEntry.Screen) 
        {
            Debug.LogError("当前窗口已经打开!窗口ID: " + windowEntry.Screen.ScreenID);
        }
        else if (CurrentWindow != null && CurrentWindow.HideOnForegroundLost && !windowEntry.Screen.IsPopup) 
        {
            CurrentWindow.Hide();
        }

        windowHistory.Push(windowEntry);
        AddTransition(windowEntry.Screen);

        if (windowEntry.Screen.IsPopup) 
        {
            priorityParaLayer.DarkenBG();
        }

        windowEntry.Show();
        CurrentWindow = windowEntry.Screen;
    }
    
    private void AddTransition(IScreenController screen) 
    {
        screenTransitioning.Add(screen);
        RequestScreenBlock?.Invoke();
    }
    
    private void RemoveTransition(IScreenController screen) {
        screenTransitioning.Remove(screen);
        if (!IsScreenTransitionProgress) 
        {
            RequestScreenUnblock?.Invoke();
        }
    }
    
    
}
