using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Windows;
using UsageWatcher.Model;

namespace UsageWatcher.Native
{
    internal class MouseDetector
    {
        private Point lastMousePos;

        private Timer timer;

        internal event MouseMovedEventHandler MouseMoved;
        internal delegate void MouseMovedEventHandler(object sender, Point p);

        public MouseDetector(Resolution resolution)
        {
            lastMousePos = MouseDetector.GetMousePosition();

            timer = new Timer((int)resolution);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Point currentMousePos = MouseDetector.GetMousePosition();

            if (currentMousePos != lastMousePos)
            {
                OnMouseMoved(currentMousePos);
            }
        }

        internal void OnMouseMoved(Point p)
        {
            MouseMoved?.Invoke(this, p);
        }

        #region static stuff
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        private struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        private static Point GetMousePosition()
        {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        #endregion
    }
}
