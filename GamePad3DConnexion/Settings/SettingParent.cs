using System.Collections.Generic;

namespace GamePad3DConnexion.Settings
{
    public class SettingParent
    {
        public bool LoadFirstJoyStick { get; set; } = true;

        public string FirstJoyStringName { get; set; } = "";

        public int JoyStickPollRate { get; set; } = 10;

        public List<JoyStickApplicationSetting> JoyStickApplicationSettings { get; set; } = new List<JoyStickApplicationSetting>();
    }

    public class JoyStickApplicationSetting
    {
        public string JoyStickName { get; set; }

        public bool IsDefaultJoyStick { get; set; }

        public List<JoyStickApplication> JoyStickApplications { get; set; } = new List<JoyStickApplication>();
    }

    public class JoyStickApplication
    {
        public string ApplicationName { get; set; }

        public bool VectorLock { get; set; } = true;

        public ApplicationNameTypes ApplicationNameType { get; set; } = ApplicationNameTypes.Window;

        public int CommandSendRate { get; set; } = 50;

        public List<SettingKeyValue> SettingKeyValues { get; set; } = new List<SettingKeyValue>();
    }

    public enum ApplicationNameTypes
    {
        Window = 1,
        ApplicationExe = 2
    }

    public class ActionKeyValue
    {
        public string Name { get; set; }

        public List<KeyBoardValues> Modifiers { get; set; } = new List<KeyBoardValues>();

        public string MouseDirection { get; set; }
    }
}