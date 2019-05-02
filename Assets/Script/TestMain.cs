using SLua;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[CustomLuaClass]
public class TestMain : MonoBehaviour
{
    [SerializeField]
    Text ProgressText;

    [SerializeField]
    Text ResultText;

    LuaSvr svr;
    LuaTable self;
    LuaFunction update;

    LuaFunction start;

    [CustomLuaClass]
    public delegate void UpdateDelegate(object self);

    UpdateDelegate ud;

    [CustomLuaClass]
    public delegate void StartDelegate(object self);

    StartDelegate sd;

    private void Awake()
    {
        svr = new LuaSvr();

        svr.init(tick, InitComplete);

        LuaState.main.loaderDelegate += LuaLoader;
    }

    private void Start()
    {
        StartCoroutine(waitSd());
    }

    IEnumerator waitSd()
    {
        while (sd == null)
            yield return null;

        sd?.Invoke(self);
    }

    void tick(int p)
    {
        ProgressText.text = p.ToString();
    }

    void InitComplete()
    {
        self = svr.start("testlua2") as LuaTable;

        update = (LuaFunction)self["update"];

        ud = update.cast<UpdateDelegate>();

        start = (LuaFunction)self["start"];

        sd = start.cast<StartDelegate>();

        // 调用lua的方法
        //int isPrime = LuaSvr.mainState.getFunction("testFun").call().convertToInt();

        //ResultText.text = string.Concat("isPrime:", isPrime);

        //string strAdd = LuaSvr.mainState.getFunction("add").call(2, 1).convertToString();

        //ResultText.text = strAdd;

        //LuaSvr.mainState.getFunction("testFun").call();
        //int inter = Convert.ToInt32(LuaSvr.mainState.getFunction("sum").call(5));
    }

    void Update()
    {
        if (ud != null)
            Debug.Log($"ud is null?{ud == null}");
        // 调用lua的update函数
        ud?.Invoke(self);
    }

    //加载lua脚本文件
    [DoNotToLua]
    public byte[] LuaLoader(string fn, ref string absoluteFn)
    {
        string path = $"{Directory.GetCurrentDirectory()}/Assets/Script/Lua/{fn}.txt";

        return File.ReadAllBytes(path); ;
    }

    // 调用委托
    public static void CallD()
    {

    }

    // lua调用 C# 脚本函数
    public void luaCallCS(string str)
    {
        Debug.Log("c#---" + str);
    }
}
