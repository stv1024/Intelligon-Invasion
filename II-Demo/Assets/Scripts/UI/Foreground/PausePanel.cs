using System.Collections.Generic;
using UnityEngine;

public class PausePanel : BaseTempSingletonPanel
{
    #region 单例面板通用

    private static PausePanel _instance;

    public static PausePanel Instance
    {
        get { return _instance; }
        private set
        {
            if (_instance && value)
            {
                Debug.LogError("more than 1 PausePanel instance now!");
                Destroy(_instance.gameObject);
            }
            _instance = value;
        }
    }

    private void Awake()
    {
        Instance = this;
        Initialize();
    }

    protected override void OnDestroy()
    {
        Instance = null;
        base.OnDestroy();
    }

    private static GameObject Prefab
    {
        get
        {
            var go = Resources.Load("UI/Foreground/PausePanel") as GameObject;
            return go;
        }
    }

    public static void Load()
    {
        if (Instance)
        {
            MainRoot.FocusPanel(Instance);
        }
        else
        {
            if (!Prefab)
            {
                Debug.LogWarning("找不到Panel的Prefab");
                return;
            }
            MainRoot.ShowPanel(Prefab);
        }
    }

    public static void UnloadInterface()
    {
        if (Instance) Instance.Unload();
    }

    #endregion

    protected override void Initialize()
    {
        base.Initialize();
        UMengPlugin.Event("pause_show", new Dictionary<string, object>{{"level", RoundData.StageID}});
    }

    public void OnResumeClick()
    {
        Time.timeScale = 1f;
        UnloadInterface();
        UMengPlugin.Event("pause_resume", new Dictionary<string, object> { { "level", RoundData.StageID } });
    }

    public void OnRestartClick()
    {
        Time.timeScale = 1f;
        GameManager.Instance.RestartGame(RoundData.StageID);
        UnloadInterface();
        UMengPlugin.Event("pause_restart", new Dictionary<string, object> { { "level", RoundData.StageID } });
    }

    public void OnReturnToEntranceClick()
    {
        Time.timeScale = 1f;
        GameManager.Instance.Clear();
        //MainRoot.Goto(MainRoot.UIStateName.Entrance);
        UnloadInterface();
        UMengPlugin.Event("pause_quit", new Dictionary<string, object> { { "level", RoundData.StageID } });
    }

    public override void OnConfirmClick()
    {
        OnResumeClick();
    }

    public override bool OnEscapeClick()
    {
        OnResumeClick();
        return true;
    }
}