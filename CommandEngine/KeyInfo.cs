using System;
using System.Collections.Generic;
using System.Text;

namespace CommandEngine
{
    public struct KeyInfo
    {
        public Keys Key { get; }
        public string Text { get; }
        public bool Alt { get; }
        public bool Shift { get; }
        public bool Ctrl { get; }

        public KeyInfo(Keys key, string text, bool alt, bool shift, bool ctrl)
        {
            Key = key;
            Text = text;
            Alt = alt;
            Shift = shift;
            Ctrl = ctrl;
        }
    }
}
