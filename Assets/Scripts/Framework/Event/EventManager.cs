using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    #region 内部类+接口

    private interface IEventInfo
    {
        void DestroyInfo();
    }
    
    private class EventInfo : IEventInfo
    {
        public Action action;

        public EventInfo(Action action)
        {
            this.action = action;
        }

        public void DestroyInfo()
        {
            action = null;
        }
    }

    private class EventInfo<T> : IEventInfo
    {
        public Action<T> action;

        public EventInfo(Action<T> action)
        {
            this.action = action;
        }

        public void DestroyInfo()
        {
            action = null;
        }
    }

    private class EventInfo<T, K> : IEventInfo
    {
        public Action<T, K> action;

        public EventInfo(Action<T, K> action)
        {
            this.action = action;
        }

        public void DestroyInfo()
        {
            action = null;
        }
    }
    
    private class EventInfo<T, K, L> : IEventInfo
    {
        public Action<T, K, L> action;

        public EventInfo(Action<T, K, L> action)
        {
            this.action = action;
        }

        public void DestroyInfo()
        {
            action = null;
        }
    }    

    #endregion

    private static Dictionary<string, IEventInfo> eventInfoDic = new Dictionary<string, IEventInfo>();

    #region 添加事件监听

    public static void AddEventListener(string eventName, Action action)
    {
        if (eventInfoDic.TryGetValue(eventName, out IEventInfo eventInfo))
        {
            (eventInfo as EventInfo).action += action;
        }
        else
        {
            eventInfo = new EventInfo(action);
            eventInfoDic.Add(eventName, eventInfo);
        }
    }

    public static void AddEventListener<T>(string eventName, Action<T> action)
    {
        if (eventInfoDic.TryGetValue(eventName, out IEventInfo eventInfo))
        {
            (eventInfo as EventInfo<T>).action += action;
        }
        else
        {
            eventInfo = new EventInfo<T>(action);
            eventInfoDic.Add(eventName, eventInfo);
        }
    }
    
    public static void AddEventListener<T, K>(string eventName, Action<T, K> action)
    {
        if (eventInfoDic.TryGetValue(eventName, out IEventInfo eventInfo))
        {
            (eventInfo as EventInfo<T, K>).action += action;
        }
        else
        {
            eventInfo = new EventInfo<T, K>(action);
            eventInfoDic.Add(eventName, eventInfo);
        }
    }
    
    public static void AddEventListener<T, K, L>(string eventName, Action<T, K, L> action)
    {
        if (eventInfoDic.TryGetValue(eventName, out IEventInfo eventInfo))
        {
            (eventInfo as EventInfo<T, K, L>).action += action;
        }
        else
        {
            eventInfo = new EventInfo<T, K, L>(action);
            eventInfoDic.Add(eventName, eventInfo);
        }
    }
    
    #endregion

    #region 取消事件监听

    public static void RemoveEventListener(string eventName, Action action)
    {
        if (eventInfoDic.TryGetValue(eventName, out IEventInfo eventInfo))
        {
            (eventInfo as EventInfo).action -= action;
        }
    }

    public static void RemoveEventListener<T>(string eventName, Action<T> action)
    {
        if (eventInfoDic.TryGetValue(eventName, out IEventInfo eventInfo))
        {
            (eventInfo as EventInfo<T>).action -= action;
        }
    }
    
    public static void RemoveEventListener<T, K>(string eventName, Action<T, K> action)
    {
        if (eventInfoDic.TryGetValue(eventName, out IEventInfo eventInfo))
        {
            (eventInfo as EventInfo<T, K>).action -= action;
        }
    }

    public static void RemoveEventListener<T, K, L>(string eventName, Action<T, K, L> action)
    {
        if (eventInfoDic.TryGetValue(eventName, out IEventInfo eventInfo))
        {
            (eventInfo as EventInfo<T, K, L>).action -= action;
        }
    }

    #endregion

    #region 事件触发

    public static void EventTrigger(string eventName)
    {
        if (eventInfoDic.TryGetValue(eventName, out IEventInfo eventInfo))
        {
            (eventInfo as EventInfo).action?.Invoke();
        }
    }

    public static void EventTrigger<T>(string eventName, T arg1)
    {
        if (eventInfoDic.TryGetValue(eventName, out IEventInfo eventInfo))
        {
            (eventInfo as EventInfo<T>).action?.Invoke(arg1);
        }
    }
    
    public static void EventTrigger<T, K>(string eventName, T arg1, K arg2)
    {
        if (eventInfoDic.TryGetValue(eventName, out IEventInfo eventInfo))
        {
            (eventInfo as EventInfo<T, K>).action?.Invoke(arg1, arg2);
        }
    }
    
    public static void EventTrigger<T, K, L>(string eventName, T arg1, K arg2, L arg3)
    {
        if (eventInfoDic.TryGetValue(eventName, out IEventInfo eventInfo))
        {
            (eventInfo as EventInfo<T, K, L>).action?.Invoke(arg1, arg2, arg3);
        }
    }
    
    #endregion

    #region 移除事件
    
    public static void RemoveEventListener(string eventName)
    {
        if (eventInfoDic.TryGetValue(eventName, out IEventInfo eventInfo))
        {
            eventInfo.DestroyInfo();
            eventInfoDic.Remove(eventName);
        }
    }
    
    public static void Clear()
    {
        foreach (string eventName in eventInfoDic.Keys)
        {
            eventInfoDic[eventName].DestroyInfo();
        }
        eventInfoDic.Clear();
    }

    #endregion

}
