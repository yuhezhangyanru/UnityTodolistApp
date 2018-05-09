using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateObjMenu  
{ 
    /// <summary>
    /// 创建一个本地的用户配置对象
    /// </summary>
    [MenuItem("Menus/CreateUserConfig(创建数据库配置文件)")]
    public static void CreateUserConfig()
    {
        var nameInfoObj = ScriptableObject.CreateInstance<UserConfig>();
        nameInfoObj.name = StringConst.USER_CONFIG_NAME;
        UnityEditor.AssetDatabase.CreateAsset(nameInfoObj, "Assets/Resources/" + nameInfoObj.name + ".asset");
    }
     
    /// <summary>
    /// 更新：prefab维护列表
    /// </summary>
    [MenuItem("Menus/UpdateResourceMapObj(更新prefab维护对象的列表)")]
    public static void UpdateResourceMapObj()
    { 
        var resoureObj = ScriptableObject.CreateInstance<ResourceMap>();
        resoureObj.name = StringConst.RESOURCE_OBJ_NAME;
        resoureObj.prefabList = new List<ResourceItemPair>();

        var objs = Resources.LoadAll("Prefabs");
        for (int index = 0; index < objs.Length; index++)
        {
            var temp = objs[index];
            resoureObj.prefabList.Add(new ResourceItemPair(temp.name.ToLower(), temp as GameObject));
        }
        string path = "Assets/Resources/" + resoureObj.name + ".asset";
        //UnityEditor.AssetDatabase.DeleteAsset(
        UnityEditor.AssetDatabase.CreateAsset(resoureObj, path);
    }
}
