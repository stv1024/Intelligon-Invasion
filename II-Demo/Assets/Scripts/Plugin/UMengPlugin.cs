using System.Runtime.InteropServices;
using Assets.Scripts;
using Com.Morln.Alking.Plugin;
using SimpleJson;
using UnityEngine;
using System;
using System.Collections.Generic;

public class UMengPlugin
{
    private const string TAG = "UMengPlugin";

#if UNITY_ANDROID

#elif UNITY_IPHONE
    
    [DllImport("__Internal")]
    private static extern void _umengStart(string appKey,string channel,string version);

    [DllImport("__Internal")]
    private static extern void _umengEvent(string eventId,string json);

    [DllImport("__Internal")]
    private static extern void _umengStartLevel(string level);

     [DllImport("__Internal")]
    private static extern void _umengFailLevel(string level);

    [DllImport("__Internal")]
    private static extern void _umengFinishLevel(string level);
     
    [DllImport("__Internal")]
    private static extern void _umengPay(double money, string item, int number, double price, int source);

    [DllImport("__Internal")]
    private static extern void _umengUse(string item, int number, double price);

    [DllImport("__Internal")]
    private static extern void _umengBonus(string item, int number, double price, int trigger);

    [DllImport("__Internal")]
    private static extern void _umengBuy(string item, int number, double price);
#endif




    /// <summary>
    /// 友盟开始，程序只能调用一次。
    /// 里面包含一些常用的操作。
    /// 比如自动更新，错误收集等。
    /// </summary>
    /// <param name="appKey"></param>
    /// <param name="channel"></param>
    public static void Start(string appKey, string channel, string version)
    {
        try
        {

#if UNITY_ANDROID

#elif UNITY_IPHONE
            _umengStart(appKey,channel,version);
#endif
        }
        catch (Exception exception)
        {
            Debug.LogError(exception.Message);
        }
    }

    public static void Event(string eventId, Dictionary<string, object> dic)
    {
        if (dic == null)
        {
            dic = new Dictionary<string, object>();
        }
        dic.Add("salechannel", ClientInfoHolder.Instance.SaleChannel);
        dic.Add("version", ClientInfoHolder.Instance.ClientVersion);

        string json = "{}";

        try
        {
            if (dic != null && dic.Count > 0)
            {
                JsonNode root = new JsonNode(NodeType.Object);

                foreach (KeyValuePair<string, object> pair in dic)
                {
                    root.AddSubNode(pair.Key, new JsonNode(pair.Value.ToString()));
                }
                json = root.ToJson();
            }

        }
        catch (Exception exception)
        {
            Debug.LogError(exception.Message);
        }
        try
        {

#if UNITY_ANDROID
            using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    AndroidJavaClass cls = new AndroidJavaClass("com.morln.game.plugin.umeng.UnityUmeng");
                    cls.CallStatic("onEvent", activity, eventId, json);
                }
            }
#elif UNITY_IPHONE
             _umengEvent(eventId,json); 
#endif
        }
        catch (Exception exception)
        {
            Debug.LogError(exception.Message);
        }
    }

    public static void StartLevel(string level)
    {
        try
        {
#if UNITY_ANDROID
            using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    AndroidJavaClass cls = new AndroidJavaClass("com.morln.game.plugin.umeng.UnityUmeng");
                    cls.CallStatic("startLevel", level);
                }
            }
#elif UNITY_IPHONE
           _umengStartLevel(level);  
#endif

        }
        catch (Exception exception)
        {

            XLog.LogError(TAG, exception.Message);
        }
    }

    public static void FailLevel(string level)
    {
        try
        {
#if UNITY_ANDROID
            using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    AndroidJavaClass cls = new AndroidJavaClass("com.morln.game.plugin.umeng.UnityUmeng");
                    cls.CallStatic("failLevel", level);
                }
            }
#elif UNITY_IPHONE
           _umengFailLevel(level);
