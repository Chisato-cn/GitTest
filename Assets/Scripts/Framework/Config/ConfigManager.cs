using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : SingletonMono<ConfigManager>
{
    [SerializeField] private ConfigSetting configSetting;
    [SerializeField] private GameSetting gameSetting;
    
    public T GetConfigForID<T>(string configTypeName, int configID) where T : ConfigBase
    {
        return configSetting.GetConfigForID<T>(configTypeName, configID);
    }
    
    public List<T> GetConfigForID<T>(string configTypeName) where T : ConfigBase
    {
        return configSetting.GetConfigForID<T>(configTypeName);
    }

    public UIElement GetUIElementForID(string screenID)
    {
        return gameSetting.UIElementDic[screenID];
    }
}
