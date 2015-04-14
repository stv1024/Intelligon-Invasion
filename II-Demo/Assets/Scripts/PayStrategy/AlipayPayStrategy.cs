using System;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.PayStrategy;
using SimpleJson;
using UnityEngine;

namespace Scripts.PayChannels
{
    public class AlipayPayStrategy : BasePayStrategy
    {
        public override void Pay(UserSelectedTrade trade)
        {
            Debug.Log("开始支付宝支付。price:" + trade.Price);
            //var refill = CommonData.RefillList.Find(x => x.Name == re.Trade.PrepaymentName); //通过支付包名称找到支付包
            //if (refill == null)
            //{
            //    Toast.CreateErrorToast("无法找到对应的充值包,name:" + re.Trade.PrepaymentName);
            //    return;
            //}

            //if (!re.HasExtra || string.IsNullOrEmpty(re.Extra))
            //{
            //    LoadingMask.EndLoading();
            //    Toast.CreateErrorToast("解析订单数据失败！");
            //    Debug.LogError("没有找到extra数据。");
            //    return;
            //}


            try
            {
                // Android支付宝的参数为
                // {"notifyUrl":"支付宝插件中用到的NotifyUrl"};
                // 不再使用PayChannel.channel_url
                //var json = re.Extra;

                //var rootNode = JsonNode.FromJson(json);

                //const string key = "notifyUrl";
                //var value = "";
                //if (rootNode.SubNodeNames.Contains(key))
                //{
                //    value = rootNode[key].Value.ToString();
                //}
                //else
                //{
                //    Debug.LogWarning("没有找到" + key + "的内容。");
                //}

                var subject = trade.DisplayName;
                var body = trade.DisplayName;
                var price = trade.Price.ToString("0.00");
                var outTradeNum = Guid.NewGuid().ToString();
                var notifyUrl = ClientInfoHolder.Instance.AliNotifyUrl;

                PayTrades.Add(outTradeNum, trade);

                UMengPlugin.Event("alipay_pay_start", new Dictionary<string, object> {{"price", trade.Price}});

                AlipayPlugin.Pay(subject, body, price, outTradeNum, notifyUrl);
            }
            catch (Exception e)
            {
                //LoadingMask.EndLoading();
                //Toast.CreateErrorToast("解析订单数据失败！");
                Debug.LogError("解析TradeNoResult中的extra失败:" + e.StackTrace);
            }
        }

        /// <summary>
        /// 所有正在支付的订单，方便收到支付结果后反查信息，而又不用给支付上锁。
        /// </summary>
        public static readonly Dictionary<string, UserSelectedTrade> PayTrades =
            new Dictionary<string, UserSelectedTrade>();
    }
}