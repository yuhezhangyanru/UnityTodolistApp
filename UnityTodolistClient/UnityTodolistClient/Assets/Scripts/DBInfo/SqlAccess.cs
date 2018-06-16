using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using UnityEngine;

/// <summary>
/// function:数据库基础操作工具类
/// date:2018-5-11 23:44:34
/// </summary>
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

    private UserConfig _config { get { return GlobalResource.instance.userConfig; } }
    /// <summary>  
    /// 打开数据库  
    /// </summary>  
    public static string OpenSql(string DBServerIP, string DBName, string userName, string password, string port)
    {
        try
        {
            string sqlString = string.Format("Database={0};Data Source={1};User Id={2};Password={3};",
                       DBName, DBServerIP, userName, password, port);
            Logger.Log("open sql,sqlstr=" + sqlString);
            mySqlConnection = new MySqlConnection(sqlString);
            mySqlConnection.Open();
            return "";
        }
        catch (Exception e)
        {
            Debug.LogError("服务器连接失败.....e=" + e.Message);
            return "服务器连接失败.....e=" + e.Message;
        }
        return "";
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
        return instance.QuerySet(query,_config);
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
        return QuerySet(query,_config);
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
        return QuerySet(query,_config);
    }
 
    /// <summary>
    /// 删除某张表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table"></param>
    /// <param name="whereSql"></param>
    public void DeleteEntity<T>(string whereSql) where T : table_data_base
    {
        QuerySet("delete from "+typeof(T).Name+" where " + whereSql,_config);
    }
 
    /// <summary>  
    /// 插入一条数据，包括所有，不适用自动累加ID。  
    /// </summary>  
    /// <param name="tableName">表名</param>  
    /// <param name="values">插入值</param>  
    /// <returns></returns>  
    private DataSet InsertInto(string tableName, string[] values)
    {
        string query = "INSERT INTO " + tableName + " VALUES (" + "'" + values[0] + "'";
        for (int i = 1; i < values.Length; ++i)
        {
            query += ", " + "'" + values[i] + "'";
        }
        query += ")";
        return QuerySet(query, _config);
    }
  
    /// <summary>
    /// 将sql查询结果映射到实体对象列表,查询全部字段
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sqlWhere">只填充where中的表达式条件本身！</param>
    /// <returns></returns>
    public List<T> SelectList<T>(string sqlWhere) where T : table_data_base
    {
        DataSet dataSet = QuerySet("select * from " + typeof(T).Name + " where " + sqlWhere, _config);
        List<T> dataList = new List<T>();
        for (int index = 0; index < dataSet.Tables[0].Rows.Count; index++)
        {
            T child = Activator.CreateInstance<T>();// new table_data_base() as T;
            for (int childIndex = 0; childIndex < child.memNameList.Count; childIndex++)
            {
                string memberName = child.memNameList[childIndex];
                child.setMemberValue(memberName, dataSet.Tables[0].Rows[index][memberName]);
            }
            dataList.Add(child);
        }
        return dataList;
    }

    /// <summary>
    /// 插入一条数据库记录！//OK
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table"></param>
    public void InsertEntity<T>(T table) where T : table_data_base
    {
        OpenSql(_config.DataBaseIP,_config.DataBaseName,_config.DbUserID,_config.DbPassword,_config.DbPort);

        string[] values = new string[table.memNameList.Count];
        for (int index = 0; index < table.memNameList.Count; index++)
        {
            values[index] = table.getMemberValue(table.memNameList[index]) + "";
        }
        InsertInto(typeof(T).Name, values.ToArray());

        Close();
    }

	/// <summary>
	/// 更新表结构,返回true表示更新成功！
	/// </summary>
	/// <param name="table">Table.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public bool UpdateTable<T>(table_data_base table)  where T:table_data_base
	{ 
		OpenSql  (_config.DataBaseIP, _config.DataBaseName, _config.DbUserID, _config.DbPassword, _config.DbPort);
        var selectOld = SelectList<T> ("id='" + table.id + "'");
		if (selectOld == null || selectOld.Count == 0)
			return false;
		string setValStr = "";
		var oldItem = selectOld [0];
		for (int index = 0; index < table.memNameList.Count; index++) {
			string memberName = table.memNameList [index];
			if (oldItem.getMemberValue (memberName) != table.getMemberValue (memberName)) {
				string valStr = "'" +table.getMemberValue(memberName)+ "'";
				if (table.getMemberType (memberName).Name == "int") {
					valStr = table.getMemberValue (memberName) + "";
				} 
				setValStr += memberName + "="+ valStr+",";
			}
		}
		setValStr = setValStr.TrimEnd (','); 
		string newSql = "update "+table.GetType().Name+" set "+setValStr +" where id='"+table.id+"'";
		QuerySet (newSql,_config); 
		Close ();
		return true;
	}

    /// <summary>
    /// 生成唯一ID
    /// </summary>
    /// <returns></returns>
    public string getUUID()
    {
        var ds = QuerySet("select uuid()",_config);
        Logger.Log("UUID=" + ds.Tables[0].Rows[0]["uuid()"]+"");
        return ds.Tables[0].Rows[0]["uuid()"] + "";
    }

    /// <summary>  
    ///   
    /// 执行Sql语句   //test OK
    /// </summary>  
    /// <param name="sqlString">sql语句</param>  
    /// <returns></returns>  
    private DataSet QuerySet(string sqlString,UserConfig config)
    {
        if (config == null)
            config = _config;
        OpenSql(config.DataBaseIP, config.DataBaseName, config.DbUserID, config.DbPassword, config.DbPort);
        Logger.Log("conState=" + mySqlConnection.State + ",sql=" + sqlString);
        if (mySqlConnection.State == ConnectionState.Open)
        {
            DataSet ds = new DataSet();
            var mySqlDataAdapter = new MySqlDataAdapter(sqlString, mySqlConnection);
            mySqlDataAdapter.Fill(ds);
 
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


	/// <summary>
	/// 在sql语句中根据字段的类型决定字段在qsl中是否要单引号
	/// </summary>
	/// <returns>The filed format.</returns>
	/// <param name="fieldName">Field name.</param>
	/// <param name="fieldType">Field type.</param>
	private string getFiledFormat(string fieldName,string fieldType)
	{
		if (fieldType == "int") {
			return fieldName + "";	
	}
		return "'" + fieldName + "'";
	}

    /// <summary>
    /// ===============================================================
    /// 菜单：以下是生成表结构工具类的处理
    /// </summary> 
    /// 
	public void UpdateDBClass(out string errorTip,UserConfig config,string output ,string fileTail)
    {
        if (config == null)
            config = _config;
        errorTip = OpenSql(config.DataBaseIP, config.DataBaseName, config.DbUserID, config.DbPassword, config.DbPort);
        Logger.Log("此时的错误码=" + errorTip);
        if (errorTip != "")
            return;

        var dataSet = QuerySet("show tables", config);
        var dataSetRes1 = dataSet.Tables[0].Rows;
        bool isTypeJava = fileTail == ".java";
        bool isTypeCS = fileTail == ".cs";
        for (int index = 0; index < dataSetRes1.Count; index++)
        {
            string tableName = dataSetRes1[index][0].ToString();
            //Logger.LogError("index=" + index + ",tableName=" + tableName);
			string fileContent = "";
            if (isTypeJava)
            {
                fileContent += "public class " + tableName + "\n{\n";
            }
            else
            {
                fileContent += "using System.Collections.Generic;\nusing System;\n";
                fileContent += "[Serializable]\n";
                fileContent += "public class " + tableName + " : table_data_base";
                fileContent += "\n{";
                fileContent += "\n        public " + tableName + "()\n            : base()\n        {\n        }\n"; //添加构造！！
            }

            string fileHead = output != "" ? output : (Application.dataPath + @"\Scripts\DB\");
            string fileName = fileHead + tableName;
            if (fileTail != "")
            {
                fileName += fileTail;
            }
            else
            {
                fileName += ".cs";
            }

            string names1 = "\n\n";
            string names2 = "";

            //2018-5-11 修改获取字段信息
            var tableInfo = QuerySet("SHOW FULL FIELDS FROM " + tableName + "", config);
            for (int infoIndex = 0; infoIndex < tableInfo.Tables[0].Rows.Count; infoIndex++)
            {
                var childInfo = tableInfo.Tables[0].Rows[infoIndex];
                string useColume = childInfo["Field"] + "";
                string useType = childInfo["Type"] + "";
                string comment = childInfo["Comment"] + "";
                bool isKey = childInfo["Key"] + "" == "PRI";
                string displayType = "";
                if (isTypeJava)
                {
                    if (useType.Contains("char("))
                    {
                        displayType = "String";
                    }
                    else if (useType.Contains("int("))
                    {
                        displayType = "int";
                    }
                    else
                    {
                        Logger.LogError("表=" + tableName + ",字段=" + useColume + ",类型=" + useType + "未处理！");
                    }
                    names2 += "/*" + comment + "*/\n";
                    names2 += "        public " + displayType + " " + useColume + ";\n\t\t";
                }
                else
                {
                    if (useType.Contains("char("))
                    {
                        displayType = "string";
                    }
                    else if (useType.Contains("int("))
                    {
                        displayType = "int";
                    }
                    else
                    {
                        Logger.LogError("表=" + tableName + ",字段=" + useColume + ",类型=" + useType + "未处理！");
                    }
                    names1 += "\t\tprivate " + displayType + " _" + useColume + ";\n";
                    names2 += "\n\t\t";
                    names2 += "/*" + comment + "*/\n";
                    //	Logger.Log ("字段=" + useColume + ",类型=" + useType + ",key=" + childInfo ["Key"] + "，是主键?" + isKey);
                    string itemHead = "";
                    if (isKey)
                    {
                        if (useColume != "id")
                        {
                            Logger.LogError("表=" + tableName + ",的主键字段要求必须命名为'id'，这样才能方便统一管理！");
                        }
                        itemHead = "override ";
                    }
                    else
                    {
                    }
                    names2 += "        public " + itemHead + displayType + " " + useColume + "\n\t\t{\n\t\t\tset{ _" + useColume
                        + "=value;}\n\t\t\tget{return _" + useColume + ";}\n\t\t}";
                }
            }

			fileContent += names2;
			fileContent += names1;
			fileContent+= "}";
			Logger.Log ("生成表=" + tableName + "写到文件=" + fileName);
			if (!Directory.Exists (fileHead)) {
				Directory.CreateDirectory (fileHead);
			}
			File.WriteAllText (fileName, fileContent);
        }

        Close();
    }

	private List<string> getTableColumNames(string tableName,DataRowCollection dataSetResTable )
	{
		List<string> usefulColumNameList= new List<string>();

        //根据第一行数据决定有效的字段名
        var dataSetUseful = QuerySet("SELECT * FROM " + tableName + " LIMIT 1", _config);
		var dataSetUsefulRes = dataSetUseful.Tables[0].Rows[0].ItemArray;

		for(int subIndex=0;subIndex<dataSetUsefulRes.Length;subIndex ++)
		{ 
			Logger.Log ("有效列名=" + dataSetResTable [subIndex][0]+"");//childList[childIndex]);
			usefulColumNameList.Add(dataSetResTable [subIndex][0]+"");
		}
		return usefulColumNameList;
	}
}