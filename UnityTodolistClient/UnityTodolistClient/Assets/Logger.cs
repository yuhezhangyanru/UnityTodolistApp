using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Logger
{
    public static void Log(string log)
    {
        UnityEngine.Debug.Log("time=" + DateTime.Now.ToShortTimeString() + ",t=" + UnityEngine.Time.unscaledTime + ":" + log);
    }

    public static void LogError(string log)
    {
        UnityEngine.Debug.LogError("time=" + DateTime.Now.ToShortTimeString() + ",t=" + UnityEngine.Time.unscaledTime + ":" + log);
    }
}
