using System.Collections.Generic;
using System;
[Serializable]
public class user : table_data_base
{
        public user()
            : base()
        {
        }

		/*用户ID*/
        public override string id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/*用户昵称*/
        public string username
		{
			set{ _username=value;}
			get{return _username;}
		}
		/*电子邮箱*/
        public string email
		{
			set{ _email=value;}
			get{return _email;}
		}
		/*电话号码*/
        public string phonenumber
		{
			set{ _phonenumber=value;}
			get{return _phonenumber;}
		}
		/*用户创建时间*/
        public string createtime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/*上次登录时间*/
        public string lastlogintime
		{
			set{ _lastlogintime=value;}
			get{return _lastlogintime;}
		}

		private string _id;
		private string _username;
		private string _email;
		private string _phonenumber;
		private string _createtime;
		private string _lastlogintime;
}