using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Binding
{
    [AddComponentMenu("")]
    public class BindingElement : BindingComponent
    {
        [SerializeField]
        private BindingData data = new BindingData();

        public BindingData getData()
        {
            return data;
        }
    }
}
