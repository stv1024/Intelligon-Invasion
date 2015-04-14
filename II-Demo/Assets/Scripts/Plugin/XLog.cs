using UnityEngine;

namespace Com.Morln.Alking.Plugin
{

    /// <summary>
    /// 对unitylog进行封装，添加了一个tag。
    /// </summary>
    public class XLog
    {

        private static bool logOn = true;

        private static bool logErrorOn = true;

        private static bool logWarningOn = true;

        public static void SetLogOn(bool v)
        {
            logOn = v;
        }

        public static void SetLogErrorOn(bool v)
        {
            logErrorOn = v;
        }

        public static void SetLogWarningOn(bool v)
        {
            logWarningOn = v;
        }

        public static void Log(string tag, string msg)
        {
            if (logOn)
            {
                string log = tag + " : " + msg;
                Debug.Log(log);
            }

        }

        public static void LogError(string tag, string msg)
        {
            if (logErrorOn)
            {
                string log = tag + " : " + msg;
                Debug.LogError(log);
            }
        }


        public static void LogWarning(string tag, string msg)
        {
            if (logWarningOn)
            {
                string log = tag + " : " + msg;
                Debug.LogWarning(log);
            }

        }

    }

}

