using System.IO;
using UnityEditor;
using UnityEngine;

namespace ProtoBuffer
{
    public class UnityProtoCompiler
    {
        [MenuItem("Fairwood/Compile Proto for Cached Data")]
        public static void CompileCachedDataProto()
        {
            var ok = EditorUtility.DisplayDialog("确认重新编译Proto吗？", "将清除旧的cs文件，并生成新的cs文件，需要等待一段时间", "是的", "不了，手滑了");

            if (!ok) return;

            //.proto文件都放在这个位置
            var path = Application.dataPath + "/Editor/Proto/Data";
            
            var dic = new ProtoBufferDic(path) {OutNameSpace = "Assets.Scripts.Data.Proto"};

            dic.Parse();

            var writer = new ProtoBufferMessageWriter();

            var writePath = Application.dataPath.Substring(0, Application.dataPath.Length - 6);
            //删除旧的Proto CS文件
            var oldCsFilePaths = Directory.GetFiles(writePath + dic.OutNameSpace.Replace('.', '/'));
            foreach (var oldCsFilePath in oldCsFilePaths)
            {
                File.Delete(oldCsFilePath);
            }
            Debug.Log("删除旧的proto文件数量:" + oldCsFilePaths.Length);
            //写入新的Proto CS文件
            foreach (var msg in dic.Messages)
            {
                writer.WriteMessage(msg, writePath);
            }

            AssetDatabase.Refresh();
        }
        [MenuItem("Fairwood/Compile Proto for Communication")]
        public static void CompileProto()
        {
            var ok = EditorUtility.DisplayDialog("确认重新编译Proto吗？", "将清除旧的cs文件，并生成新的cs文件，需要等待一段时间", "是的", "不了，手滑了");

            if (!ok) return;

            //.proto文件都放在这个位置
            var path = Application.dataPath + "/Editor/Proto/Communication";
            
            var dic = new ProtoBufferDic(path) {OutNameSpace = "Assets.Scripts.Communication.Proto"};

            dic.Parse();

            var writer = new ProtoBufferMessageWriter();

            var writePath = Application.dataPath.Substring(0, Application.dataPath.Length - 6);
            //删除旧的Proto CS文件
            var oldCsFilePaths = Directory.GetFiles(writePath + dic.OutNameSpace.Replace('.', '/'));
            foreach (var oldCsFilePath in oldCsFilePaths)
            {
                File.Delete(oldCsFilePath);
            }
            Debug.Log("删除旧的proto文件数量:" + oldCsFilePaths.Length);
            //写入新的Proto CS文件
            foreach (var msg in dic.Messages)
            {
                writer.WriteMessage(msg, writePath);
            }

            AssetDatabase.Refresh();
        }
    }
}