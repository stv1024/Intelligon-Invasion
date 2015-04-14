using UnityEngine;

public abstract class BaseTempSingletonPanel : MonoBehaviour
{
    /// <summary>
    /// 将在Load时立即调用。可以放置初始化，刷新代码。生命周期内只会被调用一次。
    /// </summary>
    protected virtual void Initialize(){}

    protected void Unload()
    {
        DestroyImmediate(gameObject);
        DestroyImmediate(this);
        MainRoot.DidDestroyPanel(this);
    }
    protected virtual void OnDestroy()
    {
        //AtlasManager.DeduceUseCounts(UsedAtlasFlags);
        //MainRoot.Instance.UnloadUnusedUIAssetsAfter(30);
    }

    public virtual void OnConfirmClick()
    {
        Unload();
    }

    /// <summary>
    /// 按下返回键的处理。返回false则事件会继续传递给下面的面板
    /// </summary>
    /// <returns></returns>
    public virtual bool OnEscapeClick()
    {
        OnConfirmClick();
        return true;
    }

    public virtual string GetNameForEventStats()
    {
        var panelName = GetType().Name;
        var prefix = panelName.Substring(0, panelName.Length - 5);
        return prefix;
    }
}

/* 制作新临时单例面板教程:
 * 1.确定面板名称NAME，在ForegroundUI类里照旧添加<NAME>PanelPrefab变量；
 * 2.在Prefabs/Foregound/下找一个与新面板设计最接近的已有面板Prefab，按Ctrl+D创建副本Prefab
 * 3.将新Prefab改名为Panel-<NAME>，将它拖入到场景Main的UI Root-ForegroundUI/Respawn/下
 * 4.在Scripts/Foreground下找一个与新面板设计最接近的已有面板脚本，按Ctrl+D复制，改名为<NAME>Panel(.cs)
 * 5.先让VS reload，再打开编辑此脚本，将旧的类名全部替换为<NAME>Panel，并使之继承自BaseTempSingletonPanel
 * 6.在Load()里将ForegroundUI对旧的面板Prefab引用改为新的面板，并自行添加初始化代码
 * 7.如果面板可从扩展菜单打开，则需在ExtendMenu.cs里添加对应的单击事件方法，在其中添加<Name>Panel.Load();
 * 8.任意更改面板上的控件
 * ...可以隐藏除此面板之外的所有对象使操作更方便
 * ...
 * 9.测试：删除此面板实例，将所有场景对象激活，保存场景，点击运行，从指定入口打开面板
 */