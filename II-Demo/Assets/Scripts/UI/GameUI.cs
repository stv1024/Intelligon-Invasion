using UnityEngine;

public class GameUI : BaseUI
{
    #region 单例UI通用

    public static GameUI Instance { get; private set; }

    private static GameObject Prefab
    {
        get
        {
            var go = Resources.Load("UI/GameUI") as GameObject;
            return go;
        }
    }

    /// <summary>
    /// 实例化。开始切换时调用。不会重复加载
    /// </summary>
    /// <returns></returns>
    public static GameUI Load(bool useAnimation = true)
    {
        if (Instance) return Instance;//不重复加载
        if (!Prefab) return null;
        var ui = MainRoot.LoadUI(Prefab, useAnimation);
        Instance = ui.GetComponent<GameUI>();
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

    public void OnPauseClick()
    {
        Time.timeScale = 0f;

        PausePanel.Load();
    }

    public void TmpRestart()
    {
        Application.LoadLevel(0);
    }

    public override bool OnEscapeClick()
    {
        OnPauseClick();
        return true;
    }
}