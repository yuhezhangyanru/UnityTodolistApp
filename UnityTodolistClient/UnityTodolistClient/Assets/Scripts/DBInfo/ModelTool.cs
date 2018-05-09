

using System.Collections.Generic;
/// <summary>
/// function:操作数据库对象的工具类
/// </summary>
using System.Data;
public class ModelTool
{

    /// <summary>
    /// 从DataSet生成数据表实体对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dataSet"></param>
    /// <returns></returns>
    public static List<table_data_base> getListfromDataSet<T>(DataSet dataSet) where T : table_data_base
    {
        var list = new List<table_data_base>();// as List<T>;// new table_data_base() as T;
        for (int index = 0; index < dataSet.Tables[0].Rows.Count; index++)
        {
            var tableItem =new table_data_base() as T;
            if (tableItem.memNameList.Count == 0)
            {
                Logger.LogError("类型=" + list.GetType().Name + "的构造函数错误！必须继承自 table_data_base");
                return null;
            }
            for (int subIndex = 0; subIndex < tableItem.memNameList.Count; subIndex++)
            {
                tableItem.setMemberValue(tableItem.memNameList[subIndex], dataSet.Tables[0].Rows[index][tableItem.memNameList[subIndex]]);
            }
            list.Add(tableItem);
        }
        return list;
    }
}