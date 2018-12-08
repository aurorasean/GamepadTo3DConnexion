namespace GamePad3DConnexion.Settings
{
    public class SettingKeyValue
    {
        public string Name { get; set; }

        public bool MouseX { get; set; }

        public bool MouseY { get; set; }

        public bool MouseW { get; set; }

        public bool LeftClick { get; set; }

        public bool RightClick { get; set; }

        public decimal Multiplier { get; set; } = 1;

        public string Value { get; set; }

        public bool Ctrl { get; set; } = true;

        public bool Shift { get; set; }

        public bool Alt { get; set; }

        public bool IsKeySend { get; set; }

        public string KeyChar { get; set; }

        public bool MiddleClick { get; set; }

        public bool DisabledAxis { get; set; }
    }
}