using System;
using UnityEngine;

namespace AdGeneric.Ext
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AdInspectorButtonAttribute:PropertyAttribute
    {
        public string Name { get; set; }

        public AdInspectorButtonAttribute(string name)
        {
            Name = name;
        }
    }
}