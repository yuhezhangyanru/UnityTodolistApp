using UnityEngine;
using Assets; 
using UnityEngine.UI;
using Maticsoft.Model;

public class Main : MonoBehaviour
{
    public Text text;

    void Start()
    {
       
        Logger.Log("userInfo.db=" + GlobalResource.instance.userConfig.DbPassword);


         SqlAccess.instance.Test();

      //  SqlAccess.instance.QuerySet("select * from user");

        //查询
        // public DataSet Select(string tableName, string[] items, string[] whereColName, string[] operation, string[] value)
         var dataSet = SqlAccess.instance.Select("table_task_list", null, new string[] { "id" }, new string[] { "=" }, new string[] { "1" });
         Logger.Log("ds=" + dataSet);
         Logger.Log("ds.cont=" + dataSet.Tables.Count);

             var tableList = ModelTool.getListfromDataSet<table_task_list>(dataSet);//.Tables[0].Rows[index]);
             Logger.Log("tableList.Count=" + tableList.Count);//yanruTODO结果列表长度为0了，这是错误的！
             for (int index = 0; index < tableList.Count; index++)
             {
                 table_task_list item = tableList[index] as table_task_list;
                 Logger.Log("查询结果table1.str=" + tableList[index].ToString());
             }
    }
}
