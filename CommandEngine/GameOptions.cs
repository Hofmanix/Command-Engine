using System;
using System.Collections.Generic;
using System.Text;

namespace CommandEngine
{
    public class GameOptions
    {
        public MethodParameterNaming MethodParametersNaming { get; set; }
        public bool ShowArea { get; set; } = true;
    }

    public enum MethodParameterNaming
    {
        Order,
        Named
    }
}
