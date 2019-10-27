using System;
using System.Collections.Generic;
using System.Text;

namespace CommandEngine.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AreaAttribute: Attribute
    {
        public IEnumerable<Type> AreaTypes { get; }

        public bool All { get; }

        public AreaAttribute(Type areaType): this(new[] { areaType }) { }

        public AreaAttribute(params Type[] areaTypes)
        {
            AreaTypes = areaTypes;
        }

        public AreaAttribute(bool all, params Type[] exceptions): this(exceptions)
        {
            All = all;
        }
    }
}
