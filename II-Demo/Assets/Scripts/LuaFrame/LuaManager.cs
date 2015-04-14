using System.Collections;
using System.Collections.Generic;
using System.IO;
using Fairwood;
using LuaInterface;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LuaManager : MonoBehaviour
{
    public static LuaManager Instance { get; private set; }

    /// <summary>
    /// 文件列表的位置
    /// </summary>
    private static readonly string FileListPath = Path.Combine(Application.streamingAssetsPath, "LuaFileList.txt");
    public static LuaState Main;
    public static readonly Dictionary<string, string> LuaScriptDict = new Dictionary<string, string>();
    public bool HasRegisterFinished = false;

    public delegate void OnLuaRegisterFinishHandler();
    public event OnLuaRegisterFinishHandler OnLuaRegisterFinish;

    void Awake()
    {
        Instance = this;
        StartCoroutine(Init());
    }
    IEnumerator Init()
    {
        //获取LuaFileList.txt
        WWW www;
        var filePath = FileListPath;
        string txtFileList;
        if (filePath.Contains("://"))
        {
            www = new WWW(filePath);
            yield return www;
            if (www.error == null)
            {
                txtFileList = www.text;
            }
            else
            {
                Debug.LogError("CANNOT Find LuaFileList.txt:" + www.error);
                yield break;
            }
        }
        else
        {
            if (File.Exists(filePath))
            {
                txtFileList = File.ReadAllText(filePath);
            }
            else
            {
                Debug.LogError("CANNOT Find LuaFileList.txt");
                yield break;
            }
        }

        var fileList = txtFileList.Replace("\r", "").Split('\n').Where(line => line.Length > 0).ToList();
        //var luaCodeList = new List<string>();
        foreach (var luaFilePath in fileList)
        {
            var luaFileFullPath = Path.Combine(Application.streamingAssetsPath, "Lua/" + luaFilePath);
            string luaCode;
            if (luaFileFullPath.Contains("://"))
            {
                www = new WWW(luaFileFullPath);
                yield return www;
                if (www.error == null)
                {
                    luaCode = www.text;
                }
                else
                {
                    Debug.LogError("CANNOT Find " + luaFileFullPath + ":" + www.error);
                    continue;
                }
            }
            else
            {
                if (File.Exists(luaFileFullPath))
                {
                    luaCode = File.ReadAllText(luaFileFullPath);
                }
                else
                {
                    Debug.LogError("CANNOT Find " + luaFileFullPath);
                    continue;
                }
            }
            print("Register:" + luaFilePath.Substring(0, luaFilePath.Length - 4) + "," + luaCode);
            LuaScriptDict.Add(luaFilePath, luaCode);
            //luaCodeList.Add(luaCode);
        }

        Main = new LuaState();
        //foreach (var script in luaCodeList)
        //{
        //    Main.DoString(script);
        //}
        HasRegisterFinished = true;
        if (OnLuaRegisterFinish != null) OnLuaRegisterFinish();
    }

    /// <summary>
    /// 用Lua文件名获取Lua代码
    /// </summary>
    /// <param name="luaPath">require后面的路径</param>
    /// <returns></returns>
    public static string Load(string luaPath)
    {
        string code;
        if (LuaScriptDict.TryGetValue(luaPath, out code))
        {
            return code;
        }

        print("Load Fail:" + luaPath);
        return null;
    }

#if UNITY_EDITOR
    /// <summary>
    /// 生成lua文件列表
    /// </summary>
    [MenuItem("ulua Frame/Force Generate File List")]
    static void ForceGenerateFileList()
    {
        const string prefix = "Assets/StreamingAssets/Lua/";
        var paths = AssetDatabase.GetAllAssetPaths().ToList();
        paths = paths.Where(path => path.StartsWith(prefix) && path.EndsWith(".lua")).ToList();
        var prefixLength = prefix.Length;
        var str = "";
        foreach (var path in paths)
        {
            str += path.Substring(prefixLength) + "\n";
        }
        File.WriteAllText("Assets/StreamingAssets/LuaFileList.txt", str);
        Debug.Log("Generate SUCCESSFUL!!\n" + str);
    }
#endif
}
