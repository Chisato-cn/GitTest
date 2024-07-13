using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Config Setting", menuName = "Framework/Config Setting")]
public class ConfigSetting : ConfigBase
{
    [SerializeField, DictionaryDrawerSettings(KeyLabel = "类型", ValueLabel = "列表")]
    private Dictionary<string, Dictionary<int, ConfigBase>> configSettingDic = new Dictionary<string, Dictionary<int, ConfigBase>>();

    public T GetConfigForID<T>(string configTypeName, int configID) where T : ConfigBase
    {
        //检查类型
        if (!configSettingDic.ContainsKey(configTypeName))
        {
            throw new System.Exception("JK:配置设置中不包含这个Key:" + configTypeName);
        }
        //检查ID
        if (!configSettingDic[configTypeName].ContainsKey(configID))
        {
            throw new System.Exception($"JK:配置设置中{configTypeName}不包含这个ID:{configID}");
        }
        //说明一切正常
        return configSettingDic[configTypeName][configID] as T;
    }
    
    public List<T> GetConfigForID<T>(string configTypeName) where T : ConfigBase
    {
        //检查类型
        if (!configSettingDic.ContainsKey(configTypeName))
        {
            throw new System.Exception("JK:配置设置中不包含这个Key:" + configTypeName);
        }

        List<T> list = new List<T>();
        foreach (var config in configSettingDic[configTypeName].Values)
        {
            list.Add((T)config);
        }
        
        //说明一切正常
        return list;
    }
}
