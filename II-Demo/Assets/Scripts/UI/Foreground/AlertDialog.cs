using UnityEngine;
using UnityEngine.UI;

public class AlertDialog : BaseTempSingletonPanel
{
    #region 单例面板通用

    private static AlertDialog _instance;

    public static AlertDialog Instance
    {
        get { return _instance; }
        private set
        {
            if (_instance && value)
            {
                Debug.LogError("more than 1 AlertDialog instance now!");
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
            var go = Resources.Load("UI/Foreground/AlertDialog") as GameObject;
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
        if (Instance) Instance.OnConfirmClick();
    }

    #endregion

    public GameObject Btn0, Btn1, BtnClose;
    public Text LblContent, LblButton0, LblButton1;

    public delegate void OnButtonClickMethod();

    public OnButtonClickMethod OnButton0Click, OnButton1Click;

    /// <summary>
    /// 带一个确认按钮的提示框
    /// </summary>
    /// <param name="content"></param>
    public static void Load(string content)
    {
        Load(content, "确认", null);
    }
    public static void Load(string content, string button0Label,
                            OnButtonClickMethod button0ClickCallback, bool showCloseButton = false)
    {
        Load();
        var ad = Instance;
        if (ad)
        {
            ad.LblContent.text = content;
            ad.LblButton0.text = button0Label;
            ad.Btn0.transform.localPosition = Vector3.zero;
            ad.OnButton0Click = button0ClickCallback;

            ad.Btn1.SetActive(true);
            ad.Btn1.SetActive(false);
            ad.BtnClose.SetActive(showCloseButton);
        }
    }

    public static void Load(string content, string button0Label,
                            OnButtonClickMethod button0ClickCallback, string button1Label, OnButtonClickMethod button1ClickCallback, bool showCloseButton = false)
    {
        Load();
        var ad = Instance;
        if (ad)
        {
            ad.LblContent.text = content;
            ad.Btn0.SetActive(true);
            ad.LblButton0.text = button0Label;
            ad.Btn0.transform.localPosition = new Vector3(-97, 0, 0);
            ad.OnButton0Click = button0ClickCallback;
            
            ad.Btn1.SetActive(true);
            ad.LblButton1.text = button1Label;
            ad.Btn1.transform.localPosition = new Vector3(97, 0, 0);
            ad.OnButton1Click = button1ClickCallback;
            
            ad.BtnClose.SetActive(showCloseButton);
        }
    }

    public void OnBtn0Click()
    {
        UnloadInterface();
        if (OnButton0Click != null) OnButton0Click();
    }

    public void OnBtn1Click()
    {
        UnloadInterface();
        if (OnButton1Click != null) OnButton1Click();
    }

    public void OnCloseClick()
    {
        UnloadInterface();
    }

    public void ProcessEscapeEvent()
    {
        if (BtnClose.activeSelf) OnCloseClick();
    }
}