using SLua;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

[CustomLuaClass]
public class LuaBehavior : MonoBehaviour
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

    public virtual void Awake()
    {
        svr = new LuaSvr();

        svr.init(tick, InitComplete);

        LuaState.main.loaderDelegate += LuaLoader;
    }

    void tick(int p)
    {
        Debug.Log($"Lua init Progress {p}");
    }

    void InitComplete()
    {
        self = svr.start(fileName) as LuaTable;

        ud = functionCast<UpdateDelegate>("update");

        sd = functionCast<StartDelegate>("start");

        destoryDele = functionCast<DestoryDelegate>("destory");

        initComplete();
    }

    [DoNotToLua]
    public virtual void initComplete() { }

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
        LuaFunction luaFunction = (LuaFunction)self[tableName];

        if (luaFunction == null)
        {
            Debug.LogError($"Get {tableName} LuaFunction is null");
        }

        return luaFunction.cast<T>();
    }
}
