using SLua;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class jsonClass
{
    public string aaa;
    public int bbb;
    public bool ccc;
}

[CustomLuaClass]
public class ConvertJsonClass : LuaBehavior
{
    public override string fileName => "convertJson";

    jsonClass theJsonClass;

    public override void Awake()
    {
        base.Awake();

        theJsonClass = new jsonClass()
        {
            aaa = "aaa",
            bbb = 123,
            ccc = false
        };
    }

    public override void initComplete()
    {
        base.initComplete();

        var theJson = JsonUtility.ToJson(theJsonClass);
        Debug.Log(theJson);
        callLuaFunction("getJson", string.Empty);
    }
}
