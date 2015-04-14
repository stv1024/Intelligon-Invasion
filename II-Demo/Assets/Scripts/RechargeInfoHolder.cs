using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 充值包列表配置，全由客户端配置咯。
/// </summary>
public class RechargeInfoHolder : MonoBehaviour
{
    private static RechargeInfoHolder _instance;

    public static RechargeInfoHolder Instance
    {
        get { return _instance ?? (_instance = Resources.Load<RechargeInfoHolder>("RechargeInfoHolder")); }
    }

    /// <summary>
    /// Main
    /// </summary>
    public List<string> Name;
    public List<bool> Open;
    public List<string> DisplayName;
    public List<float> Price;
    public List<int> Coin;
    public List<string> Code;
}