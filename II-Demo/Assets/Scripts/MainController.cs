using System.Collections.Generic;
using Assets.Scripts.Data.Proto;
using Fairwood;
using Fairwood.Math;
using UnityEngine;

/// <summary>
/// 主控制器
/// </summary>
public class MainController : MonoBehaviour
{
    public static MainController Instance { get; private set; }

    /// <summary>
    /// 指定所有需要在开场实例化的Manager的Prefab
    /// </summary>
    public List<GameObject> ManagerPrefabs;  

    /// <summary>
    /// 全场最早的入口
    /// </summary>
    void Awake()
    {
        Instance = this;

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        RemoveDuplicateManagerPrefabs();
        foreach (var prefab in ManagerPrefabs)
        {
            PrefabHelper.InstantiateAndReset(prefab, null);
        }
    }
    /// <summary>
    /// 剔除重复的ManagerPrefabs
    /// </summary>
    void RemoveDuplicateManagerPrefabs()
    {
        var list = ManagerPrefabs;
        var opeInd = list.Count - 1;
        while (opeInd >= 0)
        {
            if (list[opeInd])
            {
                var compareInd = opeInd - 1;
                while (compareInd >= 0)
                {
                    if (list[compareInd].Equals(list[opeInd]))
                    {
                        list.RemoveAt(opeInd);
                        opeInd --;
                        break;
                    }
                    compareInd --;
                }
                opeInd --;
            }
            else
            {
                list.RemoveAt(opeInd);
                opeInd -= 2;
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        RemoveDuplicateManagerPrefabs();
    }
#endif

    void Start()
    {
        //获取、检测数据
        Resources.Load<CsvHolder>("Csv Holder").ForceLoadAllCsv();

        //关卡数据
        //var path = MainData.AllLevelDataPath;
        //var ta = Resources.Load<TextAsset>(path);
        //if (ta)
        //{
        //    MainData.LoadAllLevelData(ta.bytes);
        //}
        //else
        //{
        //    Debug.LogError("没有关卡数据，无法正常进行游戏");
        //}

        //永久数据
        ForeverDataHolder.GetDefaultData = GetDefaultForeverData;
        ForeverDataHolder.CheckRecoverData = CheckRecoverUserData;
        ForeverDataHolder.PrepareData(true);
        
        MainData.CheckAllData();

        CheckConditionsAndStartLaunch();
    }

    void Update()
    {
        CheckConditionsAndStartLaunch();

        if (Input.GetKeyUp(KeyCode.C))
        {
            GameUI.Load();
        }
    }

    public delegate bool LaunchConditionHandler();

    /// <summary>
    /// 在程序的最初Awake里添加条件，之后，条件全部满足时才会启动进入入口界面。
    /// 用于某些渠道需要在之前加入新的对话框等等。
    /// </summary>
    public readonly static List<LaunchConditionHandler> LaunchConditions = new List<LaunchConditionHandler>();

    /// <summary>
    /// 是否已经StartLaunch过了。
    /// </summary>
    private static bool _hasLaunched;

    /// <summary>
    /// 开始加载入口界面
    /// </summary>
    void CheckConditionsAndStartLaunch()
    {
        if (_hasLaunched) return;
        foreach (var launchCondition in LaunchConditions)
        {
            if (!launchCondition()) return;//有条件不满足就不能启动。
        }
        //满足所有条件，可以启动啦。
        _hasLaunched = true;

        NaviUI.Load(false);
    }


    public ForeverData GetDefaultForeverData()
    {
        var data = new ForeverData(0);
        var stageTable = CsvHolder.CsvDictionary["Stage"];
        var openedStages = stageTable.ToList().FindAll(line => (line["Pre Stage"] ?? 0) == 0);
        foreach (var openedStage in openedStages)
        {
            data.UserStageDataList.Add(new UserStageData(openedStage["ID"], 0, 0));
        }
        return data;
    }

    public bool CheckRecoverUserData(ForeverData userData)
    {
        //检查自动解锁状态
        var res = true;
//        res = res && CheckRecoverAutoUnlockStage(userData);
        return res;
    }

    /// <summary>
    /// 检查自动解锁状态
    /// </summary>
    /// <param name="userData"></param>
    /// <returns></returns>
    public bool CheckRecoverAutoUnlockStage(ForeverData userData)
    {
        var stageTable = CsvHolder.CsvDictionary["Stage"];
        for (var i = 0; i < userData.UserStageDataList.Count;)
        {
            var userStageData = userData.UserStageDataList[i];
            var id = userStageData.Id;
            //TODO:如果[]找不到会不会报错
            var line = stageTable[id];
            if (null == line)
            {
                Debug.LogWarning("Delete obsolete stage's user data. id:" + id);
                userData.UserStageDataList.RemoveAt(i);//删除已删除关卡对应的用户数据
            }
            else
            {
                //TODO:目前遍历关卡树需要n^2，暂且不需要
                i++;
            }
        }
        return true;
    }


    public void EnterStage(int id)
    {
        Debug.LogWarning("EnterStage " + id);
        GameUI.Load();
        GameManager.Instance.RestartGame(id);
    }
}