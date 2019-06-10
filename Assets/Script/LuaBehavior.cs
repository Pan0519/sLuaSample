using Binding;
using SLua;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[CustomLuaClass]
public class LuaBehavior : MonoBase
{
    LuaSvr svr;
    LuaTable self;

    [CustomLuaClass]
    public delegate void UpdateDelegate(object self);
    UpdateDelegate ud;

    [CustomLuaClass]
    public delegate void StartDelegate(object self);
    StartDelegate sd;

    [CustomLuaClass]
    public delegate void DestoryDelegate(object self);
    DestoryDelegate destoryDele;

    [SerializeField]
    string fileName;

    Action initCompleteAction;

    public Text ShowProgressText;

    public virtual void Awake()
    {
        svr = new LuaSvr();

        svr.init(tick, InitComplete);

        LuaState.main.loaderDelegate += LuaLoader;
    }

    void tick(int p)
    {
        //Debug.Log($"Lua init Progress {p}");

        if (null == ShowProgressText)
            return;

        ShowProgressText.text = p.ToString();
    }

    void InitComplete()
    {
        self = svr.start(fileName) as LuaTable;

        //ud = functionCast<UpdateDelegate>("update");

        //sd = functionCast<StartDelegate>("start");

        //destoryDele = functionCast<DestoryDelegate>("destory");

        initComplete();
    }

    public object callLuaFunction(string functionName, params object[] args)
    {
        return LuaSvr.mainState.getFunction(functionName).call(args);
    }

    [DoNotToLua]
    public virtual void initComplete()
    {
        setBindingDataMaps();
    }

    [DoNotToLua]
    public byte[] LuaLoader(string fn, ref string absoluteFn)
    {
        if (string.IsNullOrEmpty(fn))
        {
            Debug.LogError($"Get Lua File Name is Empty");
            return new byte[] { };
        }

        string path = $"{Directory.GetCurrentDirectory()}/Assets/Script/Lua/{fn}.txt";
        return File.ReadAllBytes(path);
    }

    public virtual void Start()
    {
        StartCoroutine(waitStart());
    }

    IEnumerator waitStart()
    {
        while (sd == null)
            yield return null;

        sd(self);
    }

    void Update()
    {
        if (ud == null)
            return;

        ud(self);
    }

    private void OnDestroy()
    {
        if (destoryDele == null)
            return;

        destoryDele(self);
    }

    T functionCast<T>(string tableName) where T : class
    {
        if (null == self)
        {
            Debug.LogError($"Get {tableName} Lua Self is null");
            return null;
        }

        LuaFunction luaFunction = (LuaFunction)self[tableName];

        if (luaFunction == null)
        {
            Debug.LogError($"Get {tableName} LuaFunction is null");
        }

        return luaFunction.cast<T>();
    }
    #region setBindingDataMaps

    Dictionary<string, BindingMapsData> bindingDataMaps = new Dictionary<string, BindingMapsData>();

    void setBindingDataMaps()
    {
        var bindingDataList = GetComponent<BindingNode>().getBindings();

        for (int i = 0; i < bindingDataList.Count; ++i)
        {
            var bindingData = bindingDataList[i];

            string identifier = bindingData.getIdentifier().getIdentifier();

            if (bindingDataMaps.ContainsKey(identifier))
                continue;

            BindingMapsData bindingMapData = new BindingMapsData()
            {
                theObject = bindingData.getObject()
            };

            bindingDataMaps.Add(identifier, bindingMapData);
        }
    }

    public Component getBindingComponent(string identifiter, string type)
    {
        BindingMapsData returnValue;

        if (bindingDataMaps.TryGetValue(identifiter, out returnValue))
        {
            return returnValue.getComponent(type);
        }

        return null;
    }

    #endregion
}

public class BindingMapsData
{
    public object theObject;

    Component theCompoent;

    public Component getComponent(string componentName)
    {
        if (null == theCompoent)
        {
            var obj = theObject as GameObject;

            theCompoent = obj.GetComponent(componentName);
        }

        return theCompoent;
    }
}
