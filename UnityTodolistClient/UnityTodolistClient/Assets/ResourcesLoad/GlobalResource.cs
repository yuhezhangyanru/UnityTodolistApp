 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 全局变量及资源索引控制器
/// </summary>
public class GlobalResource
{
    private static GlobalResource _instance;
    public static GlobalResource instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GlobalResource();
            }
            return _instance;
        }
    }

    /// <summary>
    /// 用户配置信息
    /// </summary>
    public UserConfig userConfig;

    /// <summary>
    /// 所有的prefab，一律使用小写访问取用，机制可用
    /// </summary>
    private ResourceMap _resourcesObj;
    private Dictionary<string, GameObject> _resourceDic = new Dictionary<string, GameObject>();

    protected GlobalResource()
    {
        userConfig = Resources.Load(StringConst.USER_CONFIG_NAME) as UserConfig;
        if (userConfig == null)
        {
            Logger.LogError("return！请先关闭游戏执行菜单Menus->CreateUserConfig 生成配置文件！");
            return;
        }

        _resourcesObj = Resources.Load(StringConst.RESOURCE_OBJ_NAME) as ResourceMap;
        for (int index = 0; index < +_resourcesObj.prefabList.Count; index++)
        {
            _resourceDic.Add(_resourcesObj.prefabList[index].key, _resourcesObj.prefabList[index].value);
        }
    }

    /// <summary>
    /// 加载使用资源对象
    /// </summary>
    /// <param name="path"></param>
    /// <param name="loadCallback"></param>
    public void LoadResources(string path, Action<GameObject> loadCallback)
    {
        if (!_resourceDic.ContainsKey(path))
        {
            Logger.LogError("path=" + path + ",的资源不存在！或关闭游戏后执行 Menus->UpdateResourceMapObj刷新资源列表重试！");
            return;
        }
        loadCallback(_resourceDic[path]);//TODO 后续可能需要改为异步的加载
    }
}
