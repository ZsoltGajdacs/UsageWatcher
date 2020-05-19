using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsageWatcher.Model;
using UsageWatcher.Native;

namespace UsageWatcher.Service
{
    internal class UsageService
    {
        private KeyboardHook keyboard;
        private MouseDetector mouse;

        private UsageStore store;

        public UsageService(UsageStore store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));

            keyboard = new KeyboardHook(KeyboardHook.Parameters.PassAllKeysToNextApp);
            keyboard.KeyIntercepted += Keyboard_KeyIntercepted;

            mouse = new MouseDetector(store.Resolution);
            mouse.MouseMoved += Mouse_MouseMoved;
        }

        private void Mouse_MouseMoved(object sender, System.Windows.Point p)
        {
            store.AddUsage(UsageType.MOUSE);
        }

        private void Keyboard_KeyIntercepted(object sender, KeyboardHook.KeyboardHookEventArgs e)
        {
            store.AddUsage(UsageType.KEYBOARD);
        }
    }
}
