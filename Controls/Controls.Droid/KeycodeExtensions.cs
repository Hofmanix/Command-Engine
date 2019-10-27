using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CommandEngine.Controls.Droid
{
    internal static class KeycodeExtensions
    {
        private static Dictionary<Keycode, Keys> _keycodeKeysMap = new Dictionary<Keycode, Keys>
        {
            {Keycode.A, Keys.A },
            {Keycode.AltLeft, Keys.AltLeft },
            {Keycode.AltRight, Keys.AltRight }
        };

        public static Keys ToKeys(this Keycode keycode)
        {
            return _keycodeKeysMap[keycode];
        }
    }
}