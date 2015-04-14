using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 截图分享专用管理器
/// </summary>
public class ShareManager : MonoBehaviour, WechatPlugin.ShareResultListener
{
    private static ShareManager _instance;
    void Awake()
    {
        _instance = this;
        WechatPlugin.AddShareResultListener(this);
    }
    public AudioClip ScreenShotAudio;
    public static void Share(string posStr)
    {
        LastActionIsShare = true;
        LastPos = posStr;
        _instance._Share();
    }

    public void _Share()
    {
        if (_isCapturingScreenShot) return; //防止一下截图好几次
        //DateTime now = DateTime.Now;
        //string filename = string.Format("FlappyBirdsOnline-{0}-{1}-{2}-{3}-{4}.png", now.Month,
        //        now.Day, now.Hour, now.Minute, now.Second);
        const string filename = "ScreenShot.png";
        string cptrAddr;
        string readAddr;
        switch (Application.platform)
        {
            case RuntimePlatform.IPhonePlayer:
                cptrAddr = filename;
                readAddr = Path.Combine(Application.persistentDataPath, filename);
                break;
            case RuntimePlatform.Android:
                cptrAddr = filename;
                readAddr = Path.Combine(Application.persistentDataPath, filename);
                break;
            default:
                cptrAddr = Path.Combine(Application.persistentDataPath, filename);
                readAddr = Path.Combine(Application.persistentDataPath, filename);
                break;
        }

        if (File.Exists(readAddr)) File.Delete(readAddr); //先删除旧图片

        Application.CaptureScreenshot(cptrAddr);
        Debug.Log("截图至cptAddr=" + cptrAddr + "; 读取路径readAddr=" + readAddr + " @" + Time.realtimeSinceStartup);

        //NGUITools.PlaySound(ScreenShotAudio);
        _isCapturingScreenShot = true;
        StartCoroutine(DelayToShare(readAddr));
    }

    private static bool _isCapturingScreenShot;

    private IEnumerator DelayToShare(string readAddr)
    {
        var delay = 0f;
        while (!File.Exists(readAddr) && delay < 10) //delay超过10就不要再等待了
        {
            yield return new WaitForEndOfFrame();
            delay += Time.deltaTime;
        }
        Debug.Log("Share to Social @" + Time.realtimeSinceStartup);
        _isCapturingScreenShot = false;
        
        if (Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.WindowsEditor)
        {
            //重命名，转存到桌面
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            File.Copy(readAddr,
                      Path.Combine(desktop,
                                   "screenshot " + DateTime.Now.ToString("yyyy-M-d h-mm") + ".png"), true);
            //Toast.CreateToast("截图已保存到桌面");
        }

        var title = "雷影忍者";
        var desc = "史上最佳动作游戏——雷影忍者";
        WechatPlugin.SharePic(readAddr, title, desc, "thumb.png", WechatPlugin.Scene.WxSceneTimeline);
    }

    public static void Invite(string posStr)
    {
        LastActionIsShare = false;
        LastPos = posStr;
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            //Toast.CreateErrorToast("电脑版不能使用微信分享。在手机上也安装载爱掼蛋吧！");
        }
        var url = "www.iguandan.com";
        var title = "雷影忍者";
        var desc = "史上最佳动作游戏——雷影忍者";
        WechatPlugin.ShareWebpage(url, title, desc, "thumb.png", WechatPlugin.Scene.WxSceneSession);
    }

    public static bool LastActionIsShare;
    public static string LastPos;

    public void Result(WechatPlugin.WxErrorCode errorCode, WechatPlugin.ShareInfo info)
    {
        var prefix = LastActionIsShare ? "wxshare" : "wxinvite";
        switch (errorCode)
        {
            case WechatPlugin.WxErrorCode.WxSuccess:
                UMengPlugin.Event(prefix + "_success",
                                  new Dictionary<string, object> {{"pos", LastPos}});
                break;
            default:
                UMengPlugin.Event(prefix + "_fail",
                                  new Dictionary<string, object> { { "pos", LastPos }, { "code", (int)errorCode } });
                break;
        }
    }
}