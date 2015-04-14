using System.Runtime.InteropServices;
using Com.Morln.Alking.Plugin;
using UnityEngine;
using System;
using System.Collections.Generic;
using SimpleJson;

/// <summary>
/// 支付宝插件。
/// 如果是安卓平台，必须放置在AndroidReceiver物体上。
/// 如果是苹果平台，必须放置在iOSReceiver物体上。
/// </summary>
public class AlipayPlugin : MonoBehaviour
{
    private const string TAG = "AlipayPlugin";

#if UNITY_IPHONE
         
    [DllImport("__Internal")]        
    private static extern void _alipayInit(string partnerId, string sellerId, string rsaPrivateKey);

     
      [DllImport("__Internal")]
    private static extern void _alipayPay(string subject, string body, string totalFee, string outTradeNo, string notifyUrl);


#elif UNITY_ANDROID

#endif





    private static readonly List<AlipayListener> Listeners = new List<AlipayListener>();

    public static void AddListener(AlipayListener listener)
    {
        if (!Listeners.Contains(listener))
        {
            Listeners.Add(listener);
        }
    }

    public static void RemoveListener(AlipayListener listener)
    {
        if (Listeners.Contains(listener))
        {
            Listeners.Remove(listener);
        }
    }



    /// <summary>
    /// 初始化
    /// </summary>
    public static void Init(string partnerId, string sellerId, string rsaPrivateKey)
    {
        XLog.Log(TAG, "alipay init");
        try
        {
#if UNITY_IPHONE
        _alipayInit(partnerId,sellerId,rsaPrivateKey);
#elif UNITY_ANDROID
            using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    AndroidJavaClass cls = new AndroidJavaClass("com.morln.game.plugin.alipay.UnityMorlnAlipay");
                    cls.CallStatic("init", activity, partnerId, sellerId, rsaPrivateKey);
                }

            }


#endif
        }
        catch (Exception exception)
        {
            XLog.LogError(TAG, exception.Message);

        }


    }
    /// <summary>
    /// 支付，可以使安卓支付，也可以是iOS支付宝支付
    /// </summary>
    /// <param name="trade"></param>
    public static void Pay(string subject, string body, string totalFee, string outTradeNo, string notifyUrl)
    {
        XLog.Log(TAG, "alipay pay");
        try
        {
#if UNITY_IPHONE
         _alipayPay(subject,body,totalFee,outTradeNo,notifyUrl);
#elif UNITY_ANDROID

            using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    AndroidJavaClass cls = new AndroidJavaClass("com.morln.game.plugin.alipay.UnityMorlnAlipay");
                    cls.CallStatic("pay", subject, body, totalFee, outTradeNo, notifyUrl);
                }

            }
#endif
        }
        catch (Exception exception)
        {

            XLog.LogError(TAG, exception.Message);
        }
    }

    /// <summary>
    /// 支付成功，由android 或者 iOS 底层调用
    /// </summary>
    /// <param name="json"></param>
    public void AlipayPaySuccess(string json)
    {
        XLog.Log(TAG, "alipay success:" + json);
        Trade trade = new Trade();
        try
        {
            trade.FromJson(json);

            foreach (AlipayListener listener in Listeners)
            {
                listener.PaySuccess(trade);
            }
        }
        catch (Exception exception)
        {
            XLog.LogError(TAG, exception.Message);

        }
    }


    public void AlipayPayCanceled(string json)
    {
        XLog.Log(TAG, "alipay canceled:" + json);
        Trade trade = new Trade();
        try
        {
            trade.FromJson(json);

            foreach (AlipayListener listener in Listeners)
            {
                listener.PayCanceled(trade);
            }
        }
        catch (Exception exception)
        {
            XLog.LogError(TAG, exception.Message);

        }
    }

    public void AlipayPayFailed(string json)
    {
        XLog.Log(TAG, "alipay failed:" + json);
        Trade trade = new Trade();
        try
        {
            trade.FromJson(json);

            foreach (AlipayListener listener in Listeners)
            {
                listener.PayFailed(trade);
            }
        }
        catch (Exception exception)
        {
            XLog.LogError(TAG, exception.Message);
        }
    }

    public interface AlipayListener
    {
        void PaySuccess(Trade trade);

        void PayCanceled(Trade trade);

        void PayFailed(Trade trade);
    }


    /// <summary>
    /// 订单
    /// </summary>
    public class Trade
    {

        public string Subject { get; set; }

        public string Body { get; set; }

        public string TotalFee { get; set; }

        public string OutTradeNo { get; set; }

        public string NotifyUrl { get; set; }

        public Trade()
        {
            Subject = "";
            Body = "";
            TotalFee = "";
            OutTradeNo = "";
            NotifyUrl = "";
        }

        public void FromJson(string json)
        {

            try
            {
                JsonNode root = JsonNode.FromJson(json);

                JsonNode subjectNode = root["subject"];
                if (subjectNode != null)
                {
                    Subject = subjectNode;
                }

                JsonNode bodyNode = root["body"];
                if (bodyNode != null)
                {
                    Body = bodyNode;
                }

                JsonNode totalFeeNode = root["totalFee"];
                if (totalFeeNode != null)
                {
                    TotalFee = totalFeeNode;
                }

                JsonNode outTradeNoNode = root["outTradeNo"];
                if (outTradeNoNode != null)
                {
                    OutTradeNo = outTradeNoNode;
                }

                JsonNode notifyUrlNode = root["notifyUrl"];
                if (notifyUrlNode != null)
                {
                    NotifyUrl = notifyUrlNode;
                }

            }
            catch (Exception exception)
            {

                Debug.LogError(exception.Message);
            }
        }

        public string ToJson()
        {
            JsonNode root = new JsonNode(NodeType.Object);
            root.AddSubNode("subject", new JsonNode(Subject));
            root.AddSubNode("body", new JsonNode(Body));
            root.AddSubNode("totalFee", new JsonNode(TotalFee));
            root.AddSubNode("outTradeNo", new JsonNode(OutTradeNo));
            root.AddSubNode("notifyUrl", new JsonNode(NotifyUrl));
            return root.ToJson();
        }


    }

}
