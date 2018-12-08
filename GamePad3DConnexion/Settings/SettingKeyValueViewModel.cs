namespace GamePad3DConnexion.Settings
{
    public class SettingKeyValueViewModel : ObjectNotifyModel
    {
        public static SettingKeyValueViewModel GetFrom(SettingKeyValue value)
        {
            return new SettingKeyValueViewModel
            {
                _Alt = value.Alt,
                _Ctrl = value.Ctrl,
                _MouseX = value.MouseX,
                _MouseY = value.MouseY,
                _MouseW = value.MouseW,
                _Multiplier = value.Multiplier,
                _Name = value.Name,
                _Shift = value.Shift,
                _KeyChar = value.KeyChar,
                _LeftClick = value.LeftClick,
                _RightClick = value.RightClick,
                _MiddleClick = value.MiddleClick,
                _DisabledAxis = value.DisabledAxis,
            };
        }

        public SettingKeyValue GetSetting()
        {
            return new SettingKeyValue
            {
                Alt = Alt,
                Shift = Shift,
                Name = Name,
                Multiplier = Multiplier,
                MouseX = MouseX,
                MouseY = MouseY,
                Ctrl = Ctrl,
                KeyChar = KeyChar,
                MouseW = MouseW,
                LeftClick = LeftClick,
                RightClick = RightClick,
                MiddleClick = MiddleClick,
                DisabledAxis = DisabledAxis,
                IsKeySend = !string.IsNullOrEmpty(KeyChar)
            };
        }

        private string _Name;

        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(SettingKeyValueViewModel.Name));
            }
        }

        private bool _MouseW;

        public bool MouseW
        {
            get => _MouseW;
            set
            {
                if (value)
                {
                    _MouseW = value;
                    _MouseY = !_MouseW;
                    _MouseX = !_MouseW;
                    OnPropertyChanged(nameof(MouseY));
                    OnPropertyChanged(nameof(MouseX));
                    OnPropertyChanged(nameof(MouseW));
                }
            }
        }

        private bool _MouseX;

        public bool MouseX
        {
            get => _MouseX;
            set
            {
                if (value)
                {
                    _MouseX = value;
                    _MouseY = !_MouseX;
                    _MouseW = !_MouseX;
                    OnPropertyChanged(nameof(MouseY));
                    OnPropertyChanged(nameof(MouseW));
                    OnPropertyChanged(nameof(MouseX));
                }
            }
        }

        private bool _MouseY;

        public bool MouseY
        {
            get => _MouseY;
            set
            {
                if (value)
                {
                    _MouseY = value;
                    _MouseX = !_MouseY;
                    _MouseW = !_MouseY;
                    OnPropertyChanged(nameof(MouseW));
                    OnPropertyChanged(nameof(MouseX));
                    OnPropertyChanged(nameof(MouseY));
                }
            }
        }

        private decimal _Multiplier = 1;

        public decimal Multiplier
        {
            get => _Multiplier;
            set
            {
                _Multiplier = value;
                OnPropertyChanged(nameof(Multiplier));
            }
        }

        private bool _Ctrl;

        public bool Ctrl
        {
            get => _Ctrl;
            set
            {
                _Ctrl = value;
                OnPropertyChanged(nameof(Ctrl));
            }
        }

        private bool _Shift;

        public bool Shift
        {
            get => _Shift;
            set
            {
                _Shift = value;
                OnPropertyChanged(nameof(Shift));
            }
        }

        private bool _Alt;

        public bool Alt
        {
            get => _Alt;
            set
            {
                _Alt = value;
                OnPropertyChanged(nameof(Alt));
            }
        }

        private string _KeyChar;

        public string KeyChar
        {
            get => _KeyChar;
            set
            {
                _KeyChar = value;
                OnPropertyChanged(nameof(KeyChar));
            }
        }

        private bool _LeftClick;

        public bool LeftClick
        {
            get => _LeftClick;
            set
            {
                _LeftClick = value;
                OnPropertyChanged(nameof(LeftClick));
            }
        }

        private bool _RightClick;

        public bool RightClick
        {
            get => _RightClick;
            set
            {
                _RightClick = value;
                OnPropertyChanged(nameof(RightClick));
            }
        }

        private bool _MiddleClick;

        public bool MiddleClick
        {
            get => _MiddleClick;
            set
            {
                _MiddleClick = value;
                OnPropertyChanged(nameof(MiddleClick));
            }
        }

        private bool _DisabledAxis;

        public bool DisabledAxis
        {
            get => _DisabledAxis;
            set
            {
                _DisabledAxis = value;
                OnPropertyChanged(nameof(DisabledAxis));
            }
        }
    }
}