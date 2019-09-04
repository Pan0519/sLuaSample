using SLua;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[CustomLuaClass]
public class TestMain : LuaBehavior
{
    [SerializeField]
    Text ProgressText;

    [SerializeField]
    Text ResultText;

    static TestMain selfClass;

    [CustomLuaClass]
    public delegate void newClassDelegate(object self);
    newClassDelegate newClassDele;
    
    public override string fileName => "testlua2";

    public override void Awake()
    {
        selfClass = this;

        ShowProgressText = ProgressText;



        base.Awake();
    }

    [MonoPInvokeCallback(typeof(LuaFunction))]
    [StaticExport]
    public static int instanceTestMain(IntPtr l)
    {
        LuaObject.pushValue(l, true);
        LuaObject.pushObject(l, selfClass);
        return 2;
    }

    public override void initComplete()
    {
        //LuaSvr.mainState.getFunction("testBaseMain").call();

        //int isPrime =.convertToInt();

        //ResultText.text = string.Concat("isPrime:", isPrime);

        // 调用lua的方法

        //string strAdd = LuaSvr.mainState.getFunction("add").call(2, 1).convertToString();

        //ResultText.text = strAdd;

        //LuaSvr.mainState.getFunction("testFun").call();
        //int inter = Convert.ToInt32(LuaSvr.mainState.getFunction("sum").call(5));
    }

    // 调用委托
    public static void CallD()
    {

    }

    // lua调用 C# 脚本函数
    public void luaCallCS(string str)
    {
        ProgressText.text = str;
        //Debug.Log("c#---" + str);
    }
}
