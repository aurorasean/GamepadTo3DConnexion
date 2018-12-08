using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GamePad3DConnexion.Settings.Controls
{
    public class JoystickApplicationViewModel : ObjectNotifyModel
    {
        private int _ApplicationCommandTimer;
        private string _ApplicationName;
        private bool _ApplicationVectorLock;

        private bool _IsLoaded;

        private JoyStickApplication _joyStickApplication;

        private ObservableCollection<SettingKeyValueViewModel> _SettingKeyValues = new ObservableCollection<SettingKeyValueViewModel>();

        public JoystickApplicationViewModel()
        {
        }

        public int ApplicationCommandTimer
        {
            get => _ApplicationCommandTimer;
            set
            {
                _ApplicationCommandTimer = value;
                OnPropertyChanged(nameof(ApplicationCommandTimer));
            }
        }

        public string ApplicationName
        {
            get => _ApplicationName;
            set
            {
                _ApplicationName = value;
                OnPropertyChanged(nameof(ApplicationName));
            }
        }

        public bool ApplicationVectorLock
        {
            get => _ApplicationVectorLock;
            set
            {
                _ApplicationVectorLock = value;
                OnPropertyChanged(nameof(ApplicationVectorLock));
            }
        }

        public bool IsLoaded
        {
            get => _IsLoaded;
            set
            {
                _IsLoaded = value;
                OnPropertyChanged(nameof(IsLoaded));
            }
        }

        public ObservableCollection<SettingKeyValueViewModel> SettingKeyValues
        {
            get => _SettingKeyValues;
            set
            {
                _SettingKeyValues = value;
                OnPropertyChanged(nameof(SettingKeyValues));
            }
        }

        public void LoadFromSetting(JoyStickApplication joyStickApplication)
        {
            IsLoaded = true;
            ApplicationCommandTimer = joyStickApplication.CommandSendRate;
            _joyStickApplication = joyStickApplication;
            SettingKeyValues.Clear();
            ApplicationVectorLock = joyStickApplication.VectorLock;
            foreach (SettingKeyValue key in _joyStickApplication.SettingKeyValues)
            {
                SettingKeyValues.Add(SettingKeyValueViewModel.GetFrom(key));
            }
        }

        public void LoadNew(string applicationName)
        {
            IsLoaded = false;
            ApplicationCommandTimer = 50;
            _joyStickApplication = new JoyStickApplication
            {
                ApplicationNameType = ApplicationNameTypes.Window,
                CommandSendRate = ApplicationCommandTimer,
                ApplicationName = applicationName,
                SettingKeyValues = GetDefaultSettingAxis()
            };
            foreach (SettingKeyValue key in _joyStickApplication.SettingKeyValues)
            {
                SettingKeyValues.Add(SettingKeyValueViewModel.GetFrom(key));
            }
        }

        public JoyStickApplication SaveCurrent()
        {
            _joyStickApplication.CommandSendRate = ApplicationCommandTimer;
            _joyStickApplication.VectorLock = ApplicationVectorLock;
            _joyStickApplication.SettingKeyValues.Clear();
            foreach (SettingKeyValueViewModel setting in SettingKeyValues)
            {
                _joyStickApplication.SettingKeyValues.Add(setting.GetSetting());
            }
            return _joyStickApplication;
        }

        private List<SettingKeyValue> GetDefaultSettingAxis()
        {
            List<SettingKeyValue> settingKeyValues = new List<SettingKeyValue>();
            List<System.Reflection.PropertyInfo> properties = typeof(VectorRotator).GetProperties().Where(x => x.CanWrite).ToList();
            foreach (System.Reflection.PropertyInfo prop in properties)
            {
                settingKeyValues.Add(new SettingKeyValue
                {
                    Name = prop.Name
                });
            }
            return settingKeyValues;
        }
    }
}