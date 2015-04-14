using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.PayStrategy;
using Scripts.PayChannels;
using UnityEngine;

public class TradeManager : MonoBehaviour, AlipayPlugin.AlipayListener
{
    static TradeManager instance;
    public static TradeManager Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            if (instance && value)
            {
                Debug.LogError("more than 1 TradeManager instance now!");
                Destroy(value.gameObject);
            }
            else if (value == null)
            {
                Debug.LogError("should not delete TradeManager!");
            }
            else
            {
                instance = value;
            }
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        //InvokeRepeating("CheckPulse", 30f, 30f);

        AlipayPlugin.AddListener(this);
    }

    private static bool _hasInittedAlipay;
    /// <summary>
    /// 在收到VersionResult之后调用，会检测是否只有一次
    /// </summary>
    public static void CheckAndInitAlipay()
    {
        var privateKey = AlipayUtil.Restore(ClientInfoHolder.Instance.AliPkey);
        AlipayPlugin.Init(ClientInfoHolder.Instance.AliPid, ClientInfoHolder.Instance.AliSid, privateKey);
        _hasInittedAlipay = true;
    }

    public void PaySuccess(AlipayPlugin.Trade trade)
    {
        UserSelectedTrade selectTrade;
        if (AlipayPayStrategy.PayTrades.ContainsKey(trade.OutTradeNo))
        {
            selectTrade = AlipayPayStrategy.PayTrades[trade.OutTradeNo];
            AlipayPayStrategy.PayTrades.Remove(trade.OutTradeNo);
        }
        else
        {
            selectTrade = null;
        }
        if (selectTrade != null)
        {
        }
        else
        {
            //估计是上一次充值后程序挂了，那我也不知道玩家的目的，就只能折算成金币了。
        }
        int paychannel;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                paychannel = PayChannelHelper.CHANNEL_ANDROID_ALIPAY;
                break;
            case RuntimePlatform.IPhonePlayer:
                paychannel = PayChannelHelper.CHANNEL_IOS_ALIPAY;
                break;
            default:
                paychannel = PayChannelHelper.CHANNEL_WEB_ALIPAY;
                break;
        }
        float price;
        float.TryParse(trade.TotalFee, out price);
        UMengPlugin.Event("alipay_pay_success",
                          new Dictionary<string, object>
                              {
                                  //{"name", selectTrade != null ? selectTrade.Name : "(noname)"},
                                  //{"paychannel", paychannel},
                                  {"price", price},
                              });
        UMengPlugin.Pay(price, "revive", 1, price, 2);//如果不是支付宝，记得修改
    }

    public void PayCanceled(AlipayPlugin.Trade trade)
    {
        //MessageBoxPanel.Load("您取消了支付，如有问题请联系客服QQ:" + CommonData.QQ);
        UserSelectedTrade selectTrade;
        if (AlipayPayStrategy.PayTrades.ContainsKey(trade.OutTradeNo))
        {
            selectTrade = AlipayPayStrategy.PayTrades[trade.OutTradeNo];
            AlipayPayStrategy.PayTrades.Remove(trade.OutTradeNo);
        }
        else
        {
            selectTrade = null;
        }
        int paychannel;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                paychannel = PayChannelHelper.CHANNEL_ANDROID_ALIPAY;
                break;
            case RuntimePlatform.IPhonePlayer:
                paychannel = PayChannelHelper.CHANNEL_IOS_ALIPAY;
                break;
            default:
                paychannel = PayChannelHelper.CHANNEL_WEB_ALIPAY;
                break;
        }

        float price;
        float.TryParse(trade.TotalFee, out price);
        UMengPlugin.Event("alipay_pay_cancel",
                          new Dictionary<string, object>
                              {
                                  //{"name", selectTrade != null ? selectTrade.Name : "(noname)"},
                                  //{"paychannel", paychannel},
                                  {"price", price},
                              });
    }

    public void PayFailed(AlipayPlugin.Trade trade)
    {
        //AlertDialog.Load("支付失败，如有问题请联系客服QQ:" + CommonData.QQ);//这个展示时间更长，不容易关闭
        UserSelectedTrade selectTrade;
        if (AlipayPayStrategy.PayTrades.ContainsKey(trade.OutTradeNo))
        {
            selectTrade = AlipayPayStrategy.PayTrades[trade.OutTradeNo];
            AlipayPayStrategy.PayTrades.Remove(trade.OutTradeNo);
        }
        else
        {
            selectTrade = null;
        }
        int paychannel;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                paychannel = PayChannelHelper.CHANNEL_ANDROID_ALIPAY;
                break;
            case RuntimePlatform.IPhonePlayer:
                paychannel = PayChannelHelper.CHANNEL_IOS_ALIPAY;
                break;
            default:
                paychannel = PayChannelHelper.CHANNEL_WEB_ALIPAY;
                break;
        }

        float price;
        float.TryParse(trade.TotalFee, out price);
        UMengPlugin.Event("alipay_pay_fail",
                              new Dictionary<string, object>
                                  {
                                      //{"name", selectTrade != null ? selectTrade.Name : "(noname)"},
                                      //{"paychannel", paychannel},
                                  {"price", price},
                              });
    }
}