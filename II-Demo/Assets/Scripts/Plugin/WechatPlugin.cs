using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Text;
using Com.Morln.Alking.Plugin;
using SimpleJson;
using UnityEngine;
/// <summary>
/// 微信分享。
/// 使用步骤，先注册(register)，然后再分享。
/// </summary>
public class WechatPlugin:MonoBehaviour
{
    private const string TAG = "WechatPlugin";

#if UNITY_IPHONE
    [DllImport("__Internal")]
    private static extern void _wxRegisterApp(string appId);

    [DllImport("__Internal")]
    private static extern void _wxSharePic(string path, string title, string desc, string thumb, int Scene);

    [DllImport("__Internal")]
    private static extern void _wxShareWebpage(string url, string title, string desc, string thumb, int Scene);

#elif UNITY_ANDROID

#endif

    /// <summary>
    /// 微信场景
    /// </summary>
    public enum Scene
    {
        /// <summary>
        /// 好友会话
        /// </summary>
        WxSceneSession = 0,
        /// <summary>
        /// 朋友圈
        /// </summary>
        WxSceneTimeline = 1
    }
    /// <summary>
    /// 微信错误码
    /// </summary>
    public enum WxErrorCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        WxSuccess = 0,

        /// <summary>
        /// 普通错误类型
        /// </summary>
        WxErrorCodeCommon = -1,
        /// <summary>
        /// 用户取消并返回
        /// </summary>
        WxErrorCodeUserCancel = -2,
        /// <summary>
        /// 发送失败
        /// </summary>
        WxErrorCodeSendFail = -3,

        /// <summary>
        /// 授权失败
        /// </summary>
        WxErorCodeAuthDeny = -4,

        /// <summary>
        /// 微信不支持
        /// </summary>
        WxErrorCodeUnsupport = -5

    }

    public enum ShareType
    {
        Unknown = 0,

        SharePic = 1,

        ShareWeb = 2

    }
   

    public class ShareInfo
    {

        public ShareType Type { get; set; }

        public Scene Scene { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(").Append(Type).Append(",").Append(Scene).Append(")");
            return sb.ToString();
        }
    }

    private static ShareInfo LastShareInfo { get; set; }

    public void WxShareResult(string json)
    {
        
        XLog.Log(TAG,"share result : " + json+",last share:" + (LastShareInfo==null?"":LastShareInfo.ToString()));

        try
        {
            JsonNode node = JsonNode.FromJson(json);
            JsonNode errorCodeNode = node["errorCode"];
            int code = errorCodeNode;
            WxErrorCode errorCode = (WxErrorCode) code;
            foreach (ShareResultListener listener in Listeners)
            {
                listener.Result(errorCode,LastShareInfo);
            }

        }
        catch (Exception exception)
        {
            
            XLog.LogError(TAG,exception.Message);
        }
    }
    private static readonly List<ShareResultListener> Listeners = new List<ShareResultListener>();

    public static void AddShareResultListener(ShareResultListener listener)
    {
        if (!Listeners.Contains(listener))
        {
            Listeners.Add(listener);
        }
    }

    public static void RemoveShareResultListener(ShareResultListener listener)
    {
        if (Listeners.Contains(listener))
        {
            Listeners.Remove(listener);
        }
    }
    /// <summary>
    /// 分享结果回调
    /// </summary>
    public interface ShareResultListener
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorCode">分享的错误码</param>
        /// <param name="info">分享信息，可以为null</param>
        void Result(WxErrorCode errorCode,ShareInfo info);

    }


    /// <summary>
    /// 微信注册app
    /// </summary>
    /// <param name="appId"></param>
    public static void RegisterApp(string appId)
    {
        Log("RegisterApp,appId = " + appId);
        
        try
        {
#if UNITY_ANDROID
            using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    AndroidJavaClass cls = new AndroidJavaClass("com.morln.game.plugin.wechat.UnityWechat");
                    cls.CallStatic("registerApp", activity,appId);
                }
            }
#elif UNITY_IPHONE
         _wxRegisterApp(appId);
#endif
        }
        catch (Exception exception)
        {
            Debug.LogError(exception.Message);
        }
        

    }
    /// <summary>
    /// 微信分享图片
    /// </summary>
    /// <param name="path">图片的路径</param>
    /// <param name="title">标题</param>
    /// <param name="desc">描述</param>
    /// <param name="thumb">缩略图在asset中的名称，如：thumb.png</param>
    /// <param name="scene">发送的场景</param>
    public static void SharePic(string path,string title,string desc,string thumb,Scene scene)
    {
        Log("SharePic,path =" + path);
        int sceneInt = (int) scene;

        LastShareInfo = new ShareInfo {Scene = scene,Type = ShareType.SharePic};
        try
        {
#if UNITY_ANDROID
            using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    AndroidJavaClass cls = new AndroidJavaClass("com.morln.game.plugin.wechat.UnityWechat");
                    cls.CallStatic("sharePic", activity, path, title, desc, thumb, sceneInt);
                }
            }
#elif UNITY_IPHONE
        _wxSharePic(path,title,desc,thumb,sceneInt);
#endif
        }
        catch (Exception exception)
        {
            Debug.LogError(exception.Message);
        }

    }
    /// <summary>
    /// 微信分享网页
    /// </summary>
    /// <param name="url">网址</param>
    /// <param name="title">标题</param>
    /// <param name="desc">描述</param>
    /// <param name="thumb">缩略图在asset中的名称，如:thumb.png</param>
    /// <param name="scene">发送的场景</param>
    public static void ShareWebpage(string url, string title, string desc, string thumb, Scene scene)
    {
        Log("ShareWebpage,url = " + url);
        int sceneInt = (int)scene;
        LastShareInfo = new ShareInfo { Scene = scene, Type = ShareType.ShareWeb }  ;
        try
        {
#if UNITY_ANDROID
            using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    AndroidJavaClass cls = new AndroidJavaClass("com.morln.game.plugin.wechat.UnityWechat");
                    cls.CallStatic("shareWebpage", activity, url, title, desc, thumb, sceneInt);
                }
            }
#elif UNITY_IPHONE
        _wxShareWebpage(url,title,desc,thumb,sceneInt);
#endif
        }
        catch (Exception exception)
        {
            Debug.LogError(exception.Message);
        }
    }

    public static void Log(string msg)
    {
        XLog.Log(TAG, msg);
    }
}
