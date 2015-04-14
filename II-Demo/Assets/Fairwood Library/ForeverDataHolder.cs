using System;
using System.IO;
using Assets.Scripts.Data.Proto;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 一运行就要设置GetDefaultData和CheckRecoverData。设置属性时，若跟原值不一样则会自动Save；但设置列表里的元素不会自动Save。
/// </summary>
public static class ForeverDataHolder
{
    public static ForeverData ForeverData;

    static readonly string Path = System.IO.Path.Combine(Application.persistentDataPath, "ForeverData.bytes");

    /// <summary>
    /// 生成初始默认数据的方法，要确保Proto层不报错；可不满足自检条件。要在PrepareData前设置。
    /// </summary>
    public delegate ForeverData GetDefaultDataHandler();
    /// <summary>
    /// 生成初始默认数据的方法，null则会调用空构造器。要确保Proto层不报错；可不满足自检条件。要在PrepareData前设置。
    /// </summary>
    public static GetDefaultDataHandler GetDefaultData;

    /// <summary>
    /// 设置自检方法，返回是否有错
    /// </summary>
    /// <param name="foreverData"></param>
    /// <returns>有错误码</returns>
    public delegate bool CheckRecoverDataHandler(ForeverData foreverData);

    /// <summary>
    /// 设置自检方法，返回是否有错，null则不自检
    /// </summary>
    public static CheckRecoverDataHandler CheckRecoverData;

    /// <summary>
    /// 先准备好策划级数值，能自动检测和恢复，确保准确。
    /// </summary>
    /// <returns>true则会立即保存</returns>
    public static bool PrepareData(bool saveIfChanged)
    {
        if (GetDefaultData == null)
        {
            Debug.LogError("不能没有GetDefaultData");
            return true;
        }
        ForeverData foreverData;
        if (!File.Exists(Path))//被清空缓存或第一次运行，创建空数据。
        {
            foreverData = GetDefaultData();
            if (saveIfChanged) File.WriteAllBytes(Path, foreverData.GetProtoBufferBytes());
        }
        else
        {
            var bytes = File.ReadAllBytes(Path);
            foreverData = new ForeverData();
            foreverData.ParseFrom(bytes);
        }

        //检测和恢复
        if (CheckRecoverData != null && CheckRecoverData(foreverData))
        {
            if (saveIfChanged) File.WriteAllBytes(Path, foreverData.GetProtoBufferBytes());
        }

        ForeverData = foreverData;

        return true;
    }

    public static float LastSaveTime;
    /// <summary>
    /// 修改列表里的数值时不会自动Save，一定要手动Save。
    /// </summary>
    public static void Save()
    {
        File.WriteAllBytes(Path, ForeverData.GetProtoBufferBytes());
        LastSaveTime = Time.realtimeSinceStartup;
    }

    /// <summary>
    /// 删除用户数据文件
    /// </summary>
    [MenuItem("Fairwood/Delete UserData File")]
    public static void DeleteFile()
    {
        try
        {
            if (File.Exists(Path)) File.Delete(Path);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

}