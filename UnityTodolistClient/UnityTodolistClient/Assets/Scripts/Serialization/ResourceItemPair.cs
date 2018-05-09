using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
/// <summary>
/// function：用于记录prefab对象的引用
/// </summary>
[System.Serializable]
public class ResourceItemPair
{
    public string key;
    public GameObject value;

    public ResourceItemPair(string key, GameObject value)
    {
        this.key = key;
        this.value = value;
    }
}
