using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class PanelUILayer : UILayer<IPanelController>
{
    /// <summary>
    /// 注册后的面板将根据其面板层级重新归属到不同的面板层级下
    /// </summary>
    [SerializeField, DictionaryDrawerSettings(KeyLabel = "层级枚举", ValueLabel = "对应层级枚举的Transform")]
    private Dictionary<PanelPriority, Transform> priorityLayerDic = new Dictionary<PanelPriority, Transform>();

    /// <summary>
    /// 如果是IPanel类型的才允许调用PanelParaLayer，重置为Panel的Layer子物体，反之调用基类的SetParent
    /// </summary>
    public override void ResetScreenParent(IScreenController screen, Transform screenTransform)
    {
        IPanelController panel = screen as IPanelController;

        if (panel != null)
        {
            ResetParentToParaLayer(panel.Priority, screenTransform);
        }
        else
        {
            base.ResetScreenParent(screen, screenTransform);
        }
    }

    /// <summary>
    /// PanelParaLayer中有None层，Priority层，Tutorials层，Blocker层，重置的时候需要选择获取层的Transform
    /// </summary>
    private void ResetParentToParaLayer(PanelPriority priority, Transform screenTransform) 
    {
        Transform trans;
        if (!priorityLayerDic.TryGetValue(priority, out trans))
        {
            trans = transform;
        }
            
        screenTransform.SetParent(trans, false);
    }

    /// <summary>
    /// 向外部提供一个IsPanelVisible的函数用于检查这个Panel是否正在显示，类似于公司框架的IsShowing
    /// </summary>
    public bool IsPanelVisible(string panelID)
    {
        IPanelController panel;
        if (registeredScreenDic.TryGetValue(panelID, out panel))
        {
            return panel.IsVisible;
        }
        
        return false;
    }
    
    /// <summary>
    /// 然后就是实现Show，Hide函数，负责调用UI自身的Show和Hide即可
    /// </summary>
    public override void ShowScreen(IPanelController screen)
    {
        screen.Show();
    }

    /// <summary>
    /// 然后就是实现Show，Hide函数，负责调用UI自身的Show和Hide即可
    /// </summary>
    public override void ShowScreen<TProperties>(IPanelController screen, TProperties properties)
    {
        screen.Show(properties);
    }

    /// <summary>
    /// 然后就是实现Show，Hide函数，负责调用UI自身的Show和Hide即可
    /// </summary>
    public override void HideScreen(IPanelController screen)
    {
       screen.Hide();
    }
}
