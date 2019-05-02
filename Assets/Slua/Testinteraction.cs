using SLua;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CustomLuaClass]
public class Testinteraction : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Test_interaction,Hello-----");
    }

    public void ShowLog()
    {
        Debug.Log("Show");
    }

    public void aaa(string a)
    {
        Debug.Log(a.ToString());
    }
}
