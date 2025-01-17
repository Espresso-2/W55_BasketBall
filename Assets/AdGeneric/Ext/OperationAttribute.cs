using System;

namespace AdGeneric.Ext
{
    public class OperationAttribute:Attribute
    {
        public global::AdGeneric.Operation.Operation Operation { get; set; }

        public OperationAttribute(global::AdGeneric.Operation.Operation operation)
        {
            Operation = operation;
        }
    }
}