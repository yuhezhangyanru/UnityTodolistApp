using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExportDBMain : MonoBehaviour {

    public InputField inputServer;
    public InputField inputDBName;
    public InputField inputUserName;
    public InputField inputPassword;
    public InputField inputPort;
    public InputField inputOutput;
    public InputField inputClassType;
    public Button btnStart;
    public Text txt_error;

    // Use this for initialization
    void Start () {
        inputServer.text = "localhost";
        inputUserName.text = "root";
        inputPassword.text = "";
        inputPort.text = "3306";
        inputPassword.text = "123456";
        inputOutput.text = @"D:\localhost_db\";
        inputClassType.text = ".cs";
        this.btnStart.onClick.AddListener(OnClickButton);
	}

    private void OnClickButton()
    {
        string errorCode = "";
        Logger.Log("此时的IP=" + inputServer.text);
        Logger.Log("DBname=" + inputDBName.text);
        UserConfig config = new UserConfig();
        config.DataBaseIP = inputServer.text;
        config.DataBaseName = inputDBName.text;
        config.DbUserID = inputUserName.text;
        config.DbPassword = inputPassword.text;
        config.DbPort = inputPort.text;
        SqlAccess.instance.UpdateDBClass(out errorCode, config,inputOutput.text,inputClassType.text);
        if (errorCode == "")
            errorCode = "生成成功！";
        setLog(errorCode);
    }

    private void setLog(string log)
    {
        txt_error.text = log;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
