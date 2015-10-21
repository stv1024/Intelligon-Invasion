using System;
using System.Collections.Generic;
using Fairwood;
using Fairwood.Math;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

/// <summary>
/// 永存，控制着游戏层。
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PreloadAllGameAssets();
    }

    public GameObject GameCamera;

    /// <summary>
    /// 有一定的计算量，最好不要频繁调用
    /// </summary>
    public static Vector2 CurrentMouseStdPos
    {
        get { return MainRoot.CurrentMouseStdPos; }
    }

    /// <summary>
    /// 物理层容器
    /// </summary>
    public Transform GameContainer;
    /// <summary>
    /// GameContainer静态接口，等同于Instance.GameContainer。
    /// </summary>
    public static Transform Container { get { return Instance.GameContainer; } }

    /// <summary>
    /// 用于存放无用东西，欺骗Unity提前加载内存的容器。
    /// </summary>
    public Transform BinContainer;

    #region Prefabs

    public GameObject NeutronPrefab;
    public GameObject PhotonPrefab;
    #endregion

    public List<Atom> AtomList = new List<Atom>();
    public List<AtomAttraction> AtomAttractionList = new List<AtomAttraction>();

    public List<Neutron> NeutronList = new List<Neutron>();

    private void Update()
    {
        UpdateInteract();

    }

    #region Creator

    public Entity CreateParticle(string particleName, Vector2 position)
    {
        Entity particle = null;
        switch (particleName)
        {
            case "Neutron":
            {
                particle = PrefabHelper.InstantiateAndReset<Neutron>(NeutronPrefab, GameContainer);
                particle.Position = position;
                break;
            }
            case "Photon":
            {
                particle = PrefabHelper.InstantiateAndReset<Photon>(PhotonPrefab, GameContainer);
                particle.Position = position;
                break;
            }
        }
        return particle;
    }
    #endregion

    #region Sprites

    public const int LAYER_ID_MAIN = 8;
    public Sprite IntelligonSprite;
    public Sprite AtomSprite;
    public Sprite AtomDecayGraphic;
    public Sprite NeutronGraphic;
    #endregion

    void PauseForPanel()
    {
        TimeScaleManager.IsPause = true;
        Time.timeScale = 0;
    }
    void ResumeFromPanel()
    {
        TimeScaleManager.IsPause = false;
    }

    #region 流程控制接口
    /// <summary>
    /// 清理所有东西。
    /// </summary>
    public void Clear()
    {
        if (GameContainer != null)
        {
            foreach (Transform child in GameContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }

        TimeScaleManager.ResetToStart();

        //清理旧的Coroutine。
        CoroutineManager.RemoveCoroutines(x=>x.ID == "Game");

        //清理UI
        if (GameUI.Instance)
        {

        }
    }

    /// <summary>
    /// 开始新的游戏。
    /// </summary>
    public void RestartGame(int stageID)
    {
        //销毁旧的东西
        Clear();

        RoundData.ClearRoundData();

        //设置新的东西
        RoundData.RoundStartTime = GameTime;
        RoundData.LastRespawnOrReviveTime = GameTime;
        RoundData.StageID = stageID;

        if (!GameContainer) //非预览测试模式，创建关卡内容
        {
            var stagePrefab = Resources.Load<GameObject>("Stages/Stage" + stageID);
            if (stagePrefab)
            {
                var container = PrefabHelper.InstantiateAndReset(stagePrefab, transform);
                GameContainer = container.transform;
            }
            else
            {
                Debug.LogError("Missing Stage" + stageID + " 's prefab!");
            }
        }
    }


    public void RevivePlayer()
    {
        TimeScaleManager.RemoveAllTimeScaleEffect();

        RoundData.LastRespawnOrReviveTime = GameTime;
    }

    #endregion

    #region 交互

    private bool _pressedLastFrame;
    private float _pressDownTime;
    private Vector2 _pressDownPos;
    /// <summary>
    /// 按住的第一帧是当前位置，之后是上一帧的位置
    /// </summary>
    private Vector2 _pressLastPos;

    //private void OnPress(bool press)
    //{
    //    Press = press;
    //    if (press)
    //    {
    //        _pressDownTime = Time.time;
    //    }
    //    else
    //    {
    //        //if (MyPlayer &&
    //        //    (MyPlayer.State == BaseCharacter.StateEnum.Stand || MyPlayer.State == BaseCharacter.StateEnum.Run))
    //        //{
    //        //    MyPlayer.StopToIdle();
    //        //}
    //        if (MyPlayer)
    //        {
    //            if (MyPlayer.State != BaseCharacter.StateEnum.Attack && MyPlayer.State != BaseCharacter.StateEnum.Die &&
    //                MyPlayer.AtkCooldown)
    //            {
    //                var curP = CurrentMouseStdPos;
    //                var direction = MyPlayer.Direction;
    //                MyPlayer.Attack(direction);
    //            }
    //        }
    //    }
    //}

    void UpdateInteract()
    {
        var cmrHitCldr = new Collider();
        //var cmrHitCldr = UICamera.lastHit.collider;
        //if (cmrHitCldr && cmrHitCldr.GetComponent<NonMaskCollider>()) cmrHitCldr = null;
        var pressed = (Input.touches.Length > 0 || Input.GetKey(KeyCode.Mouse0)) && cmrHitCldr == null;
        var currentMouseStdPos = CurrentMouseStdPos;
        

        _pressedLastFrame = pressed;
        _pressLastPos = currentMouseStdPos;
    }

    #endregion

    #region 工具
    /// <summary>
    /// 统一读取游戏时间的地方，在Time.time之后加一层，为了日后可能的特殊需要
    /// </summary>
    public static float GameTime { get { return Time.time; } }
    public static float DeltaTime { get { return Time.deltaTime; } }
    public static Vector2 GetRandomPos()
    {
        return ClampStdPos(new Vector2(Random.Range(-480f, 480f), Random.Range(-320f, 320f)));
    }
    public static Vector2 GetNearCenterRandomPos()
    {
        return Random.insideUnitCircle * 160f;
    }

    public static Vector2 ClampStdPos(Vector2 stdPos)
    {
        Vector2 newStdPos;
        newStdPos.x = Mathf.Clamp(stdPos.x, -470, 470);//人是有宽度的，不能站在墙根上
        newStdPos.y = Mathf.Clamp(stdPos.y, -290, 200);
        return newStdPos;
    }
    public static bool IsPointInsideBattleField(Vector2 stdPos)
    {
        return -320 <= stdPos.x && stdPos.x <= 320 && -568 <= stdPos.y && stdPos.y <= 568;
    }

    public Vector3 TransformDirection(Vector3 direction)
    {
        return GameContainer.TransformDirection(direction);
    }
    public Vector3 TransformPoint(Vector3 position)
    {
        return GameContainer.TransformPoint(position);
    }
    public Vector3 InverseTransformDirection(Vector3 direction)
    {
        return GameContainer.InverseTransformDirection(direction);
    }
    public Vector3 InverseTransformPoint(Vector3 position)
    {
        return GameContainer.InverseTransformPoint(position);
    }

    public static void PreloadAllGameAssets()
    {

    }
    #endregion

    #region 编辑器用
#if UNITY_EDITOR

    [ContextMenu("Clear Stage Reference")]
    void RefreshRendererSize()
    {
        GameContainer = null;
    }
#endif
    #endregion
}