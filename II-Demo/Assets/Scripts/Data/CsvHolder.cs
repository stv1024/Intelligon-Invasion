using System.Collections.Generic;
using System.IO;
using Edu.CSV;
using Fairwood;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 所有的CSV数据表的载体
/// </summary>
public class CsvHolder : MonoBehaviour
{
    public List<TextAsset> CsvTextAssets = new List<TextAsset>();

    public static readonly Dictionary<string, CsvConfigReader> CsvDictionary = new Dictionary<string, CsvConfigReader>();

    //public readonly Dictionary<string, Dictionary<int, Dictionary<string, object>>> CsvDictionary =
    //    new Dictionary<string, Dictionary<int, Dictionary<string, object>>>();

    public void ForceLoadAllCsv()
    {
        CsvDictionary.Clear();
        foreach (var csvTextAsset in CsvTextAssets)
        {
            var text = csvTextAsset.text;
            var reader = new CsvConfigReader();
            reader.Read(new StringReader(text));
            //var table = new Dictionary<int, Dictionary<string, object>>();
            CsvDictionary.Add(csvTextAsset.name, reader);
            //var csvKeys = reader.Keys;
            
            //foreach (var csvKey in csvKeys)
            //{
            //    var line = new Dictionary<string, object>();
            //    table.Add(csvLine.);
            //}
        }
    }

#if UNITY_EDITOR
    [MenuItem("Fairwood/Force Register All Csv Files")]
    static void ForceRegisterAllCsv()
    {
        var csvHolder = Resources.Load<CsvHolder>("Csv Holder");
        const string prefix = "Assets/csv/";
        var paths = AssetDatabase.GetAllAssetPaths().ToList();

        paths = paths.Where(path => path.StartsWith(prefix) && path.EndsWith(".txt")).ToList();

        csvHolder.CsvTextAssets =
            paths.Select(p => AssetDatabase.LoadAssetAtPath(p, typeof (TextAsset)) as TextAsset).ToList();

        EditorUtility.SetDirty(csvHolder);
    }
#endif
}