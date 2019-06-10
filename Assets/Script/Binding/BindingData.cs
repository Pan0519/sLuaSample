using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Binding
{
    [Serializable]
    public class Identifier
    {
        [SerializeField]
        private string identifier = string.Empty;

        public string getIdentifier()
        {
            return identifier;
        }
    }

    [Serializable]
    public class BindingData
    {
        [SerializeField]
        private Identifier identifier = new Identifier();

        [SerializeField]
        private Object @object = null;

        public void setIdentifier(Identifier identifier)
        {
            this.identifier = identifier;
        }

        public Identifier getIdentifier()
        {
            return identifier;
        }

        public Object getObject()
        {
            return @object;
        }

        public void setObject(Object @object)
        {
            this.@object = @object;
        }
    }
}
