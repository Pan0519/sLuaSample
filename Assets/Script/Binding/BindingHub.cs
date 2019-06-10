using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Binding
{
    [DisallowMultipleComponent]
    public abstract class BindingHub : BindingComponent
    {
        [SerializeField]
        private List<BindingData> bindings = new List<BindingData>();

        public void clearBindings()
        {
            bindings.Clear();
        }

        public void AddBinding(BindingData binding)
        {
            if (null == binding)
            {
                return;
            }

            if (!bindings.Contains(binding))
            {
                bindings.Add(binding);
            }
        }

        public List<BindingData> getBindings()
        {
            return bindings;
        }
    }
}
