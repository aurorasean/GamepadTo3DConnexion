using GamePad3DConnexion.Settings;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;

namespace GamePad3DConnexion
{
    public enum MouseAction
    {
        Down = 0,
        Up = 1
    }

    public enum MouseButtons
    {
        Left = 0,
        Right = 1,
        Middle = 2,
        Wheel = 3
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public static implicit operator Point(POINT point)
        {
            return new Point(point.X, point.Y);
        }
    }

    public static class MouseHelper
    {
        public const int KEYBDEVENTF_KEYDOWN = 0;
        public const int KEYBDEVENTF_KEYUP = 2;
        public const byte KEYBDEVENTF_SHIFTSCANCODE = 0x2A;
        public const byte KEYBDEVENTF_SHIFTVIRTUAL = 0x10;
        private const int MOUSEEVENT_LEFTDOWN = 0x02;
        private const int MOUSEEVENT_LEFTUP = 0x04;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        private const int MOUSEEVENTF_MOVE = 0x0001;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const int MOUSEEVENTF_RIGHTUP = 0x0010;
        private const int MOUSEEVENTF_WHEEL = 0x0800;
        private static bool Initialised = false;
        private static Dictionary<MouseButtons, Dictionary<MouseAction, int>> keyValuePairs = new Dictionary<MouseButtons, Dictionary<MouseAction, int>>();

        public static Point GetCursorPosition()
        {
            GetCursorPos(out POINT lpPoint);
            return lpPoint;
        }

        public static void Initialize()
        {
            if (!Initialised)
            {
                Dictionary<MouseAction, int> buttonLeft = new Dictionary<MouseAction, int>
                {
                    { MouseAction.Down, MOUSEEVENT_LEFTDOWN },
                    { MouseAction.Up, MOUSEEVENT_LEFTUP }
                };
                Dictionary<MouseAction, int> buttonRight = new Dictionary<MouseAction, int>
                {
                    { MouseAction.Down, MOUSEEVENTF_RIGHTDOWN },
                    { MouseAction.Up, MOUSEEVENTF_RIGHTUP }
                };
                Dictionary<MouseAction, int> buttonMiddle = new Dictionary<MouseAction, int>
                {
                    { MouseAction.Down, MOUSEEVENTF_MIDDLEDOWN },
                    { MouseAction.Up, MOUSEEVENTF_MIDDLEUP }
                };
                Dictionary<MouseAction, int> buttonWheel = new Dictionary<MouseAction, int>
                {
                    { MouseAction.Down, MOUSEEVENTF_WHEEL },
                    { MouseAction.Up, MOUSEEVENTF_WHEEL }
                };
                keyValuePairs.Add(MouseButtons.Left, buttonLeft);
                keyValuePairs.Add(MouseButtons.Right, buttonRight);
                keyValuePairs.Add(MouseButtons.Middle, buttonMiddle);
                keyValuePairs.Add(MouseButtons.Wheel, buttonWheel);
                Initialised = true;
            }
        }

        [DllImport("user32.dll", EntryPoint = "keybd_event", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern void keybd_event(byte vk, byte scan, int flags, int extrainfo);

        public static void SendKeyBoardDown(KeyBoardValues keyBoardValues)
        {
            keybd_event((byte)keyBoardValues, KEYBDEVENTF_SHIFTSCANCODE, KEYBDEVENTF_KEYDOWN, 0);
        }

        public static void SendKeyBoardUp(KeyBoardValues keyBoardValues)
        {
            keybd_event((byte)keyBoardValues, KEYBDEVENTF_SHIFTSCANCODE, KEYBDEVENTF_KEYUP, 0);
        }

        public static void SendMouseEvent(MouseButtons button, MouseAction action, int x, int y, int dwData, int dxExtraInfo = 0)
        {
            int actionCode = keyValuePairs[button][action];
            mouse_event(actionCode, x, y, dwData, dxExtraInfo);
        }

        public static void SetCursorPosition(double x, double y)
        {
            SetCursorPos((int)x, (int)y);
        }

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dxExtraInfo);

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        private static extern bool SetCursorPos(int x, int y);
    }
}