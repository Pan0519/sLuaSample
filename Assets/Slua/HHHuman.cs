using SLua;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CustomLuaClass]
public class HHHuman
{
    protected int age = 0;

    protected string name = string.Empty;

    public static int Age { get; set; }

    public string Name { get; set; }

    public void SetAge(int age)
    {
        //Age = age;
        Debug.Log(age);
    }
}
