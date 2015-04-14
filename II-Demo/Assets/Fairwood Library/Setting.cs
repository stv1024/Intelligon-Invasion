using UnityEngine;

namespace Assets.Scripts
{
    public class Setting
    {
        public static string DeviceUID
        {
            get { return SystemInfo.deviceUniqueIdentifier; }
        }
    }
}