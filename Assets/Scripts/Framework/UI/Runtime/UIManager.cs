using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMono<UIManager>
{
    private PanelUILayer panelLayer;
    private WindowUILayer windowLayer;

    private Canvas mainCanvas;
    private GraphicRaycaster graphicRaycaster;

    protected override void Awake()
    {
        base.Awake();
        
        mainCanvas = GetComponentInChildren<Canvas>();
    }

    private void Start()
    {
        Init();
    }

    /// <summary>
    /// 以后由GameRoot控制框架系统的初始化顺序,以保证依赖关系正确
    /// </summary>
    private void Init() 
    {
        if (panelLayer == null) 
        {
            panelLayer = gameObject.GetComponentInChildren<PanelUILayer>(true);
            
            if (panelLayer == null) 
            {
                Debug.LogError("UI框架缺少Panel层!");
            }
            else 
            {
                panelLayer.Init();
            }
        }

        if (windowLayer == null) 
        {
            windowLayer = gameObject.GetComponentInChildren<WindowUILayer>(true);
            
            if (windowLayer == null)
            {
                Debug.LogError("UI框架缺少Window层!");
            }
            else 
            {
                windowLayer.Init();
                windowLayer.RequestScreenBlock += OnRequestScreenBlock;
                windowLayer.RequestScreenUnblock += OnRequestScreenUnblock;
            }
        }

        graphicRaycaster = mainCanvas.GetComponent<GraphicRaycaster>();
    }
    
    private void OnRequestScreenBlock() 
    {
        if (graphicRaycaster != null) 
        {
            graphicRaycaster.enabled = false;
        }
    }

    private void OnRequestScreenUnblock() 
    {
        if (graphicRaycaster != null)
        {
            graphicRaycaster.enabled = true;
        }
    }

    public void Show<T>(string screenID, T properties) where T : IScreenProperties
    {
        if (!IsScreenRegistered(screenID))
        {
            UIElement element = ConfigManager.Instance.GetUIElementForID(screenID);
            GameObject screenInstance = Instantiate(element.prefab);
            IScreenController screenController = screenInstance.GetComponent<IScreenController>();
            Transform screenTransform = screenInstance.transform;
            screenInstance.SetActive(false);
            
            RegisterScreen(screenID, screenController, screenTransform);
        }

        ShowScreen(screenID, properties);
    }
    
    public void Show(string screenID)
    {
        if (!IsScreenRegistered(screenID))
        {
            UIElement element = ConfigManager.Instance.GetUIElementForID(screenID);
            GameObject screenInstance = Instantiate(element.prefab);
            IScreenController screenController = screenInstance.GetComponent<IScreenController>();
            Transform screenTransform = screenInstance.transform;
            screenInstance.SetActive(false);
            
            RegisterScreen(screenID, screenController, screenTransform);
        }

        ShowScreen(screenID);
    }

    public void Hide(string screenID)
    {
        if (!IsScreenRegistered(screenID))
        {
            Debug.LogError("尝试关闭的界面并未注册!界面ID: " + screenID);
            return;
        }

        HideScreen(screenID);
    }

    /// <summary>
    /// 显示界面UI
    /// </summary>
    private void ShowScreen(string screenID) 
    {
        Type type;
        if (IsScreenRegistered(screenID, out type)) 
        {
            if (type == typeof(IWindowController)) 
            {
                OpenWindow(screenID);
            }
            else if (type == typeof(IPanelController)) 
            {
                ShowPanel(screenID);
            }
        }
        else
        {
            Debug.LogError("尝试打开的界面并未注册!界面ID: " + screenID + "界面类型: " + type);
        }
    }

    private void ShowScreen<T>(string screenID, T properties) where T : IScreenProperties
    {
        Type type;
        if (IsScreenRegistered(screenID, out type)) 
        {
            if (type == typeof(IWindowController)) 
            {
                CloseCurrentWindow();
                OpenWindow(screenID, properties as IWindowProperties);
            }
            else if (type == typeof(IPanelController)) 
            {
                ShowPanel(screenID, properties as IPanelProperties);
            }
        }
        else
        {
            Debug.LogError("尝试打开的界面并未注册!界面ID: " + screenID + "界面类型: " + type);
        }
    }
    
    /// <summary>
    /// 打开窗口
    /// </summary>
    private void OpenWindow<T>(string screenID, T properties) where T : IWindowProperties 
    {
        windowLayer.ShowScreenByID<T>(screenID, properties);
    }
    
    private void OpenWindow(string screenID)
    {
        windowLayer.ShowScreenByID(screenID);
    }
    
    /// <summary>
    /// 显示面板
    /// </summary>
    private void ShowPanel(string screenID)
    {
        panelLayer.ShowScreenByID(screenID);
    }
    
    private void ShowPanel<T>(string screenID, T properties) where T : IPanelProperties 
    {
        panelLayer.ShowScreenByID<T>(screenID, properties);
    }

    /// <summary>
    /// 隐藏界面
    /// </summary>
    private void HideScreen(string screenID)
    {
        Type type;
        if (IsScreenRegistered(screenID, out type)) 
        {
            if (type == typeof(IWindowController)) 
            {
                CloseWindow(screenID);
            }
            else if (type == typeof(IPanelController)) 
            { 
                HidePanel(screenID);
            }
        }
        else
        {
            Debug.LogError("尝试打开的界面并未注册!界面ID: " + screenID + "界面类型: " + type);
        }
    }
    
    /// <summary>
    /// 关闭窗口
    /// </summary>
    private void CloseWindow(string screenID) 
    {
        windowLayer.HideScreenByID(screenID);
    }
    
    /// <summary>
    /// 隐藏面板
    /// </summary>
    private void HidePanel(string screenID) 
    {
        panelLayer.HideScreenByID(screenID);
    }
    
    /// <summary>
    /// 检查界面是否已经注册
    /// </summary>
    private bool IsScreenRegistered(string screenID) 
    {
        if (windowLayer.IsScreenRegistered(screenID)) return true;
        if (panelLayer.IsScreenRegistered(screenID)) return true;
        return false;
    }
    
    /// <summary>
    /// 检查界面是否已经注册
    /// </summary>
    private bool IsScreenRegistered(string screenID, out Type type) 
    {
        if (windowLayer.IsScreenRegistered(screenID)) 
        {
            type = typeof(IWindowController);
            return true;
        }

        if (panelLayer.IsScreenRegistered(screenID)) 
        {
            type = typeof(IPanelController);
            return true;
        }

        type = null;
        return false;
    }
    
    /// <summary>
    /// 注册界面UI
    /// </summary>
    private void RegisterScreen(string screenID, IScreenController controller, Transform screenTransform) 
    {
        IWindowController window = controller as IWindowController;
        if (window != null) 
        {
            windowLayer.RegisterScreen(screenID, window);
            if (screenTransform != null)
            {
                windowLayer.ResetScreenParent(controller, screenTransform);
            }
            return;
        }

        IPanelController panel = controller as IPanelController;
        if (panel != null) 
        {
            panelLayer.RegisterScreen(screenID, panel);
            if (screenTransform != null) panelLayer.ResetScreenParent(controller, screenTransform);
        }
    }
    
    
    /// <summary>
    /// 关闭当前界面
    /// </summary>
    private void CloseCurrentWindow() 
    {
        if (windowLayer.CurrentWindow != null) 
        {
            CloseWindow(windowLayer.CurrentWindow.ScreenID);    
        }
    }
}
