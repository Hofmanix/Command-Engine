using System;
using System.Collections.Generic;
using System.Text;

namespace CommandEngine.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    public class HelpAttribute: Attribute
    {
        public string Description { get; }

        public HelpAttribute(string description)
        {
            Description = description;
        }
    }
}
