using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Types
{
    /// <summary>
    /// A unity serialisable wrapper for System.Type
    /// Implicitly convertable from System.Type
    /// </summary>
    [Serializable]
    public class SerialisedType : ISerializationCallbackReceiver
    {
        public string TypeName;

        public Type Value;

        public void OnAfterDeserialize()
        {
            Value = Type.GetType(TypeName);
        }

        public void OnBeforeSerialize()
        {
            TypeName = Value.ToString();
        }

        public static implicit operator SerialisedType(Type t) => new SerialisedType { Value = t };
        public static explicit operator Type(SerialisedType t) => t.Value;
    }

}