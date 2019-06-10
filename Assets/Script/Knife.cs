using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Knife : Singleton<Knife>
{
    BindingFlags bindingFlags
    {
        get
        {
            return BindingFlags.Public;
        }
    }

    public void Bind(Component compon)
    {
        FieldInfo[] fields = compon.GetType().GetFields(bindingFlags);

        setInfoValue(fields, compon.gameObject, compon);
    }

    public T Bind<T>(GameObject go) where T : Component
    {
        T classType = null;

        Component component = go.GetComponent<T>();

        FieldInfo[] fields = typeof(T).GetFields(bindingFlags);

        setInfoValue(fields, go, classType);

        return classType;
    }


    void setInfoValue(FieldInfo[] fields, GameObject go, System.Object classType)
    {
        //Type objectType = classType.GetType();

        //Debug.Log($"Object Type {objectType}");

        Debug.Log($"Get Field Lenght {fields.Length}");
        for (int i = 0; i < fields.Length; ++i)
        {
            FieldInfo info = fields[i];

            Type infoType = info.FieldType;

            Debug.Log($"Info  Type {infoType}-name {info.Name}");
        }
    }
}
