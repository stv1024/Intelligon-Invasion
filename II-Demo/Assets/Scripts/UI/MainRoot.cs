using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fairwood.Math;

/// <summary>
/// UI Root手动调节高度
/// </summary>
public class MainRoot : MonoBehaviour
{
    public static MainRoot Instance { get; private set; }

    public const int MainLayer = 8;
    public Camera MyCamera;

    public enum ScreenDirectionEnum
    {
        Portrait,
        Landscape,
    }

    public ScreenDirectionEnum ScreenDirection;
    
    public Transform ForegroundGUI;

    public GameObject ChangeUIMask;//UI过渡时前景遮罩

    void Awake()
    {
        Instance = this;
        var h = Camera.main.pixelHeight;
        var w = Camera.main.pixelWidth;
        Debug.Log("屏幕分辨率:" + w + "*" + h + "; Ratio:" + (h * 1f / w));
        if (ScreenDirection == ScreenDirectionEnum.Landscape)
        {
            if (h/w > 1f/1.5f) //如果屏幕太高，就在上下增加白边
            {
                // ReSharper disable PossibleLossOfFraction
                MyCamera.orthographicSize = 3.2f*h/w/(1f/1.5f);
                // ReSharper restore PossibleLossOfFraction
                //if (Background) Background.localScale = new Vector3(1, Screen.height * 1.5f / Screen.width, 1);
            }
            else if (h/w < 1.5f)
            {
                //if (Background) Background.localScale = new Vector3(Screen.width / 1.5f / Screen.height, 1, 1);
            }
        }
        else
        {
            //if (w / h > 1f / 1.5f) //如果屏幕太高，就在上下增加白边
            //{
            //    // ReSharper disable PossibleLossOfFraction
            //    MyCamera.orthographicSize = 3.2f * h / w / (1f / 1.5f);
            //    // ReSharper restore PossibleLossOfFraction
            //    //if (Background) Background.localScale = new Vector3(1, Screen.height * 1.5f / Screen.width, 1);
            //}
            //else if (h / w < 1.5f)
            //{
            //    //if (Background) Background.localScale = new Vector3(Screen.width / 1.5f / Screen.height, 1, 1);
            //}
        }
    }

    /// <summary>
    /// 有一定的计算量，最好不要频繁调用
    /// </summary>
    public static Vector2 CurrentMouseStdPos
    {
        get
        {
            //var stdPos = InverseTransformPoint(
            //        Camera.main.ScreenPointToRay(UICamera.lastTouchPosition).origin).ToVector2();
            return new Vector2();
        }
    }

    public static Vector2 StdCameraRange
    {
        get
        {
            if (Instance.ScreenDirection == ScreenDirectionEnum.Landscape)
            {
                var range = new Vector2(960, 640);
                var h = Camera.main.pixelHeight;
                var w = Camera.main.pixelWidth;
                if (w*1f/h > 1.5f) //如果屏幕太宽
                {
                    range.x *= w/1.5f/h;
                }
                else if (w*1f/h < 1.5f)
                {
                    range.y *= h*1.5f/w;
                }
                return range;
            }
            else
            {
                return new Vector2(640, 1136);
            }
        }
    }

    /// <summary>
    /// UI进场延迟
    /// </summary>
    public float UIEnterStageDelay;
    /// <summary>
    /// UI退场的时间
    /// </summary>
    public float UIExitStageDuration;
    /// <summary>
    /// 当前UI，每时每刻最多只有一个UI
    /// </summary>
    public BaseUI CurrentUI;

    /// <summary>
    /// UI放在什么东西下面
    /// </summary>
    public static Transform UIParent { get { return Instance.UIContainer; } }

    public Transform UIContainer;

