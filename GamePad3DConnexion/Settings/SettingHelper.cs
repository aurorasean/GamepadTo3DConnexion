using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;

namespace GamePad3DConnexion.Settings
{
    public static class SettingHelper
    {
        public static SettingParent Instance;

        public static void LoadSetting(string file)
        {
            if (!File.Exists(file))
            {
                Instance = new SettingParent();
                SaveSetting(file);
            }
            else
            {
                string fileContent = File.ReadAllText(file);
                Instance = JsonConvert.DeserializeObject<SettingParent>(fileContent);
            }
        }

        public static void SaveSetting(string file)
        {
            string content = JsonConvert.SerializeObject(Instance, Formatting.Indented, new JsonConverter[] { new StringEnumConverter() });
            File.WriteAllText(file, content);
        }
    }
}