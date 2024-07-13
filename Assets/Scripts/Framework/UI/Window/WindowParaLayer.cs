using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 这是一个“辅助”层级，以便显示优先级更高的窗口，专门需要蒙黑处理的（不关闭上一个窗口）
/// 默认情况下，它包含任何标记为弹出窗口的窗口。它由 WindowUILayer 控制
/// </summary>
public class WindowParaLayer : MonoBehaviour
{
    /// <summary>
    /// 蒙黑背景游戏物体
    /// </summary>
    [SerializeField] private GameObject darkenBGGameObject = null;
    
    private List<GameObject> containedScreenList = new List<GameObject>();

    /// <summary>
    /// 窗口传递，添加到该层级的窗口并重置层级，在添加进list中管理
    /// 因为弹窗可以在弹窗，所以需要添加到list中管理，上一个弹窗关闭了，从list中取另一个在显示
    /// </summary>
    public void AddScreen(Transform screenTransform)
    {
        screenTransform.SetParent(transform, false);
        containedScreenList.Add(screenTransform.gameObject);
    }

    /// <summary>
    /// 蒙黑刷新，如果list中还有弹窗在显示，那就需要将蒙黑打开
    /// </summary>
    public void RefreshDarken()
    {
        for (int i = 0; i < containedScreenList.Count; i++)
        {
            if (containedScreenList[i] != null && containedScreenList[i].activeSelf)
            {
                darkenBGGameObject.SetActive(true);
                return;
            }
        }
        
        darkenBGGameObject.SetActive(false);
    }

    public void DarkenBG()
    {
        darkenBGGameObject.SetActive(true);
        darkenBGGameObject.transform.SetAsLastSibling();
    }
}