    public static BaseUI LoadUI(GameObject prefab, bool useAnimation)
    {
        if (Instance.CurrentUI) Instance.CurrentUI.ExitStage(useAnimation);
        //var go = Instantiate(prefab) as GameObject;
        //go.name = prefab.name;
        //go.transform.parent = UIParent;
        //Instance.CurrentUI = go.GetComponent<BaseUI>();
        Instance.CurrentUI = PrefabHelper.InstantiateAndReset<BaseUI>(prefab, UIParent);
        return Instance.CurrentUI;
    }
    public static IEnumerator DelayActivateUI(GameObject uiGo)
    {
        yield return new WaitForSeconds(Instance.UIEnterStageDelay);
        if (uiGo) uiGo.SetActive(true);//有可能在等待激活过程中被销毁
    }


    private readonly List<BaseTempSingletonPanel> _baseTempSingletonPanels = new List<BaseTempSingletonPanel>(); 
    public static int TempSingletonPanelsCount { get { return Instance._baseTempSingletonPanels.Count; } }

    public static void ShowPanel(GameObject prefab)
    {
        if (Instance._baseTempSingletonPanels.Count <= 0) //Panel从无到有
        {
            Instance.CurrentUI.DidPanelFromNoneToSome();
        }
        foreach (var baseTempSingletonPanel in Instance._baseTempSingletonPanels)
        {
            baseTempSingletonPanel.gameObject.SetActive(false);//已存在的面板都失活，告知UI
        }
        Instance._baseTempSingletonPanels.Add(
            PrefabHelper.InstantiateAndReset<BaseTempSingletonPanel>(prefab, Instance.ForegroundGUI));
    }
    /// <summary>
    /// 将Panel列表中指定的一个提到最前
    /// </summary>
    /// <param name="panel"></param>
    public static void FocusPanel(BaseTempSingletonPanel panel)
    {
        if (panel == null)
        {
            Debug.LogException(new NullReferenceException("panel"));
            return;
        }
        Instance._baseTempSingletonPanels.Remove(panel);
        foreach (var baseTempSingletonPanel in Instance._baseTempSingletonPanels)
        {
            if (baseTempSingletonPanel != panel)
            {
                baseTempSingletonPanel.gameObject.SetActive(false);
            }
        }
        Instance._baseTempSingletonPanels.Add(panel);//将panel提到最上
        panel.gameObject.SetActive(true);//激活面板
    }
    public static void DidDestroyPanel(BaseTempSingletonPanel panel)
    {
        if (!Instance._baseTempSingletonPanels.Remove(panel)) Debug.LogError("怎么可能不在列表里，请检查bug隐患");
        if (Instance._baseTempSingletonPanels.Count > 0)
        {
            Instance._baseTempSingletonPanels[Instance._baseTempSingletonPanels.Count - 1].gameObject.SetActive(true);//激活最上层Panel
        }
        else
        {
            Instance.CurrentUI.DidAllPanelDestroy();//所有面板都销毁了，告知UI
        }
    }

    /// <summary>
    /// 当前UI/Panel状态，用于数据统计
    /// </summary>
    public string CurrentViewStateName
    {
        get
        {
            if (_baseTempSingletonPanels.Count > 0)
            {
                return _baseTempSingletonPanels[_baseTempSingletonPanels.Count - 1].GetNameForEventStats();
            }
            return CurrentUI.ToString();
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnEscapeClick();
        }
        else if (Input.GetKeyUp(KeyCode.F11))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }
    void OnEscapeClick()
    {
        //TODO 先处理Dialogs

        if (AlertDialog.Instance)
        {
            AlertDialog.Instance.ProcessEscapeEvent();
            return;
        }

        //再处理Panels
        for (var i = _baseTempSingletonPanels.Count - 1; i >= 0; i--)
        {
            if (_baseTempSingletonPanels[i].OnEscapeClick()) return;
        }

        //最后处理UIs
        if (CurrentUI.OnEscapeClick()) return;
    }

    public static Vector3 InverseTransformPoint(Vector3 position)
    {
        return Instance.transform.InverseTransformPoint(position);
    }

    #region LayerInfo

    public const int LayerID_Game = 2;
    public const int LayerID_UI = 3;

    #endregion
}