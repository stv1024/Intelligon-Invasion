using System.Collections;
using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    #region UI通用部分

    public virtual void Initialize(){}
    /// <summary>
    /// 延迟进场，在加载之后，激活之前
    /// </summary>
    /// <returns></returns>
    protected virtual void DelayEnterStageCoroutine()
    {
        gameObject.SetActive(false);
        MainRoot.Instance.StartCoroutine(MainRoot.DelayActivateUI(gameObject));
    }

    /// <summary>
    /// 进场动画，从激活之后。重载以添加动画。
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator EnterStageEffectCoroutine()
    {
        yield break;
    }

    /// <summary>
    /// 成员方法，退场，一段时间后必定销毁自己
    /// </summary>
    public virtual void ExitStage(bool useAnimation)
    {
        if (!gameObject.activeInHierarchy || !useAnimation)//如果在失活状态或不需要动画，直接销毁
        {
            DestroySelf();
        }
        else
        {
            Invoke("DestroySelf", MainRoot.Instance.UIExitStageDuration);
            StartCoroutine(OffStageEffectCoroutine());
        }
    }

    /// <summary>
    /// 退场动画，从开始切换之后。重载以添加动画。
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator OffStageEffectCoroutine()
    {
        yield break;
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    protected virtual void Release(){}

    protected void DestroySelf()
    {
        Release();//释放资源
        Destroy(gameObject);//帧末销毁自己。因为已经孤立，所以无所谓在一帧的什么时间点销毁
    }

    /// <summary>
    /// 按下返回键的处理。返回false则事件会继续传递给下面的面板
    /// </summary>
    /// <returns></returns>
    public virtual bool OnEscapeClick()
    {
        return false;
    }

    //NGUI工具
    //public UIWidget UIRange;

    protected virtual void ResetUIRange()
    {
        //if (!UIRange)
        //{
        //    UIRange = GetComponent<UIWidget>() ?? gameObject.AddComponent<UIWidget>();
        //}
        //var range = MainRoot.StdCameraRange;
        //UIRange.width = (int) range.x;
        //UIRange.height = (int) range.y;
    }

    /// <summary>
    /// Panel数从0变成大于0，做一些操作
    /// </summary>
    public virtual void DidPanelFromNoneToSome()
    {

    }

    /// <summary>
    /// 所有Panel都销毁了，做一些逆操作
    /// </summary>
    public virtual void DidAllPanelDestroy()
    {

    }

    #endregion
}