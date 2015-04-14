using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// 客户端信息载体，放在Resources/下，独立于场景，更方便更改
    /// </summary>
    public class ClientInfoHolder : MonoBehaviour
    {
        //以下都是默认信息，可以用远程配置替换。
        public string AliNotifyUrl = "";
        public string AliPkey = "";
        public string AliPid = "";
        public string AliSid = "";
        public string iOSAppId = "";
        public string WxAppId = "";
        public string UmengAppkey = "";
        public int ClientVersion;
        public SaleChannelEnum SaleChannel;
        public string SoftwareVersion;

        private static ClientInfoHolder _instance;

        public static ClientInfoHolder Instance
        {
            get { return _instance ?? (_instance = Resources.Load<ClientInfoHolder>("Data/ClientInfo Holder")); }
        }

        public static bool IsAppStore
        {
            get { return Instance.SaleChannel == SaleChannelEnum.AppStore; }
        }
        /// <summary>
        /// 渠道是Test吗
        /// </summary>
        public static bool IsTest
        {
            get { return Instance.SaleChannel == SaleChannelEnum.Test; }
        }
    }
}