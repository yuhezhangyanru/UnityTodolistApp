using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Assets
{
    public class SqlAccess
    {
        private static SqlAccess _instance = null;
        public static SqlAccess instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SqlAccess();
                }
                return _instance;
            }
        }

        public static MySqlConnection mySqlConnection;//连接类对象  
        private static string database = "effectivetodolist_db";
        private static string host = "127.0.0.1";
        private static string id = "root";
        private static string pwd = "123456";
        private static string port = "3306";
         
        /// <summary>  
        /// 打开数据库  
        /// </summary>  
        public static void OpenSql()
        {
            try
            {
                var config = GlobalResource.instance.userConfig;
                string sqlString = string.Format("Database={0};Data Source={1};User Id={2};Password={3};",
                    config.DataBaseName, config.DataBaseIP, config.DbUserID, config.DbPassword, config.DbPort);
                Logger.Log("open sql,sqlstr=" + sqlString);
                mySqlConnection = new MySqlConnection(sqlString);
                mySqlConnection.Open();
            }
            catch (Exception e)
            {
                throw new Exception("服务器连接失败.....e="+e.Message);
            }
        }

        //yanruTODO测试函数
        public void Test()
        {
            Logger.Log("-----------------Mysql 连接--------------------------------");
         
            OpenSql();   
            Logger.Log("-----------------查询 操作--------------------------------"); 
            string querySQL = "select * from user";
            var mySQLAdapter = new MySqlDataAdapter(querySQL, mySqlConnection);
            DataTable mySQLDataTable = new DataTable();
            mySQLAdapter.Fill(mySQLDataTable);

            foreach (DataRow dr in mySQLDataTable.Rows)
            {
                for (int j = 0; j < dr.ItemArray.Length; j++)
                {
                    string str = dr[j].ToString();
                }
                Logger.Log("数据" + dr["id"]);
            }
            Logger.Log("条数：" + mySQLDataTable.Rows.Count);
         

           //-查询，利用 MySQLDataReader,依次读取每一条数据 
            var mySQLCommand = new MySqlCommand(querySQL, mySqlConnection);
            var mySQLReader = (MySqlDataReader)mySQLCommand.ExecuteReader();
            int i = 0;
            while (mySQLReader.Read())
            {
                i++;
                for (int j = 0; j < mySQLReader.FieldCount; j++)
                {
                    string str = mySQLReader[j].ToString();
                }
                Logger.Log("数据:" + mySQLReader["id"].ToString());
            }
            mySQLReader.Close();
             
            Logger.Log("-----------------操作结束--------------------------------");
            
            #region ---------------------------插入操作插入一条任务信息------------------------------------
            Logger.Log("---------------------直接插入 操作--------------------------");

             
            string x = "10001";
            string x1 = "1";//任务ID
            string x2 = "1";//userID
            string x3 = "晚上十点睡觉";//任务描述
            string startTime = "2012-12-9 10:00:00"; //任务开始时间
            string endTime = "2012-12-9 12:00:00"; //任务结束时间
			string finishTime = "2012-12-9 12:00:00"; //任务完成时间 //完成时间为空表示该任务未完成

            //TODO 暂时屏蔽，不然总是报错
            //string insertSQL = "INSERT INTO table_task_list VALUES('"+x+"','"+x1+"','"+x2+"','"+startTime+"','"+endTime+"','"+finishTime+"');";
            //var insertCommand = new MySqlCommand(insertSQL, mySqlConnection);
            //insertCommand.ExecuteNonQuery();
  
            Logger.Log("-----------------操作结束--------------------------------");


            #endregion
            Logger.Log("-----------------更新 操作--------------------------------");

			 
			//屏蔽更新那操作
			string updateSQL = "UPDATE table_task_list SET description='哈哈哈晚上十点睡觉' WHERE id = 1";
            var updateCmd = new MySqlCommand(updateSQL, mySqlConnection);
            updateCmd.ExecuteNonQuery();


            Logger.Log("-----------------操作结束--------------------------------");


            mySqlConnection.Close();
        }


        /// <summary>  
        /// 创建表  
        /// </summary>  
        /// <param name="name">表名</param>  
        /// <param name="colName">属性列</param>  
        /// <param name="colType">属性类型</param>  
        /// <returns></returns>  
        public DataSet CreateTable(string name, string[] colName, string[] colType)
        {
            if (colName.Length != colType.Length)
            {
                throw new Exception("输入不正确：" + "columns.Length != colType.Length");
            }
            string query = "CREATE TABLE  " + name + "(" + colName[0] + " " + colType[0];
            for (int i = 1; i < colName.Length; i++)
            {
                query += "," + colName[i] + " " + colType[i];
            }
            query += ")";
            return instance. QuerySet(query);
        }

        /// <summary>  
        /// 创建具有id自增的表  
        /// </summary>  
        /// <param name="name">表名</param>  
        /// <param name="col">属性列</param>  
        /// <param name="colType">属性列类型</param>  
        /// <returns></returns>  
        public DataSet CreateTableAutoID(string name, string[] col, string[] colType)
        {
            if (col.Length != colType.Length)
            {
                throw new Exception("columns.Length != colType.Length");
            }

            string query = "CREATE TABLE  " + name + " (" + col[0] + " " + colType[0] + " NOT NULL AUTO_INCREMENT";
            for (int i = 1; i < col.Length; ++i)
            {
                query += ", " + col[i] + " " + colType[i];
            }
            query += ", PRIMARY KEY (" + col[0] + ")" + ")";
            //    Debug.Log(query);  
            return QuerySet(query);
        }

        /// <summary>  
        /// 查询  
        /// </summary>  
        /// <param name="tableName">表名</param>  
        /// <param name="items">需要查询的列</param>  
        /// <param name="whereColName">查询的条件列</param>  
        /// <param name="operation">条件操作符</param>  
        /// <param name="value">条件的值</param>  
        /// <returns></returns>  
        public DataSet Select(string tableName, string[] items, string[] whereColName, string[] operation, string[] value)
        {
            if (whereColName.Length != operation.Length || operation.Length != value.Length)
            {
                throw new Exception("输入不正确：" + "col.Length != operation.Length != values.Length");
            }
            string query = "SELECT ";
            if (items != null && items.Length > 0)
            {
                query += items[0];
                for (int i = 1; i < items.Length; i++)
                {
                    query += "," + items[i];
                }
            }
            else
            {
                query += "* ";
            }
            query += "  FROM  " + tableName + "  WHERE " + " " + whereColName[0] + operation[0] + " '" + value[0] + "'";
            for (int i = 1; i < whereColName.Length; i++)
            {
                query += " AND " + whereColName[i] + operation[i] + "' " + value[i] + "';";
            }
            UnityEngine.Debug.Log("query=" + query);
            return QuerySet(query);
        }

        /// <summary>  
        /// 删除  
        /// </summary>  
        /// <param name="tableName">表名</param>  
        /// <param name="cols">条件：删除列</param>  
        /// <param name="colsvalues">删除该列属性值所在得行</param>  
        /// <returns></returns>  
        public DataSet Delete(string tableName, string[] cols, string[] colsvalues)
        {
            string query = "DELETE FROM " + tableName + " WHERE " + cols[0] + " = " + colsvalues[0];
            for (int i = 1; i < colsvalues.Length; ++i)
            {

                query += " or " + cols[i] + " = " + colsvalues[i];
            }
            //  Debug.Log(query);  
            return QuerySet(query);
        }

        /// <summary>  
        /// 更新  
        /// </summary>  
        /// <param name="tableName">表名</param>  
        /// <param name="cols">更新列</param>  
        /// <param name="colsvalues">更新的值</param>  
        /// <param name="selectkey">条件：列</param>  
        /// <param name="selectvalue">条件：值</param>  
        /// <returns></returns>  
        public DataSet UpdateInto(string tableName, string[] cols, string[] colsvalues, string selectkey, string selectvalue)
        {
            string query = "UPDATE " + tableName + " SET " + cols[0] + " = " + colsvalues[0];
            for (int i = 1; i < colsvalues.Length; ++i)
            {
                query += ", " + cols[i] + " =" + colsvalues[i];
            }
            query += " WHERE " + selectkey + " = " + selectvalue + " ";
            return QuerySet(query);
        }

        /// <summary>  
        /// 插入一条数据，包括所有，不适用自动累加ID。  
        /// </summary>  
        /// <param name="tableName">表名</param>  
        /// <param name="values">插入值</param>  
        /// <returns></returns>  
        public DataSet InsertInto(string tableName, string[] values)
        {
            string query = "INSERT INTO " + tableName + " VALUES (" + "'" + values[0] + "'";
            for (int i = 1; i < values.Length; ++i)
            {
                query += ", " + "'" + values[i] + "'";
            }
            query += ")";
            // Debug.Log(query);  
            return QuerySet(query);
        }


        /// <summary>  
        /// 插入部分  
        /// </summary>  
        /// <param name="tableName">表名</param>  
        /// <param name="col">属性列</param>  
        /// <param name="values">属性值</param>  
        /// <returns></returns>  
        public DataSet InsertInto(string tableName, string[] col, string[] values)
        {
            if (col.Length != values.Length)
            {
                throw new Exception("columns.Length != colType.Length");
            }
            string query = "INSERT INTO " + tableName + " (" + col[0];
            for (int i = 1; i < col.Length; ++i)
            {
                query += ", " + col[i];
            }

            query += ") VALUES (" + "'" + values[0] + "'";
            for (int i = 1; i < values.Length; ++i)
            {
                query += ", " + "'" + values[i] + "'";
            }
            query += ")";
            //   Debug.Log(query);  
            return instance. QuerySet(query);

        }
        /// <summary>  
        ///   
        /// 执行Sql语句  
        /// </summary>  
        /// <param name="sqlString">sql语句</param>  
        /// <returns></returns>  
        public DataSet QuerySet(string sqlString)
        {
            OpenSql();
            if (mySqlConnection.State == ConnectionState.Open)
            {
                DataSet ds = new DataSet();
                //try
                //{
                var mySqlDataAdapter = new MySqlDataAdapter(sqlString, mySqlConnection);
                mySqlDataAdapter.Fill(ds);

                    //MySqlCommand cmd = new MySqlCommand(sqlString, mySqlConnection);
                    // MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    ////cmd.ExecuteNonQuery();//.ExecuteReader();
                    // cmd.ExecuteReader();
                     
                return ds;
            }
            Close();
            return null;
        }

        public void Close()
        {
            if (mySqlConnection != null)
            {
                mySqlConnection.Close();
                mySqlConnection.Dispose();
                mySqlConnection = null;
            }
        }
    }
}
 