using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Com.Morln.Alking.Plugin;
using SimpleJson;
using UnityEngine;

public class DownloadMgr : MonoBehaviour
{

    public const string TAG = "DownloadMgr";

#if UNITY_IPHONE

    [DllImport("__Internal")]
    private static extern void _httpGetString(string url);
    [DllImport("__Internal")]
    private static extern void _httpGetFile(string url,string path);
#endif

    private static readonly object locker = new object();

    private static DownloadMgr _instance;

    public static DownloadMgr Instance
    {
        get
        {
            return _instance;
        }
    }
    

    void Awake()
    {
        _instance = this;
    }


    public delegate void OnResult(DownloadResult result);

    private static List<DownloadResult> Results = new List<DownloadResult>();
 
    private static List<DownloadResult> CacheList = new List<DownloadResult>(); 
	
	// Update is called once per frame
	void Update () 
    {
	    if (Results.Count > 0)
	    {
	        DownloadResult result = Results[0];
	        result.Callback(result);
            Results.RemoveAt(0);
	    }
	}

    public void FinishDownload(string json)
    {
        lock (locker)
        {
            DownloadResult result = new DownloadResult();
            result.ParseFromJson(json);

            DownloadResult cached = null;
            foreach (DownloadResult item in CacheList)
            {
                if (item.Url.Equals(result.Url))
                {
                    cached = item;
                    break;
                }
            }
            if (cached != null)
            {
                result.Callback = cached.Callback;
                CacheList.Remove(cached);
                Results.Add(result);
            }       
        }
    }


    public void DownString(string url,OnResult callback)
    {
        try
        {
            lock (locker)
            {
                DownloadResult result = new DownloadResult();
                result.Url = url;
                result.Callback = callback;

                if (Application.platform == RuntimePlatform.Android)
                {

#if UNITY_ANDROID
                    CacheList.Add(result);
                    AndroidJavaClass cls = new AndroidJavaClass("com.morln.game.plugin.http.HttpUtils");
                    cls.CallStatic("httpGetString", url);
#endif
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    CacheList.Add(result);
                    Thread thread = new Thread(IosGetString);
                    thread.Start(result);
                }
                else
                {
                    Thread thread = new Thread(EditorGetString);
                    thread.Start(result);
                }
            }
            
        }
        catch (Exception exception)
        {
            
            XLog.LogError(TAG,exception.Message);
        }
       

    }

    public void DownloadFile(string url, string path,OnResult callback)
    {
        try
        {
            lock (locker)
            {
                DownloadResult result = new DownloadResult();
                result.Url = url;
                result.Path = path;
                result.Callback = callback;

                if (Application.platform == RuntimePlatform.Android)
                {
#if UNITY_ANDROID
                    CacheList.Add(result);
                    AndroidJavaClass cls = new AndroidJavaClass("com.morln.game.plugin.http.HttpUtils");
                    cls.CallStatic("httpGetFile", url, path);
#endif
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    CacheList.Add(result);
                    Thread thread = new Thread(IosGetFile);
                    thread.Start(result);
                }
                else
                {
                    Thread thread = new Thread(EditorGetFile);
                    thread.Start(result);
                }
            }
           
        }
        catch (Exception exception)
        {
            
            XLog.LogError(TAG,exception.Message);
        }
        
    }
    
    public class DownloadResult
    {
        public string Url { get; set; }
        /// <summary>
        /// 状态码：
        /// 200，请求正常
        /// -1，失败
        /// 除了200别的都是非正常
        /// </summary>
        public int StatusCode { get; set; }

        public string Content { get; set; }

        public string Path { get; set; }

        public OnResult Callback { get; set; }

        public void ParseFromJson(string json)
        {
            try
            {
                JsonNode root = JsonNode.FromJson(json);
                JsonNode urlNode = root["url"];
                if (urlNode != null)
                {
                    Url = urlNode;
                }
                JsonNode statusCodeNode = root["statusCode"];
                if (statusCodeNode != null)
                {
                    StatusCode = statusCodeNode;
                }

                JsonNode contentNode = root["content"];
                if (contentNode != null)
                {
                    Content = contentNode;
                }

                JsonNode pathNode = root["path"];
                if (pathNode != null)
                {
                    Path = pathNode;
                }

            }
            catch (Exception exception)
            {
                
               XLog.LogError(TAG,exception.Message);
            }
           

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("url : ").Append(Url)
                .Append(",statusCode : ").Append(StatusCode);
            if (Content != null)
            {
                sb.Append(",content : ").Append(Content);
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <returns>json:{"status"}</returns>
    private void EditorGetString(object obj)
    {
        DownloadResult result = obj as DownloadResult;
        try
        {

            string content = "";
            HttpWebRequest request = WebRequest.Create(result.Url) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            int statusCode = (int)response.StatusCode;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                XLog.Log(TAG, "请求成功");
                Stream stream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(stream, Encoding.GetEncoding("utf-8"));
                content = streamReader.ReadToEnd();
                streamReader.Close();
                response.Close();
            }
            result.StatusCode = statusCode;
            result.Content = content;
            Results.Add(result);

        }
        catch (Exception exception)
        {
            XLog.LogError(TAG, exception.Message);
            result.StatusCode = -1;
            Results.Add(result);
        }    
    }

    private void EditorGetFile(object obj)
    {

        DownloadResult result = obj as DownloadResult;
        try
        {
            HttpWebRequest request = WebRequest.Create(result.Url) as HttpWebRequest;
            request.Method = "GET";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            int statusCode = (int)response.StatusCode;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                XLog.Log(TAG, "请求成功");
                Stream stream = response.GetResponseStream();
                FileStream fs = File.Create(result.Path);
                byte[] tmp = new byte[1024];
                int size = 0;
                while ((size = stream.Read(tmp, 0, 1024)) > 0)
                {
                    fs.Write(tmp, 0, size);
                }
                stream.Close();
                fs.Close();
                XLog.Log(TAG, "写文件成功");
            }

            result.StatusCode = statusCode;
            Results.Add(result);
        }
        catch (Exception exception)
        {
            XLog.LogError(TAG, exception.Message);
            result.StatusCode = -1;
            Results.Add(result);
        }
        
    }

    private void IosGetString(object obj)
    {

#if UNITY_IPHONE

        
            DownloadResult result = obj as DownloadResult;
            string url = result.Url;
            _httpGetString(url);
        
#endif
    }

    private void IosGetFile(object obj)
    {
#if UNITY_IPHONE
       
            DownloadResult result = obj as DownloadResult;
            string url = result.Url;
            string path = result.Path;
            _httpGetFile(url,path);
        
#endif
    }
}
