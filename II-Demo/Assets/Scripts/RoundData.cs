using System.Collections.Generic;

public static class RoundData
{
    /// <summary>
    /// 本局开始时间，少于1.1秒内不能控制玩家。
    /// </summary>
    public static float RoundStartTime;

    /// <summary>
    /// 上一次诞生或重生的时间。
    /// </summary>
    public static float LastRespawnOrReviveTime;

    /// <summary>
    /// 本局分数
    /// </summary>
    public static int Score;

    /// <summary>
    /// 第几关了，从1开始
    /// </summary>
    public static int StageID;

    /// <summary>
    /// 第几波，从1开始。Data里从0开始。
    /// </summary>
    public static int Wave;

    /// <summary>
    /// 下一次复活的价格
    /// </summary>
    public static float NextRevivePrice = 1;

    public static void ClearRoundData()
    {
        Score = 0;
    }
}