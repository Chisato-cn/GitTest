using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI界面基类
/// </summary>
public abstract class UIScreenController<TProperties> : MonoBehaviour, IScreenController where TProperties : IScreenProperties
{
    [Header("界面动画配置")]
    [SerializeField, Tooltip("界面显示动画")] private AnimComponent animIn;
    [SerializeField, Tooltip("界面隐藏动画")] private AnimComponent animOut;

    [Header("界面属性配置")]
    [SerializeField, Tooltip("界面属性")] private TProperties properties;

    public string ScreenID { get; set; }
    public bool IsVisible { get; private set; }

    public AnimComponent AnimIn
    {
        get => animIn;
        set => animIn = value;
    }
    
    public AnimComponent AnimOut   
    {
        get => animOut;
        set => animOut = value;
    }

    public TProperties Properties
    {
        get => properties;
        set => properties = value;
    }

    public Action<IScreenController> InTransitionFinished { get; set; }
    public Action<IScreenController> OutTransitionFinished { get; set; }
    public Action<IScreenController> ScreenDestroyed { get; set; }

    protected virtual void Awake()
    {
        AddListener();
    }
    
    protected virtual void AddListener()
    {
        
    }
    
    protected virtual void RemoveListener()
    {
        
    }

    protected virtual void OnDestroy()
    {
        ScreenDestroyed?.Invoke(this);
        
        InTransitionFinished = null;
        OutTransitionFinished = null;
        ScreenDestroyed = null;
        
        RemoveListener();
    }

    protected virtual void SetProperties(TProperties properties)
    {
        this.properties = properties;
    }

    /// <summary>
    /// 属性参数设置到界面的时候触发，在SetProperties之后触发，比较安全的能取到值
    /// </summary>
    protected virtual void OnPropertiesSet()
    {

    }
    
    /// <summary>
    /// 在显示的时候处理一些层级，或者属性处理等，具体看继承者重写
    /// </summary>
    protected virtual void HierarchyFixOnShow()
    {
        
    }

    public void Show(IScreenProperties properties = null)
    {
        if (properties != null)
        {
            if (properties is TProperties)
            {
                SetProperties((TProperties)properties);
            }
            else
            {
                Debug.LogError("传递的属性类型与当前界面属性类型不匹配!属性类型：" + properties.GetType() + "，当前界面属性类型：" + typeof(TProperties));
                return;
            }
        }
        
        HierarchyFixOnShow();
        OnPropertiesSet();

        if (!gameObject.activeSelf)
        {
            DoAnimation(animIn, OnTransitionInFinished, true);
        }
        else
        {
            InTransitionFinished?.Invoke(this);
        }
    }

    public virtual void Refresh(IScreenProperties properties = null)
    {
      
    }

    public void Hide(bool animate = true)
    {
        DoAnimation(animate ? animOut : null, OnTransitionOutFinished, false);
        WhileHiding();
    }

    protected virtual void WhileHiding()
    {
        
    }

    private void DoAnimation(AnimComponent caller, Action callWhenFinished, bool isVisible)
    {
        if (caller == null)
        {
            gameObject.SetActive(isVisible);
            callWhenFinished?.Invoke();
        }
        else
        {
            if (isVisible && !gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
      
            caller.Animate(transform, callWhenFinished);
        }
    }

    private void OnTransitionInFinished()
    {
        IsVisible = true;
        InTransitionFinished?.Invoke(this);
    }
    
    private void OnTransitionOutFinished()
    {
        IsVisible = false;
        
        OutTransitionFinished?.Invoke(this);
        gameObject.SetActive(false);
    }
}