﻿// This file is part of AlarmWorkflow.
// 
// AlarmWorkflow is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// AlarmWorkflow is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with AlarmWorkflow.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace AlarmWorkflow.Windows.UI
{
    static class Helper
    {
        private const int SWP_HIDEWINDOW = 0x0080;
        private const int SWP_SHOWWINDOW = 0x0040;
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 1;

        /// <summary>
        /// Show the TaskBar.
        /// </summary>
        internal static void ShowTaskBar()
        {
            int hWnd = FindWindow("Shell_TrayWnd", "");
            SetWindowPos(hWnd, 0, 0, 0, 0, 0, SWP_SHOWWINDOW);
        }

        /// <summary>
        /// Hide the TaskBar.
        /// </summary>
        internal static void HideTaskBar()
        {
            int hWnd = FindWindow("Shell_TrayWnd", "");
            SetWindowPos(hWnd, 0, 0, 0, 0, 0, SWP_HIDEWINDOW);
        }

        /// <summary>
        /// Hide the Start-Orb
        /// </summary>
        internal static void HideStartOrb()
        {
            IntPtr hwndOrb = FindWindowEx(IntPtr.Zero, IntPtr.Zero, (IntPtr)0xC017, null);
            ShowWindow(hwndOrb, SW_HIDE);
        }

        /// <summary>
        /// Show the Start-Orb
        /// </summary>
        internal static void ShowStartOrb()
        {
            IntPtr hwndOrb = FindWindowEx(IntPtr.Zero, IntPtr.Zero, (IntPtr)0xC017, null);
            ShowWindow(hwndOrb, SW_SHOW);
        }

        /// <summary>
        /// Convenience wrapper for the "Dispatcher.Invoke()" method which does not support lambdas.
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="action"></param>
        internal static void Invoke(this Dispatcher dispatcher, Action action)
        {
            dispatcher.Invoke(action);
        }

        /// <summary>
        /// Convenience wrapper for the "Dispatcher.BeginInvoke()" method which does not support lambdas.
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="action"></param>
        internal static void BeginInvoke(this Dispatcher dispatcher, Action action)
        {
            dispatcher.BeginInvoke(action);
        }

        internal static double Limit(double min, double max, double value)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            return value;
        }

        internal static System.Drawing.Rectangle GetWindowRect(Window window)
        {
            IntPtr ptr = new WindowInteropHelper(window).Handle;
            RECT rect = new RECT();
            GetWindowRect(ptr, ref rect);

            return new System.Drawing.Rectangle(rect.Left, rect.Top, (rect.Right - rect.Left), (rect.Bottom - rect.Top));
        }
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern int FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowEx(IntPtr parentHwnd, IntPtr childAfterHwnd, IntPtr className, string windowText);
        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hwnd, int command);
        [DllImport("User32.dll")]
        private static extern int SetWindowPos(int hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}