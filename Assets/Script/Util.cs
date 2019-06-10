using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static T GetComponentInParent<T>(this GameObject go, bool includeInactive) where T : Component
    {
        T[] ts = go.GetComponentsInParent<T>(includeInactive);
        return ts.Length > 0 ? ts[0] : null;
    }
}
