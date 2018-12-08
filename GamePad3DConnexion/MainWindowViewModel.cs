using GamePad3DConnexion.Settings;
using GamePad3DConnexion.Settings.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace GamePad3DConnexion
{
    public class MainWindowViewModel : ObjectNotifyModel
    {
        private string _ApplicationName;
        private JoystickApplication _Content;
        private bool _IsAddAppEnabled;
        private bool _IsApplicationNameEnabled = true;
        private bool _IsCancelledEnabled;
        private bool _IsDefaultJoyStick;
        private bool _IsLoadDefaultEveryTime;
        private JoystickApplicationViewModel _JoyStickApplicationModel;
        private JoyStickApplicationSetting _joyStickApplicationSetting;
        private ObservableCollection<string> _JoyStickNames = new ObservableCollection<string>();
        private double _left;
        private Visibility _MainWindowVisible = Visibility.Visible;
        private ObservableCollection<string> _PreviousApplications = new ObservableCollection<string>();
        private string _SelectedJoyStick;
        private bool _SelectedJoyStickValid = false;
        private string _SelectedPreviousApplication;
        private bool _ShowInTaskBar = false;
        private string _TextBox1Text;
        private string _TextBox2Text;
        private string _TextBox3Text;
        private string _TextBox4Text;
        private double _top;
        private WindowState _WindowState;
        private JoyStickHelper joyStickHelper;

        public MainWindowViewModel()
        {
            SettingHelper.LoadSetting($@"{nameof(SettingParent)}.json");
            MouseHelper.Initialize();
            Console.WriteLine("mv");
            IsLoadDefaultEveryTime = SettingHelper.Instance.LoadFirstJoyStick;
            joyStickHelper = new JoyStickHelper(SettingHelper.Instance.JoyStickPollRate);
            joyStickHelper.OnPollDataUpdatedState += JoyStickHelper_OnPollDataUpdatedState;
            joyStickHelper.OnJoyStickConnected += JoyStickHelper_OnJoyStickConnected;
            joyStickHelper.OnJoyStickDisconnected += JoyStickHelper_OnJoyStickDisconnected;
            joyStickHelper.OnJoyStickInvalid += JoyStickHelper_OnJoyStickInvalid;
            LoadJoySticks();

            if (SettingHelper.Instance.LoadFirstJoyStick)
            {
                ShowInTaskBar = false;
                MainWindowVisible = Visibility.Hidden;
                JoyStickApplicationSetting first = SettingHelper.Instance.JoyStickApplicationSettings.FirstOrDefault(x => x.IsDefaultJoyStick);
                if (first != null)
                {
                    joyStickHelper.SetJoyStick(first.JoyStickName);
                    SelectedJoyStick = first.JoyStickName;
                }
            }
        }

        public ICommand AddApplicationCommand => new DelegateCommand(AddApplication_Click);

        public string ApplicationName
        {
            get => _ApplicationName;
            set
            {
                _ApplicationName = value;
                OnPropertyChanged(nameof(ApplicationName));
                if (string.IsNullOrEmpty(value))
                {
                    IsAddAppEnabled = false;
                }
                else
                {
                    IsAddAppEnabled = true;
                }
            }
        }

        public ICommand CancelAddCommand => new DelegateCommand(CancelAdd_Click);

        public JoystickApplication Content
        {
            get => _Content;
            set
            {
                _Content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        public bool IsAddAppEnabled
        {
            get => _IsAddAppEnabled && _SelectedJoyStickValid;
            set
            {
                _IsAddAppEnabled = value;
                OnPropertyChanged(nameof(IsAddAppEnabled));
            }
        }

        public bool IsApplicationNameEnabled
        {
            get => _IsApplicationNameEnabled;
            set
            {
                _IsApplicationNameEnabled = value;
                OnPropertyChanged(nameof(IsApplicationNameEnabled));
            }
        }

        public bool IsCancelledEnabled
        {
            get => _IsCancelledEnabled;
            set
            {
                _IsCancelledEnabled = value;
                OnPropertyChanged(nameof(IsCancelledEnabled));
            }
        }

        public bool IsDefaultJoyStick
        {
            get => _IsDefaultJoyStick;
            set
            {
                _IsDefaultJoyStick = value;
                JoyStickApplicationSetting joyStickApp = SettingHelper.Instance.JoyStickApplicationSettings.FirstOrDefault(x => x.JoyStickName == SelectedJoyStick);
                if (value)
                {
                    if (joyStickApp != null)

                    {
                        joyStickApp.IsDefaultJoyStick = true;
                    }
                    List<JoyStickApplicationSetting> otherJoys = SettingHelper.Instance.JoyStickApplicationSettings.Where(x => x.JoyStickName != SelectedJoyStick).ToList();
                    foreach (JoyStickApplicationSetting other in otherJoys)
                    {
                        other.IsDefaultJoyStick = false;
                    }
                }
                else
                {
                    if (joyStickApp != null)
                    {
                        joyStickApp.IsDefaultJoyStick = false;
                    }
                }

                OnPropertyChanged(nameof(IsDefaultJoyStick));
            }
        }

        public bool IsLoadDefaultEveryTime
        {
            get => _IsLoadDefaultEveryTime;
            set
            {
                _IsLoadDefaultEveryTime = value;
                OnPropertyChanged(nameof(IsLoadDefaultEveryTime));
            }
        }

        public List<string> JoySitcks { get; set; } = new List<string>();

        public ObservableCollection<string> JoyStickNames
        {
            get => _JoyStickNames;
            set
            {
                _JoyStickNames = value;
                OnPropertyChanged(nameof(JoyStickNames));
            }
        }

        public ICommand LoadCommand => new DelegateCommand(LoadJoySticksButton_Click);

        public Visibility MainWindowVisible
        {
            get => _MainWindowVisible;
            set
            {
                _MainWindowVisible = value;
                if (View != null)
                {
                    View.Visibility = value;
                }
                OnPropertyChanged(nameof(MainWindowVisible));
            }
        }

        public ObservableCollection<string> PreviousApplications
        {
            get => _PreviousApplications;
            set
            {
                _PreviousApplications = value;
                OnPropertyChanged(nameof(PreviousApplications));
            }
        }

        public ICommand SaveApplicationCommand => new DelegateCommand(SaveApplicationCommand_Click);

        public ICommand SaveCommand => new DelegateCommand(SaveSettingsButton_Click);

        public string SelectedJoyStick
        {
            get => _SelectedJoyStick;
            set
            {
                _SelectedJoyStick = value;
                OnPropertyChanged(nameof(SelectedJoyStick));
                if (string.IsNullOrEmpty(value))
                {
                    _SelectedJoyStickValid = false;
                }
                else
                {
                    _SelectedJoyStickValid = true;
                }
                OnPropertyChanged(nameof(IsAddAppEnabled));
                JoyStickListCombo_SelectionChanged(value);
            }
        }

        public string SelectedPreviousApplication
        {
            get => _SelectedPreviousApplication;
            set
            {
                _SelectedPreviousApplication = value;
                OnPropertyChanged(nameof(SelectedPreviousApplication));
                ApplicationName = value;
                SelectedApplicationChanged(value);
            }
        }

        public bool ShowInTaskBar
        {
            get => _ShowInTaskBar;
            set
            {
                _ShowInTaskBar = value;
                OnPropertyChanged(nameof(ShowInTaskBar));
            }
        }

        public ICommand TaskTrayDoubleClickCommand => new DelegateCommand(TaskTrayDoubleClick);

        public string TextBox1Text
        {
            get => _TextBox1Text;
            set
            {
                _TextBox1Text = value;
                OnPropertyChanged(nameof(TextBox1Text));
            }
        }

        public string TextBox2Text
        {
            get => _TextBox2Text;
            set
            {
                _TextBox2Text = value;
                OnPropertyChanged(nameof(TextBox2Text));
            }
        }

        public string TextBox3Text
        {
            get => _TextBox3Text;
            set
            {
                _TextBox3Text = value;
                OnPropertyChanged(nameof(TextBox3Text));
            }
        }

        public string TextBox4Text
        {
            get => _TextBox4Text;
            set
            {
                _TextBox4Text = value;
                OnPropertyChanged(nameof(TextBox4Text));
            }
        }

        public MainWindow View { get; set; }

        public WindowState WindowState
        {
            get => _WindowState;
            set
            {
                _WindowState = value;
                OnPropertyChanged(nameof(WindowState));
                Window_StateChanged();
            }
        }

        public void SendMouseMessageForName(string joyStickName, string name, int v, JoyStickApplication application)
        {
            if (v != 0)
            {
                SettingKeyValue axisSetting = application.SettingKeyValues.FirstOrDefault(x => x.Name == name);
                if (axisSetting != null && !axisSetting.DisabledAxis)
                {
                    ActionParent action = axisSetting.GetActions(v);
                    foreach (MouseKeyAction first in action.FirstActions)
                    {
                        first.Action.Invoke();
                    }
                    foreach (MouseKeyAction clean in action.CleanUpActions)
                    {
                        clean.Action.Invoke();
                    }
                }
                Thread.Sleep(application.CommandSendRate);
            }
        }

        public void Window_StateChanged()
        {
            switch (_WindowState)
            {
                case WindowState.Maximized:
                case WindowState.Normal:
                    break;

                case WindowState.Minimized:
                    {
                        _top = View.Top;
                        _left = View.Left;
                        View.Visibility = Visibility.Hidden;
                        MainWindowVisible = Visibility.Hidden;
                    }
                    break;
            }
        }

        private void AddApplication_Click()
        {
            IsAddAppEnabled = false;
            IsApplicationNameEnabled = false;
            IsCancelledEnabled = true;
            Content = new JoystickApplication();
            _JoyStickApplicationModel = Content.DataContext as JoystickApplicationViewModel;
            _JoyStickApplicationModel.LoadNew(ApplicationName);
        }

        private void CancelAdd_Click()
        {
            ResetAddApplicationButtons();
        }

        private void JoyStickHelper_OnJoyStickConnected(object sender, string joyStickName)
        {
            TextBox3Text = $"JoyStick:{joyStickName}";
        }

        private void JoyStickHelper_OnJoyStickDisconnected(object sender, string joyStickName)
        {
            TextBox3Text = $"JoyStick:Disconnected, Last:{joyStickName}";
        }

        private void JoyStickHelper_OnJoyStickInvalid(object sender, string joyStickName)
        {
            TextBox3Text = $"JoyStick:Invalid: {joyStickName}";
        }

        private void JoyStickHelper_OnPollDataUpdatedState(object sender, VectorRotationList message)
        {
            try
            {
                Action action = () =>
                {
                    if (message != null)
                    {
                        List<string> vs = new List<string>();
                        foreach (VectorNameValue vec in message.VectorNameValues)
                        {
                            vs.Add($"{vec.Name}: {vec.Value}");
                        }
                        TextBox2Text = $"Status: {Environment.NewLine}{string.Join(Environment.NewLine, vs)}";

                        string currentWindow = WindowHelper.GetActiveWindowTitle();
                        JoyStickApplicationSetting joyStick = SettingHelper.Instance.JoyStickApplicationSettings
                            .FirstOrDefault(x => x.JoyStickName == message.JoyStickName);
                        if (joyStick != null)
                        {
                            JoyStickApplication application = joyStick.JoyStickApplications
                                .FirstOrDefault(x => currentWindow.Contains(x.ApplicationName));
                            if (application != null)
                            {
                                if (application.VectorLock)
                                {
                                    VectorNameValue highestLock = message.VectorNameValues
                                        .Where(x => x.Value != 0).OrderByDescending(x => Math.Abs(x.Value))
                                        .FirstOrDefault();
                                    TextBox4Text = $"Vector Lock: {highestLock.Name}";
                                    SendMouseMessageForName(message.JoyStickName, highestLock.Name, highestLock.Value, application);
                                }
                                else
                                {
                                    foreach (VectorNameValue vector in message.VectorNameValues)
                                    {
                                        SendMouseMessageForName(message.JoyStickName, vector.Name, vector.Value, application);
                                    }
                                }
                            }
                        }
                    }

                    TextBox1Text = $"Current window: {WindowHelper.GetActiveWindowTitle()}";
                };
                Dispatcher.CurrentDispatcher.Invoke(action);
            }
            catch
            {
                //do nothing as this errors when we are closing
            }
        }

        private void JoyStickListCombo_SelectionChanged(string joyStick)
        {
            joyStickHelper.StopTimer();
            if (!string.IsNullOrEmpty(joyStick))
            {
                joyStickHelper.SetJoyStick(joyStick);
                joyStickHelper.StartTimer();
                _joyStickApplicationSetting = SettingHelper.Instance.JoyStickApplicationSettings.FirstOrDefault(x => x.JoyStickName == joyStick);
                IsDefaultJoyStick = _joyStickApplicationSetting.IsDefaultJoyStick;
                LoadPreviousApplicationsForJoyStick();
            }
        }

        private void LoadJoySticks()
        {
            JoyStickNames.Clear();
            JoySitcks = joyStickHelper.GetJoyStickNames();
            foreach (string joy in JoySitcks)
            {
                if (!SettingHelper.Instance.JoyStickApplicationSettings.Any(x => x.JoyStickName == joy))
                {
                    SettingHelper.Instance.JoyStickApplicationSettings.Add(new JoyStickApplicationSetting
                    {
                        JoyStickName = joy
                    });
                }
                JoyStickNames.Add(joy);
            }
            ResetAddApplicationButtons();
        }

        private void LoadJoySticksButton_Click()
        {
            LoadJoySticks();
        }

        private void LoadPreviousApplicationsForJoyStick()
        {
            PreviousApplications.Clear();
            foreach (JoyStickApplication previous in _joyStickApplicationSetting.JoyStickApplications)
            {
                PreviousApplications.Add(previous.ApplicationName);
            }
        }

        private void ResetAddApplicationButtons()
        {
            IsAddAppEnabled = true;
            IsApplicationNameEnabled = true;
            IsCancelledEnabled = false;
            Content = null;
            SelectedPreviousApplication = null;
            ApplicationName = null;
        }

        private void SaveApplicationCommand_Click()
        {
            JoyStickApplication settings = _JoyStickApplicationModel.SaveCurrent();
            JoyStickApplicationSetting firstJoy = SettingHelper.Instance.JoyStickApplicationSettings.FirstOrDefault(x => x.JoyStickName == SelectedJoyStick);
            if (firstJoy != null && firstJoy.JoyStickApplications.Any(x => x.ApplicationName == ApplicationName))
            {
                JoyStickApplication firstApp = firstJoy.JoyStickApplications.First(x => x.ApplicationName == ApplicationName);
                firstApp.CommandSendRate = settings.CommandSendRate;
                firstApp.SettingKeyValues = settings.SettingKeyValues;
            }
            else if (firstJoy != null)
            {
                firstJoy.JoyStickApplications.Add(settings);
            }
            SaveSettings();
            ResetAddApplicationButtons();
        }

        private void SaveSettings()
        {
            SettingHelper.Instance.LoadFirstJoyStick = IsLoadDefaultEveryTime;
            SettingHelper.SaveSetting($@"{nameof(SettingParent)}.json");
        }

        private void SaveSettingsButton_Click()
        {
            SaveSettings();
        }

        private void SelectedApplicationChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                IsAddAppEnabled = false;
                IsApplicationNameEnabled = false;
                IsCancelledEnabled = true;
                Content = new JoystickApplication();
                _JoyStickApplicationModel = Content.DataContext as JoystickApplicationViewModel;
                JoyStickApplication application = _joyStickApplicationSetting.JoyStickApplications.FirstOrDefault(x => x.ApplicationName == value);
                _JoyStickApplicationModel.LoadFromSetting(application);
            }
        }

        private void TaskTrayDoubleClick()
        {
            MainWindowVisible = Visibility.Visible;
            View.Top = _top;
            View.Left = _left;
            View.Activate();
            View.Topmost = true;
            View.Focus();
            View.Topmost = false;
        }
    }
}