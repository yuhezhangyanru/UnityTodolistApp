using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

/// <summary>
/// function:数据表基类，用于方便操作数据表对象,每次生成的新表实体需要继承这个类
/// date:2018-5-9 21:53:57
/// </summary>
public class table_data_base
{
    public List<string> memNameList;
    /// <summary>
    /// 需要重写构造！
    /// </summary>
    public table_data_base()
    {
        MemberInfo[] infos = this.GetType().GetMembers();
        memNameList = new List<string>();
        for (int index = 0; index < infos.Length; index++)
        {
            if (infos[index].MemberType != MemberTypes.Field)
                continue;
            memNameList.Add(infos[index].Name);
            Logger.LogError("成员类型=" + infos[index].Name);
        }
    }

    public override string ToString()
    {
        string res = "";
        for (int index = 0; index < memNameList.Count; index++)
        {
            res += "," + memNameList[index] + "=" + this.getMemberValue(memNameList[index]).ToString();
        }
        return res;
    }
}
