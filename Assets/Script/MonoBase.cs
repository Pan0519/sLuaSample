using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoBase : MonoBehaviour
{
    private GameObject mCachedGameObject;
    public  GameObject cachedGameObject
    {
        get
        {
            if (null == mCachedGameObject)
            {
                mCachedGameObject = this.gameObject;
            }

            return mCachedGameObject;
        }
    }

    private Transform mCachedTransform;
    public Transform cachedTransform
    {
        get
        {
            if (null == mCachedTransform)
            {
                mCachedTransform = cachedGameObject.transform;
            }

            return mCachedTransform;
        }
    }

    private RectTransform mRectTransform;
    public RectTransform cachedRectTransform
    {
        get
        {
            if (null == mRectTransform)
            {
                mRectTransform = (RectTransform)cachedTransform;
            }

            return mRectTransform;
        }
    }
}