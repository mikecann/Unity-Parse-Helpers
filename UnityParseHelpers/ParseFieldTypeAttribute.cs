using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityParseHelpers
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ParseFieldTypeAttribute : Attribute
    {
        public Type FieldType { get; private set; }
        public ParseFieldTypeAttribute(Type type) { FieldType = type; }
    }
}
