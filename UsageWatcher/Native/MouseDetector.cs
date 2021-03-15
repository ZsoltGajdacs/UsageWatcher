using System;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using UsageWatcher.Enums;

namespace UsageWatcher.Native
{
    internal class MouseDetector : IDisposable
    {
        private Point lastMousePos;

        private readonly Timer timer;

        internal event MouseMovedEventHandler MouseMoved;
        internal delegate void MouseMovedEventHandler(object sender, Point p);

        public MouseDetector(Resolution resolution, DataPrecision precision)
        {
            lastMousePos = MouseDetector.GetMousePosition();

            double frequency = CalcTimerFrequency(resolution, precision);

            timer = new Timer(frequency);
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

                lastMousePos = currentMousePos;
            }
        }

        internal void OnMouseMoved(Point p)
        {
            MouseMoved?.Invoke(this, p);
        }

        #region static stuff
        internal class NativeMethods
        {
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetCursorPos(ref Win32Point pt);

            [StructLayout(LayoutKind.Sequential)]
            internal struct Win32Point
            {
                public Int32 X;
                public Int32 Y;
            };
        }

        private static Point GetMousePosition()
        {
            NativeMethods.Win32Point w32Mouse = new NativeMethods.Win32Point();
            NativeMethods.GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        private static double CalcTimerFrequency(Resolution resolution, DataPrecision precision)
        {
            double slashAmount = precision == DataPrecision.High ? 4 : 2;
            return (double)resolution / slashAmount;
        }

        #endregion

        #region Disposable support
        public void Dispose()
        {
            ((IDisposable)timer).Dispose();
        }
        #endregion
    }
}
