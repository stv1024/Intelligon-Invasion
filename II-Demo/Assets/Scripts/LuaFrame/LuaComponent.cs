using LuaInterface;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

[AddComponentMenu("Lua/Lua Component.cs")]
public sealed class LuaComponent : MonoBehaviour
{
    public Object LuaScript;

    private const string InitCode = @"LuaComponent:new";

    public LuaTable LuaObject;

    public int TestInt = 5;

    void Init()
    {
        //初始化脚本
        if (!LuaScript) return;
        var className = LuaScript.name;
        LuaManager.Main.DoString("require(\"" + className + "\")");
        var f = LuaManager.Main.GetFunction(className + ".new");
        f.Call(LuaManager.Main[className], this);

        if (LuaObject == null)
        {
            Debug.LogError("Fetal ERROR! LuaObject == Null. Check lua code!");
            return;
        }
    }

    public bool EnableAwake = false;

    private void Awake()
    {
        Init();

        // 执行Awake
        if (EnableAwake)
        {
            CallFunction("Awake");
        }
    }
    public bool EnableStart = false;
    private void Start()
    {
        Init();

        if (EnableStart)
        {
            CallFunction("Start");
        }
    }

    public bool EnableUpdate = false;

    private void Update()
    {
        if (EnableUpdate)
        {
            CallFunction("Update");
        }
    }

    public void OnButtonClick(string buttonName)
    {

        var luaComp = GetComponent<LuaComponent>();
        if (luaComp)
        {
            luaComp.CallFunction(string.Format("On{0}Click", buttonName));
        }
    }

    public void CallFunction(string functionName)
    {
        var f = LuaManager.Main.GetFunction(LuaScript.name + "." + functionName);
        if (f != null) f.Call(LuaObject);
    }

    //void SetButtonToTriggerMe()
    //{
    //    var button = GetComponent<Button>();
    //    if (!button)
    //    {
    //        Debug.LogWarning("No Button component on this gameObject!");
    //        return;
    //    }
    //    button.onClick.AddListener(new UnityAction<string>(OnButtonClick));
    //}
}
