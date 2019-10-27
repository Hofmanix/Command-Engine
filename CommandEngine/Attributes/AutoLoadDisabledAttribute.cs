using System;
using System.Collections.Generic;
using System.Text;

namespace CommandEngine.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    class AutoLoadDisabledAttribute: Attribute
    {
    }
}
