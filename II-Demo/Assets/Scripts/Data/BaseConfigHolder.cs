using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Data
{
    /// <summary>
    /// 文本字符串，启动后，支持热更新
    /// </summary>
    public abstract class BaseConfigHolder : MonoBehaviour
    {
        public const string REMOTE_FOLDER_PATH = "http://***.qiniu.com/";

        public abstract string Filename { get; }
        /// <summary>
        /// Resources.Load(**)的路径
        /// </summary>
        public string ResourcePath { get { return Path.Combine("Data/", Path.GetFileNameWithoutExtension(Filename)??Filename); } }
        //public string StreamingPath { get { return Path.Combine(Application.streamingAssetsPath, Filename); } }
        public string PersistentPath { get { return Path.Combine(Application.persistentDataPath, Filename); } }
        public string RemotePath { get { return Path.Combine(REMOTE_FOLDER_PATH, Filename); } }

        /// <summary>
        /// 自动重新获取时间间隔。0表示不重新获取。
        /// </summary>
        public abstract int AutoRefetchInterval { get; }

        /// <summary>
        /// 如果是null则从未获取过。
        /// </summary>
        public string Text;

        /// <summary>
        /// 启动时最早调用，可判断有无错误。
        /// </summary>
        public void PrepareData()
        {
            LoadDefaultData();
        }

        protected void Awake()
        {
            Refetch();
            if (AutoRefetchInterval > 0)
            {
                InvokeRepeating("Refetch", AutoRefetchInterval, AutoRefetchInterval);
            }
        }

        protected void Refetch()
        {
            DownloadMgr.Instance.DownString(RemotePath, result =>
                {
                    if (result.StatusCode == 200)//下载成功
                    {
                        var content = result.Content;
                        if (content != null && Text != content)
                        {
                            Text = content;
                            foreach (var listener in NewConfigUpdateListeners)
                            {
                                listener.UpdateConfig(content);
                            }
                            try
                            {
                                File.WriteAllText(PersistentPath, Text);
                            }
                            catch (Exception e)
                            {
                                Debug.LogException(e);
                            }
                        }
                    }
                    else//下载失败
                    {
                        if (Text == null)//从未获取过，尝试获取缓存的
                        {
                            try
                            {
                                if (File.Exists(PersistentPath))
                                {
                                    Text = File.ReadAllText(PersistentPath);
                                }
                                else
                                {
                                    LoadDefaultData();   
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.LogException(e);
                            }
                            finally
                            {
                                LoadDefaultData();
                            }
                        }
                    }
                });
        }

        void LoadDefaultData()
        {
            var ta = Resources.Load<TextAsset>(ResourcePath);
            if (ta)
            {
                Text = ta.text;
            }
            else
            {
                Debug.LogError("怎能没有默认数据" + name);
            }
        }

        readonly static List<INewConfigUpdateListener> NewConfigUpdateListeners = new List<INewConfigUpdateListener>();
        public static bool AddListener(INewConfigUpdateListener listener)
        {
            if (listener == null) return false;
            if (NewConfigUpdateListeners.Contains(listener)) return false;
            NewConfigUpdateListeners.Add(listener);
            return true;
        }
        public static bool RemoveListener(INewConfigUpdateListener listener)
        {
            if (listener == null) return false;
            return NewConfigUpdateListeners.Remove(listener);
        }

        public interface INewConfigUpdateListener
        {
            void UpdateConfig(string text);
        }
    }
}