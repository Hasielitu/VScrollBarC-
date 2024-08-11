using System;
// Copyright (c) Smart PC Utilities, Ltd.
// All rights reserved.

#region References

using System.Runtime.InteropServices;

namespace FlatPanelTest
{

    #endregion

    internal class NativeMethods
    {

        [DllImport("user32.dll")]
        internal static extern int SendMessage(IntPtr wnd, int msg, bool @param, int lparam);

    }
}