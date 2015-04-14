using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// DataWatcher
/// </summary>
public class DataWatcher : EditorWindow
{
    [MenuItem("Fairwood/DataWatcher")]
    static void Init()
    {
        GetWindow<DataWatcher>("数据监视器", true);
    }

    void Update()
    {
        Repaint();
    }

    void OnGUI()
    {
        //var gm = GameManager.Instance;
        //if (gm)
        //{
        //    foreach (var timeScaleEffect in gm.TimeScaleEffects)
        //    {
        //        EditorGUILayout.LabelField("" + timeScaleEffect.TimeScale + "," + timeScaleEffect.StdEndTime + "," +
        //                                   timeScaleEffect.RealEndTime);
        //    }
        //}

        _watchForeverData = EditorGUILayout.Foldout(_watchForeverData, "ForeverData");
        if (_watchForeverData)
        {
            if (ForeverDataHolder.ForeverData == null)
            {
                ForeverDataHolder.PrepareData(false);
            }
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            {
                WatchForeverData();
            }
            EditorGUILayout.EndScrollView();
        }
    }

    private static bool _watchForeverData = true;
    private static Vector2 _scrollPos;
    static void WatchForeverData()
    {
        if (ForeverDataHolder.ForeverData == null)
        {
            EditorGUILayout.LabelField("没有数据");
            return;
        }
        //var foreverData = ForeverDataHolder.ForeverData;
        //var fltMoney = (float)ForeverDataHolder.ForeverData.Money;
        //var newFltMoney = EditorGUILayout.FloatField("Money", fltMoney);
        //if (newFltMoney != fltMoney)
        //{
        //    ForeverDataHolder.ForeverData.Money = newFltMoney;
        //}

        for (int i = 0; i < ForeverDataHolder.ForeverData.UserStageDataList.Count; i++)
        {
            ForeverDataHolder.ForeverData.UserStageDataList[i].Id = EditorGUILayout.IntField("[" + i + "].ID",
                ForeverDataHolder
                    .ForeverData
                    .UserStageDataList[i].Id);
            ForeverDataHolder.ForeverData.UserStageDataList[i].Stars = EditorGUILayout.IntField("[" + i + "].Stars",
                ForeverDataHolder
                    .ForeverData
                    .UserStageDataList[i].Stars);
        }
        //ForeverDataHolder.ForeverData.SwipeAddMoneyLevel = EditorGUILayout.IntField("SwipeAddMoneyLevel",
        //                                                                            ForeverDataHolder.ForeverData
        //                                                                                             .SwipeAddMoneyLevel);

        //var str = EditorGUILayout.TextField("GameTick", "" + ForeverDataHolder.ForeverData.GameTick);
        //long l;
        //if (long.TryParse(str, out l))
        //{
        //    ForeverDataHolder.ForeverData.GameTick = l;
        //}
        //str = EditorGUILayout.TextField("NextEventTick", "" + ForeverDataHolder.ForeverData.NextEventTick);
        //if (long.TryParse(str, out l))
        //{
        //    ForeverDataHolder.ForeverData.NextEventTick = l;
        //}

        //var flt = (float)ForeverDataHolder.ForeverData.ExtraFgAddMoneyRate;
        //var newFlt = EditorGUILayout.FloatField("ExtraFgAddMoneyRate", flt);
        //if (newFlt != flt)
        //{
        //    ForeverDataHolder.ForeverData.ExtraFgAddMoneyRate = newFlt;
        //}

        //flt = (float)ForeverDataHolder.ForeverData.ExtraSwipeAddMoney;
        //newFlt = EditorGUILayout.FloatField("ExtraSwipeAddMoney", flt);
        //if (newFlt != flt)
        //{
        //    ForeverDataHolder.ForeverData.ExtraSwipeAddMoney = newFlt;
        //}

        //ForeverDataHolder.ForeverData.FgAddMoneyMultiple = EditorGUILayout.IntField("FgAddMoneyMultiple",
        //                                                                            ForeverDataHolder.ForeverData
        //                                                                                             .FgAddMoneyMultiple);
        //foreverData.HasRatedStar = EditorGUILayout.Toggle("HasRatedStar", foreverData.HasRatedStar);
        //foreverData.AppstoreApproved = EditorGUILayout.Toggle("AppstoreApproved", foreverData.AppstoreApproved);
        //EditorGUILayout.LabelField("WxinviteUrl", foreverData.WxinviteUrl);

        //ForeverDataHolder.ForeverData.LastWxDay = EditorGUILayout.IntField("LastWxDay",ForeverDataHolder.ForeverData.LastWxDay);
        //ForeverDataHolder.ForeverData.WxTimesToday = EditorGUILayout.IntField("WxTimesToday", ForeverDataHolder.ForeverData.WxTimesToday);

        EditorGUILayout.Space();
        if (GUILayout.Button("保存数据"))
        {
            ForeverDataHolder.Save();
        }
        if (GUILayout.Button("删除数据"))
        {
            var ok = EditorUtility.DisplayDialog("确认删除用户数据吗？", "删了不会有事的", "是的", "不了");
            if (ok)
            {
                ForeverDataHolder.DeleteFile();
            }
        }
        if (GUILayout.Button("打印升级项目文案数据"))
        {
            LogOutTemp();
        }
    }

    static void ShowIntList(string label, List<int> list)
    {
        EditorGUILayout.BeginVertical();
        if (label != null) EditorGUILayout.LabelField(label);
        for (int i = 0; i < list.Count; i++)
        {
            var v = list[i];
            EditorGUILayout.IntField("" + i, v);
        }
        EditorGUILayout.EndVertical();
    }
    static void ShowBoolList(string label, List<bool> list)
    {
        EditorGUILayout.BeginVertical();
        if (label != null) EditorGUILayout.LabelField(label);
        for (int i = 0; i < list.Count; i++)
        {
            var v = list[i];
            EditorGUILayout.Toggle("" + i, v);
        }
        EditorGUILayout.EndVertical();
    }
    static void ShowList<T>(List<T> list, Type type)
    {
        EditorGUILayout.BeginVertical();
        for (int i = 0; i < list.Count; i++)
        {
            var v = list[i];
            EditorGUILayout.ObjectField("" + i, v as UnityEngine.Object, type, true);
        }
        EditorGUILayout.EndVertical();
    }

    static void LogOutTemp()
    {
        var log = "";
        //var upgradableItemList = MainData.UpgradableItemList;
        //for (int i = 0; i < upgradableItemList.Count; i++)
        //{
        //    var upgradableItem = upgradableItemList[i];
        //    log += "Upgrade_" + upgradableItem.Id + "_Name,\"English\",\"" + upgradableItem.DisplayName + "\"\n";
        //    log += "Upgrade_" + upgradableItem.Id + "_Desc,\"English\",\"" + upgradableItem.Description + "\"\n";
        //    var levelInfoList = upgradableItem.LevelInfoList;
        //    for (int j = 0; j < levelInfoList.Count; j++)
        //    {
        //        log += "Upgrade_" + upgradableItem.Id +"_"+ j + "_Desc,\"English\",\"" + levelInfoList[j].Description + "\"\n";
        //        log += "Upgrade_" + upgradableItem.Id +"_"+ j + "_UpDesc,\"English\",\"" + levelInfoList[j].UpgradeDescription.Replace("\n","\\n") + "\"\n";
        //    }
        //}
        Debug.Log(log);
    }
}