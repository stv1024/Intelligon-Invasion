using System.Collections.Generic;
using Assets.Scripts.Data.Proto;
using Edu.CSV;
using Fairwood;
using Fairwood.Math;
using UnityEngine;

/// <summary>
/// 导航界面
/// </summary>
public class NaviUI : BaseUI
{
    #region 单例UI通用

    public static NaviUI Instance { get; private set; }

    private static GameObject Prefab
    {
        get
        {
            var go = Resources.Load("UI/NaviUI") as GameObject;
            return go;
        }
    }

    /// <summary>
    /// 实例化。开始切换时调用。不会重复加载
    /// </summary>
    /// <returns></returns>
    public static NaviUI Load(bool useAnimation = true)
    {
        if (Instance) return Instance;//不重复加载
        if (!Prefab) return null;
        var ui = MainRoot.LoadUI(Prefab, useAnimation);
        Instance = ui.GetComponent<NaviUI>();
        Instance.ResetUIRange();
        Instance.Initialize();
        if (useAnimation) Instance.DelayEnterStageCoroutine();
        return Instance;
    }

    public override void ExitStage(bool useAnimation)
    {
        Instance = null;//成为游离态
        base.ExitStage(useAnimation);
    }
    #endregion

    private GameObject _bg;

    /// <summary>
    /// 装有关卡点位置信息的父物体
    /// </summary>
    public Transform StageButtonPositionsParent;
    public Transform Map;
    public GameObject StageButtonTemplate;
    public Dictionary<int, StageButton> StageButtonDict = new Dictionary<int, StageButton>();

    public StageInfoPanel StageInfoPanel;

    void Awake()
    {
        ForceHideStageInfoPanel();
    }
    private void Start()
    {
        Refresh(CsvHolder.CsvDictionary["Stage"], ForeverDataHolder.ForeverData.UserStageDataList);
    }

    public void Refresh(CsvConfigReader stageTable, List<UserStageData> userStageDatas)
    {
        var userData = ForeverDataHolder.ForeverData;
        foreach (var line in stageTable)
        {
            if (line["Hide"]) continue;
            int id = line["ID"];
            StageButton stageButton;
            if (!StageButtonDict.TryGetValue(id, out stageButton))
            {
                var point = StageButtonPositionsParent.Find(id.ToString());
                if (null == point) Debug.LogError("请添加Stage Button Position. id:" + id);
                var pos = point.localPosition;
                stageButton = PrefabHelper.InstantiateAndReset<StageButton>(StageButtonTemplate, Map);
                stageButton.gameObject.SetActive(true);
                stageButton.name = StageButtonTemplate.name + id;
                stageButton.transform.localPosition = pos;
                stageButton.NaviUI = this;
            }
            var userStageData = userData.UserStageDataList.Find(x => x.Id == id);
            stageButton.SetAndRefresh(line, userStageData);
        }
        StageButtonTemplate.SetActive(false);
    }


    public void ToggleStageInfoPanel(StageButton sender)
    {
        var id = sender.StageID;
        if (StageInfoPanel.gameObject.activeSelf && StageInfoPanel.StageID == id)
        {
            ForceHideStageInfoPanel();
        }
        else
        {
            ForceShowStageInfoPanel(sender);
        }
    }

    public void ForceShowStageInfoPanel(StageButton sender)
    {
        StageInfoPanel.gameObject.SetActive(true);
        StageInfoPanel.transform.localPosition = sender.transform.localPosition + StageInfoPanel.Offset.ToVector3();
        var stageTable = CsvHolder.CsvDictionary["Stage"];
        var line = stageTable[sender.StageID];
        var userStageData = ForeverDataHolder.ForeverData.UserStageDataList.Find(x => x.Id == sender.StageID);
        StageInfoPanel.SetAndRefresh(line, userStageData);
    }

    public void ForceHideStageInfoPanel()
    {
        StageInfoPanel.gameObject.SetActive(false);
    }

    public void OnAddGoldClick()
    {
        Debug.Log("Add Gold");
    }

    public void OnAddEnergyClick()
    {
        Debug.Log("Add Energy");
    }

    public void OnShopClick()
    {
        Debug.Log("进商店");
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }

    public override bool OnEscapeClick()
    {
        OnQuitClick();
        return true;
    }
}