#endif

        }
        catch (Exception exception)
        {

            XLog.LogError(TAG, exception.Message);
        }
    }

    public static void FinishLevel(string level)
    {
        try
        {
#if UNITY_ANDROID
            using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    AndroidJavaClass cls = new AndroidJavaClass("com.morln.game.plugin.umeng.UnityUmeng");
                    cls.CallStatic("finishLevel", level);
                }
            }
#elif UNITY_IPHONE
           _umengFinishLevel(level);
#endif
        }
        catch (Exception exception)
        {

            XLog.LogError(TAG, exception.Message);
        }
    }
    /// <summary>
    /// 真实消费统计
    /// </summary>
    /// <param name="money">本次消费金额(非负数) </param>
    /// <param name="item">item 购买物品的ID(不能为空) </param>
    /// <param name="number">number 购买物品数量(非负数) </param>
    /// <param name="price">price 每个物品等值虚拟币的价格(非负数) </param>
    /// <param name="source">
    /// 支付渠道
    /// 1：App Store
    /// 2:支付宝
    /// 3:网银
    /// 4:财付通
    /// 5:移动通信
    /// 6：联通通信
    /// 7：电信通信
    /// 8：
    /// </param>
    public static void Pay(double money, string item, int number, double price, int source)
    {
        try
        {
#if UNITY_ANDROID
            using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    AndroidJavaClass cls = new AndroidJavaClass("com.morln.game.plugin.umeng.UnityUmeng");
                    cls.CallStatic("pay", money, item, number, price, source);
                }
            }
#elif UNITY_IPHONE
           _umengPay(money, item, number, price, source);
#endif

        }
        catch (Exception exception)
        {

            XLog.LogError(TAG, exception.Message);
        }
    }
    /// <summary>
    /// 购买虚拟物品统计
    /// </summary>
    /// <param name="item">消耗的物品ID </param>
    /// <param name="number">消耗物品数量</param>
    /// <param name="price">物品单价（虚拟币）</param>
    public static void Buy(string item, int number, double price)
    {
        try
        {

#if UNITY_ANDROID
            using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    AndroidJavaClass cls = new AndroidJavaClass("com.morln.game.plugin.umeng.UnityUmeng");
                    cls.CallStatic("buy", item, number, price);
                }
            }
#elif UNITY_IPHONE
           _umengBuy(item,number,price);
#endif

        }
        catch (Exception exception)
        {

            XLog.LogError(TAG, exception.Message);
        }
    }
    /// <summary>
    /// 物品消耗统计
    /// </summary>
    /// <param name="item">消耗的物品ID </param>
    /// <param name="number">消耗物品数量</param>
    /// <param name="price">物品单价（虚拟币）</param>
    public static void Use(string item, int number, double price)
    {
        try
        {
#if UNITY_ANDROID
            using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    AndroidJavaClass cls = new AndroidJavaClass("com.morln.game.plugin.umeng.UnityUmeng");
                    cls.CallStatic("use", item, number, price);
                }
            }
#elif UNITY_IPHONE
           _umengUse(item,number,price);
#endif
        }
        catch (Exception exception)
        {

            XLog.LogError(TAG, exception.Message);
        }
    }
    /// <summary>
    /// 额外奖励
    /// </summary>
    /// <param name="item">奖励物品ID </param>
    /// <param name="number">奖励物品数量</param>
    /// <param name="price">物品的虚拟币单价 </param>
    /// <param name="trigger">
    /// 触发奖励的事件, 取值在 1~99 之间， 1~20 已经被预先定义， 21~99 需要在网站设置含义。
    /// 1: 玩家赠送
    /// 2：开发商赠送
    /// 3：游戏奖励
    /// </param>
    public static void Bonus(string item, int number, double price, int trigger)
    {
        try
        {
#if UNITY_ANDROID
            using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    AndroidJavaClass cls = new AndroidJavaClass("com.morln.game.plugin.umeng.UnityUmeng");
                    cls.CallStatic("bonus", item, number, price, trigger);
                }
            }
#elif UNITY_IPHONE
           _umengBonus(item,number,price,trigger);
#endif

        }
        catch (Exception exception)
        {

            XLog.LogError(TAG, exception.Message);
        }
    }


}
