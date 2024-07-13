using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEditor.Compilation;
using UnityEngine;
using Assembly = System.Reflection.Assembly;

[CreateAssetMenu(fileName = "New Game Setting", menuName = "Framework/Game Setting")]
public class GameSetting : ConfigBase
{
    public Dictionary<string, UIElement> UIElementDic = new Dictionary<string, UIElement>();
    
#if UNITY_EDITOR
    [Button(Name = "初始化游戏配置")]
    public void InitForEditor()
    {
        UIElementAttributeOnEditor();
    }

    private void UIElementAttributeOnEditor()
    {
        UIElementDic.Clear();  
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())  
        {  
            Type[] typeArray = assembly.GetTypes();  
            foreach (Type type in typeArray)  
            {  
                if (type.IsClass && !type.IsAbstract && type.BaseType != null)  
                {  
                    Type baseType = type.BaseType;
                    if (baseType.IsGenericType && baseType.BaseType != null && baseType.BaseType.IsGenericType && baseType.BaseType.GetGenericTypeDefinition() == typeof(UIScreenController<>))  
                    {  
                        Type[] genericArguments = baseType.GetGenericArguments();  
                        if (genericArguments.Length == 1 && typeof(IScreenProperties).IsAssignableFrom(genericArguments[0]))  
                        {  
                            UIElementAttribute attribute = type.GetCustomAttribute<UIElementAttribute>();
                            Debug.Log(attribute);
                            if (attribute != null)  
                            {  
                                UIElementDic.Add(type.Name, new UIElement()  
                                {  
                                    prefab = Resources.Load<GameObject>(attribute.prefabPath),  
                                });  
                            }  
                        }  
                    }  
                }  
            }  
        }
    }
#endif
}
