using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// UILayer是管理层，是一个泛型，他限定了传进来的UI必须继承IScreen接口
/// </summary>
public abstract class UILayer<TScreen> : SerializedMonoBehaviour where TScreen : IScreenController
{
    /// <summary>
    /// UILayer对界面UI的操作是基于自身的，所以需要一个字典来存储注册的界面UI
    /// key为界面UI的ID，value为界面UI的实例
    /// 对于缓存字典，向外部提供RegisterScreen方法，内部提供UnregisterScreen方法，管理缓存字典的元素
    /// </summary>
    protected Dictionary<string, TScreen> registeredScreenDic = new Dictionary<string, TScreen>();

    /// <summary>
    /// 提供给外部使用的初始化Init，让UILayer在合适的时机去进行初始化
    /// </summary>
    public virtual void Init() 
    {
        registeredScreenDic = new Dictionary<string, TScreen>();
    }

    /// <summary>
    /// 注册界面
    /// </summary>
    public void RegisterScreen(string screenID, TScreen screen)
    {
        if (!registeredScreenDic.ContainsKey(screenID))
        {
            ProcessScreenRegister(screenID, screen);
        }
        else
        {
            Debug.LogError("该界面ID已经注册,请检查! 界面ID:" + screenID);
        }
    }
    
    /// <summary>
    /// 注册具体操作逻辑，抽象的只需要将其添加进缓存字典，其余由实现类去重写
    /// 销毁事件：从字典走注销逻辑：字典移除并取消销毁事件
    /// </summary>
    protected virtual void ProcessScreenRegister(string screenID, TScreen screen) 
    {
        screen.ScreenID = screenID;
        registeredScreenDic.Add(screenID, screen);
        screen.ScreenDestroyed += OnScreenDestroyed;
    }

    /// <summary>
    /// 重置注册的界面UI的父节点, IScreenController让实现类去操作
    /// </summary>
    /// <param name="screen"></param>
    /// <param name="screenTransform"></param>
    public virtual void ResetScreenParent(IScreenController screen, Transform screenTransform)
    {
        screenTransform.SetParent(transform, false);
    }

    /// <summary>
    /// 界面销毁事件函数，涉及到缓存字典和注销，自然在UILayer中实现是最好的
    /// </summary>
    private void OnScreenDestroyed(IScreenController screen)
    {
        if (!string.IsNullOrEmpty(screen.ScreenID) && registeredScreenDic.ContainsKey(screen.ScreenID))
        {
            UnregisterScreen(screen.ScreenID, (TScreen)screen);
        }
    }

    /// <summary>
    /// 注销界面
    /// </summary>
    public void UnregisterScreen(string screenID, TScreen screen)
    {
        if (registeredScreenDic.ContainsKey(screenID))
        {
            ProcessScreenUnregister(screenID, screen);
        }
        else
        {
            Debug.LogError("该界面ID未曾注册,无法注销,请检查! 界面ID:" + screenID);
        }
    } 
    
    /// <summary>
    /// 注销具体操作逻辑，抽象的只需要将其从缓存字典移除，其余由实现类去重写
    /// 销毁事件：从字典走注销逻辑：字典移除并取消销毁事件
    /// </summary>
    protected virtual void ProcessScreenUnregister(string screenID, TScreen screen) 
    {
        screen.ScreenDestroyed -= OnScreenDestroyed;
        registeredScreenDic.Remove(screenID);
    }
    
    /// <summary>
    /// 根据界面ID检查是否已经注册, 显示和隐藏前必须检查, 否则会报错
    /// </summary>
    public bool IsScreenRegistered(string screenID) 
    {
        return registeredScreenDic.ContainsKey(screenID);
    }

    /// <summary>
    /// 根据界面ID在注册字典中查找界面并显示
    /// </summary>
    public void ShowScreenByID(string screenID)
    {
        TScreen screen;
        if (registeredScreenDic.TryGetValue(screenID, out screen))
        {
            ShowScreen(screen);
        }
        else
        {
            Debug.LogError("该界面ID未注册,无法显示,请检查! 界面ID:" + screenID);
        }
    }
    
    /// <summary>
    /// 根据界面ID在注册字典中查找界面并显示, 同时传递属性参数
    /// </summary>
    public void ShowScreenByID<TProperties>(string screenID, TProperties properties) where TProperties : IScreenProperties
    {
        TScreen screen;
        if (registeredScreenDic.TryGetValue(screenID, out screen))
        {
            ShowScreen(screen, properties);
        }
        else
        {
            Debug.LogError("该界面ID未注册,无法显示,请检查! 界面ID:" + screenID);
        }
    }

    /// <summary>
    /// 根据界面ID在注册字典中查找界面并隐藏
    /// </summary>
    public void HideScreenByID(string screenID)
    {
        TScreen screen;
        if (registeredScreenDic.TryGetValue(screenID, out screen))
        {
            HideScreen(screen);
        }
        else
        {
            Debug.LogError("该界面ID未注册,无法隐藏,请检查! 界面ID:" + screenID);
        }
    }

    /// <summary>
    /// 隐藏所有界面
    /// </summary>
    public virtual void HideAll(bool shouldAnimateWhenHiding = true)
    {
        foreach (KeyValuePair<string, TScreen> pair in registeredScreenDic)
        {
            pair.Value.Hide(shouldAnimateWhenHiding);
        }
    }

    /// <summary>
    /// 显示界面
    /// </summary>
    public abstract void ShowScreen(TScreen screen);

    /// <summary>
    /// 显示界面, 并传递属性参数
    /// </summary>
    public abstract void ShowScreen<TProperties>(TScreen screen, TProperties properties) where TProperties : IScreenProperties;
    
    /// <summary>
    /// 隐藏界面
    /// </summary>
    public abstract void HideScreen(TScreen screen);
}
