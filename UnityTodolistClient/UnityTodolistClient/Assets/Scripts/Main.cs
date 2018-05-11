using System;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    void Start()
    { 
        //查询 
        var list = SqlAccess.instance.SelectList<table_task_list>("1=1");
        for (int index = 0; index < list.Count; index++)
        {
            Logger.Log("index=" + index + ",信息=" + list[index].ToString());
        }
 
        //插入
        table_task_list task = new table_task_list();
        task.id = SqlAccess.instance.getUUID();
        task.description = "hahah呵呵";
		task.createtime = "2012-01-01 12:00:00";
		task.actualFinishTime = "2012-1-1 12:00:00";
		task.endtime = "2012-1-1 1112:00:00";
        SqlAccess.instance.InsertEntity(task);
         
		Logger.Log ("时间 \"2012-01-01 12:00:00\"的长度=" + "2012-01-01 12:00:00".Length);

	    SqlAccess.instance.DeleteEntity<table_task_list>("description='hahah呵呵11'");//yanruTODO执行未报错，但是工作不正常

        //修改表信息！！！
		//test uuid()
		//UPDATE table_task_list SET endtime = '热情为',actualFinishTime = 'h哈哈哈' WHERE id = '120'
		//只需要传入最新的对象，内部接口根据变化决定要set哪些值
		table_task_list updateOne =new table_task_list();// task;
		updateOne.id="120";
		updateOne.description = "修改今天不睡觉了";
		updateOne.endtime = "fsjakfjd";
		updateOne.createtime = "呵呵呵====";
		SqlAccess.instance.UpdateTable<table_task_list>(updateOne);
    }
}
