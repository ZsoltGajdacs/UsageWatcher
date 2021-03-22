using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace UsageWatcher.Native
{
    internal class DiskStatusChecker
    {
        public static bool IsDiskPowered()
        {
            bool isPowered = true;
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (FileStream file = assembly.GetFile("UsageWatcher.dll"))
            {
                if (file != null && file.Length > 0)
                {
                    IntPtr hFile = file.SafeFileHandle.DangerousGetHandle();
                    bool result = GetDevicePowerState(hFile, out bool fOn);
                    if (result)
                    {
                        if (!fOn)
                        {
                            isPowered = false;
                        }
                    }
                }
            }

            return isPowered;
        }

        [DllImport("Kernel32.DLL", CharSet = CharSet.Auto, SetLastError = true)]
        private extern static bool GetDevicePowerState(IntPtr hDevice, out bool fOn);

    }
}
