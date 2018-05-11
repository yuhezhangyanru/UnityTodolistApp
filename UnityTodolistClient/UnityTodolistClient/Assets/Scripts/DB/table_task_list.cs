using System.Collections.Generic;
using System;
[Serializable]
public class table_task_list : table_data_base
{
        public table_task_list()
            : base()
        {
        }

		/*任务ID唯一，*/
        public override string id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/*任务所属用户ID*/
        public string userid
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/*任务内容描述*/
        public string description
		{
			set{ _description=value;}
			get{return _description;}
		}
		/*创建时间*/
        public string createtime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/*要求完成的到期时间*/
        public string endtime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		/*实际完成时间*/
        public string actualFinishTime
		{
			set{ _actualFinishTime=value;}
			get{return _actualFinishTime;}
		}

		private string _id;
		private string _userid;
		private string _description;
		private string _createtime;
		private string _endtime;
		private string _actualFinishTime;
}