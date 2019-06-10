using Binding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnifeType : LuaBehavior
{
    [SerializeField]
    Button ClickBtn;

    [SerializeField]
    Text ResultText;

    [SerializeField]
    GameObject parentGo;

    public override void initComplete()
    {
        base.initComplete();

        callLuaFunction("setUIInit");

        ClickBtn.onClick.AddListener(Click);
    }

    void Click()
    {
        callLuaFunction("setResultText");
    }
}
