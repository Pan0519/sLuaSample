using SLua;
using System;
using System.Collections;
using UnityEngine;

public class Circle : MonoBehaviour
{


    LuaSvr svr;
    LuaTable self;
    LuaFunction update;

    [CustomLuaClass]
    public delegate void UpdateDelegate(object self);

    UpdateDelegate ud;

    void Start()
    {
        svr = new LuaSvr();
        svr.init(tick, () =>
        {
            string fileName = nameof(Circle).ToLower();
            self = (LuaTable)svr.start($"{fileName}/{fileName}");
            Debug.Log($"Self is null ? {self == null} FileName {fileName}");
            update = (LuaFunction)self["update"];
            ud = update.cast<UpdateDelegate>();
        });
    }

    void tick(int p)
    {
        Debug.Log($"Progress Run : {p}");
    }

    void Update()
    {
        if (ud != null) ud(self);
    }
}
