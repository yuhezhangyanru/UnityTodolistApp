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
	//固定的作为ID啊
	public virtual string id 
	{
		set{ } 
		get{ return "";}
	}
    /// <summary>
    /// 需要重写构造！
    /// </summary>
    public table_data_base()
    {
        MemberInfo[] infos = this.GetType().GetMembers();
        memNameList = new List<string>();
        for (int index = 0; index < infos.Length; index++)
		{
//			Logger.LogError("classType=" + this.GetType().Name + ",成员类型=" + infos[index].Name + ",notField?"
//				+ (infos[index].MemberType != MemberTypes.Field)+",notproperty?"
//				+(infos[index].MemberType!= MemberTypes.Property));
            if ( infos[index].DeclaringType != this.GetType()) //只有子类定义的才看
                continue;
            if (infos[index].MemberType != MemberTypes.Property) //_id 属于属性范畴，
                continue;

            memNameList.Add(infos[index].Name);
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
