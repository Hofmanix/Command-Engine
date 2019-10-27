using System;
using System.Collections.Generic;
using System.Text;

namespace CommandEngine.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class NameAttribute: Attribute
    {
        public string Name;
        public string ShortName;
        
        public NameAttribute(string name)
        {
            Name = name;
        }

        public NameAttribute(string name, string shortName): this(name)
        {
            ShortName = shortName;
        }
    }
}
