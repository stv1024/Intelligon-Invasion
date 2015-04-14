using System.Collections.Generic;
using Fairwood;
using UnityEngine;

/// <summary>
/// Summary
/// </summary>
public class TimeScaleManager : MonoBehaviour
{
    #region 自动创建的单例

    private static TimeScaleManager _instance;

    public static TimeScaleManager Instance
    {
        get
        {
            TrySpawn();
            return _instance;
        }
    }

    /// <summary>
    /// 手动创建单例，不会重复的
    /// </summary>
    public static void TrySpawn()
    {
        if (!_instance)
        {
            var go = new GameObject("_TimeScaleManager");
            DontDestroyOnLoad(go);
            _instance = go.AddComponent<TimeScaleManager>(); //这时候会触发Awake()
        }
    }

    #endregion


    public struct TimeScaleEffect
    {
        public string Id;//用于查找的Id，最好不要重复。
        public float TimeScale;
        public float? StdEndTime;//两者必有一个存在，存在的用于判断
        public float? RealEndTime;//基于真实时间的判断
    }

    private static bool _isPause;

    /// <summary>
    /// 暂停
    /// </summary>
    public static bool IsPause
    {
        get { return _isPause; }
        set
        {
            _isPause = value;
            Instance.Update();
        }
    }

    /// <summary>
    /// 多重慢动作效果叠加时，取最小的那一个
    /// </summary>
    public static readonly List<TimeScaleEffect> TimeScaleEffects = new List<TimeScaleEffect>();

    private void Update()
    {
        if (IsPause)
        {
            if (Time.timeScale != 0) Time.timeScale = 0;
        }
        else
        {
            for (var i = 0; i < TimeScaleEffects.Count; )
            {
                if (TimeScaleEffects[i].StdEndTime != null)
                {
                    if (Time.time >= TimeScaleEffects[i].StdEndTime)
                    {
                        TimeScaleEffects.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
                else if (TimeScaleEffects[i].RealEndTime != null)
                {
                    if (Time.realtimeSinceStartup >= TimeScaleEffects[i].RealEndTime)
                    {
                        TimeScaleEffects.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
                else//不可能两个都空的
                {
                    Debug.LogWarning("i:" + i + ", efc:" + TimeScaleEffects.Count);
                    TimeScaleEffects.RemoveAt(i);
                }
            }

            var curTimeScale = TimeScaleEffects.Count > 0
                                   ? Mathf.Min(TimeScaleEffects.Select(x => x.TimeScale).ToArray())
                                   : 1;
            Time.timeScale = curTimeScale;
        }
    }

    /// <summary>
    /// 清空减速效果，取消暂停
    /// </summary>
    public static void ResetToStart()
    {
        TimeScaleEffects.Clear();
        IsPause = false;
    }

    /// <summary>
    /// 添加全游戏慢动作效果，填写标准游戏时间尺度下的时长，比如用于雷切开始，杀死BOSS等
    /// </summary>
    /// <param name="timeScale"></param>
    /// <param name="stdDuration">基于游戏时间</param>
    /// <param name="id"></param>
    public static void AddTimeScaleEffectWithStdDuration(float timeScale, float stdDuration, string id = null)
    {
        TimeScaleEffects.Add(new TimeScaleEffect { TimeScale = timeScale, StdEndTime = Time.time + stdDuration, Id = id });
        Instance.Update();
    }

    /// <summary>
    /// 添加全游戏慢动作效果，填写真实时间尺度下的时长，比如用于雷切开始，杀死BOSS等
    /// </summary>
    /// <param name="timeScale"></param>
    /// <param name="realDuration">基于真实时间</param>
    /// <param name="id"></param>
    public static void AddTimeScaleEffectWithRealDuration(float timeScale, float realDuration, string id = null)
    {
        TimeScaleEffects.Add(new TimeScaleEffect { TimeScale = timeScale, RealEndTime = Time.realtimeSinceStartup + realDuration, Id = id });
        Instance.Update();
    }

    public static void RemoveTimeScaleEffect(string id)
    {
        if (TimeScaleEffects.RemoveAll(x => x.Id == id) > 0)
        {
            Instance.Update();
        }
    }
    public static void RemoveAllTimeScaleEffect()
    {
        TimeScaleEffects.Clear();
    }
}