using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// function:用于维护prefab游戏对象
/// </summary>
public class ResourceMap : UnityEngine.ScriptableObject
{
    [SerializeField]
    public List<ResourceItemPair> prefabList;
}