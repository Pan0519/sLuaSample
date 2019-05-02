using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectExtends
{
    public static bool convertToBool(this object obj)
    {
        return Convert.ToBoolean(obj);
    }

    public static int convertToInt(this object obj)
    {
        return Convert.ToInt32(obj);
    }

    public static string convertToString(this object obj)
    {
        return Convert.ToString(obj);
    }
}
