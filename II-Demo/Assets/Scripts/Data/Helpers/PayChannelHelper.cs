using Assets.Scripts.PayStrategy;
using Scripts.PayChannels;

public static class PayChannelHelper
{
    public const int CHANNEL_WEB_ALIPAY = 1; //1: web alipay,
    public const int CHANNEL_ANDROID_ALIPAY = 2; //2: android alipay,
    public const int CHANNEL_IOS_IAP = 3; //3: IOS IAP
    public const int CHANNEL_WEIYUN_SMS = 4; //4: Weiyun SMS
    public const int CHANNEL_UNICOM_SMS = 5; //5: 联通短信
    public const int CHANNEL_IOS_ALIPAY = 6; //6: IOS alipay,
    public const int CHANNEL_TAOBAO = 7; //7: 淘宝网页
    public const int CHANNEL_YIDONG_SMS = 8; //8: 移动短信
    public const int CHANNEL_DIANXIN = 9; // 9: 电信短信
    public const int CHANNEL_BAORUAN = 10; // 10:宝软渠道
    public const int CHANNEL_QIANCHI = 11; // 11:千尺
    public const int CHANNEL_KUAIYONG = 12; // 12：快用

    public const int CHANNEL_PAYCENTER = 16; //安卓支付中心

    public const int CHANNEL_NIBIRU = 17; //nibiru盒子
    public const int CHANNEL_YIDONG_MM = 18; //移动MM


    /// <summary>
    /// 返回支付渠道的字符串值。
    /// </summary>
    /// <param name="payChannelID"></param>
    /// <returns></returns>
    public static string LabelOf(int payChannelID)
    {
        switch (payChannelID)
        {
            case CHANNEL_WEB_ALIPAY:
                return "Alipay_Web";
            case CHANNEL_ANDROID_ALIPAY:
                return "Alipay_Android";
            case CHANNEL_IOS_IAP:
                return "Ios_Iap";
            case CHANNEL_WEIYUN_SMS:
                return "Sms_WeiYun";
            case CHANNEL_UNICOM_SMS:
                return "Sms_Unicom";
            case CHANNEL_IOS_ALIPAY:
                return "Ios_Alipay";
            case CHANNEL_TAOBAO:
                return "Taobao";
            case CHANNEL_YIDONG_SMS:
                return "Sms_Yidong";
            case CHANNEL_DIANXIN:
                return "Sms_DianXin";
            case CHANNEL_BAORUAN:
                return "BaoRuan";
            case CHANNEL_QIANCHI:
                return "QianChi";
            case CHANNEL_KUAIYONG:
                return "KuaiYong";
            case CHANNEL_PAYCENTER:
                return "PayCenter";
            case CHANNEL_NIBIRU:
                return "Nibiru";
            case CHANNEL_YIDONG_MM:
                return "Yidong_MM";
            default:
                return "NULL";
        }
    }

    public static string GetChineseName(int payChannel)
    {
        switch (payChannel)
        {
            case CHANNEL_WEB_ALIPAY:
                return "网页支付宝";
            case CHANNEL_ANDROID_ALIPAY:
                return "支付宝";
            case CHANNEL_IOS_IAP:
                return "iOS支付";
            case CHANNEL_WEIYUN_SMS:
                return "Sms_WeiYun";
            case CHANNEL_UNICOM_SMS:
                return "Sms_Unicom";
            case CHANNEL_IOS_ALIPAY:
                return "支付宝";
            case CHANNEL_TAOBAO:
                return "淘宝";
            case CHANNEL_YIDONG_SMS:
                return "Sms_Yidong";
            case CHANNEL_DIANXIN:
                return "Sms_DianXin";
            case CHANNEL_BAORUAN:
                return "BaoRuan";
            case CHANNEL_QIANCHI:
                return "QianChi";
            case CHANNEL_KUAIYONG:
                return "KuaiYong";
            case CHANNEL_PAYCENTER:
                return "PayCenter";
            case CHANNEL_NIBIRU:
                return "Nibiru";
            case CHANNEL_YIDONG_MM:
                return "Yidong_MM";
            default:
                return "NULL";
        }
    }

    public static BasePayStrategy GetPayStrategy(int payChannelID)
    {
        switch (payChannelID)
        {
                //case CHANNEL_WEB_ALIPAY:
                //    return new OuterLinkPayStrategy();
            case CHANNEL_ANDROID_ALIPAY:
            case CHANNEL_IOS_ALIPAY:
                return new AlipayPayStrategy();
                //case CHANNEL_IOS_IAP:
                //    return new IOSIAPPayStrategy();
            default:
                return null;
        }
    }
}