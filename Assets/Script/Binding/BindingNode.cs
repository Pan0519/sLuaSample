using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Binding
{
    [AddComponentMenu("")]
    public class BindingNode : BindingHub
    {
        [SerializeField]
        private Identifier identifier = new Identifier();

        public Identifier getIdentifier()
        {
            return identifier;
        }
    }
}
