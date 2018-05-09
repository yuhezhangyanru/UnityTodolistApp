using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// function:序列化对象，用来存放一些本地的用户设置，如数据库连接信息等
/// </summary>
public class UserConfig : UnityEngine.ScriptableObject
{
    [Header("数据库名称")]
    public string DataBaseName = "";
    [Header("数据库IP")]
    public string DataBaseIP = "";
    [Header("数据库用户名")]
    public string DbUserID = "";
    [Header("数据库密码")]
    public string DbPassword = "";
    [Header("数据库端口")]
    public string DbPort = "";
}
