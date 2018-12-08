using SharpDX.DirectInput;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace GamePad3DConnexion
{
    public class JoyStickHelper
    {
        private readonly string lockObj = "";

        private readonly List<string> penKeyboardMouseName = new List<string>
        {
            "mouse", "keyboard", "wacom"
        };

        private bool aquired = false;
        private Timer aTimer;
        private VectorRotator CurrentVectorRotator;
        private DirectInput dinput;

        public JoyStickHelper(int pollInterval)
        {
            aTimer = new Timer();
            aTimer.Elapsed += ATimer_Elapsed;
            aTimer.Interval = pollInterval;
            dinput = new DirectInput();
        }

        public delegate void JoyStickConnected(object sender, string joyStickName);

        public delegate void JoyStickDisconnected(object sender, string joyStickName);

        public delegate void JoyStickInvalid(object sender, string joyStickName);

        public delegate void PollDataUpdatedState(object sender, VectorRotationList message);

        public event JoyStickConnected OnJoyStickConnected;

        public event JoyStickDisconnected OnJoyStickDisconnected;

        public event JoyStickInvalid OnJoyStickInvalid;

        public event PollDataUpdatedState OnPollDataUpdatedState;

        public Joystick Joystick { get; set; }
        public string JoyStickName { get; set; }

        public List<string> GetJoyStickNames()
        {
            DirectInput dinput = new DirectInput();
            List<string> listDevices = dinput.GetDevices().Select(x => x.InstanceName).ToList();
            List<string> toRemove = new List<string>();
            foreach (string device in listDevices)
            {
                foreach (string pkmn in penKeyboardMouseName)
                {
                    if (device.ToLower().Contains(pkmn))
                    {
                        toRemove.Add(device);
                    }
                }
            }
            foreach (string remove in toRemove)
            {
                listDevices.Remove(remove);
            }
            return listDevices;
        }

        public VectorRotationList GetJoyStickVectorRoationList()
        {
            return GetJoyStateAndMessage();
        }

        public void PollJoyStick()
        {
            VectorRotationList message = GetJoyStateAndMessage();

            OnPollDataUpdatedState?.Invoke(this, message);
        }

        public void SetJoyStick(string joyStickName)
        {
            if (aTimer.Enabled)
            {
                aTimer.Stop();
            }
            aquired = false;
            DeviceInstance firstJoy = dinput.GetDevices().FirstOrDefault(x => x.InstanceName == joyStickName);
            if (firstJoy != null)
            {
                CurrentVectorRotator = null;
                Joystick = new Joystick(dinput, firstJoy.InstanceGuid);
                JoyStickName = firstJoy.InstanceName;
                OnJoyStickConnected?.Invoke(this, firstJoy.InstanceName);
                StartTimer();
            }
        }

        public void StartTimer()
        {
            aTimer.Enabled = true;
            aTimer.Start();
        }

        public void StopTimer()
        {
            if (aTimer != null)
            {
                aTimer.Stop();
            }
        }

        private void ATimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Console.WriteLine($"{DateTime.Now} : Timer elapsed");
            PollJoyStick();
        }

        private VectorRotationList CreateMessage(VectorRotator vectorRotatorDiff, JoystickState state, string joyStickName)
        {
            VectorRotationList vectorRotationList = new VectorRotationList()
            {
                JoyStickName = joyStickName
            };

            System.Reflection.PropertyInfo[] properties = vectorRotatorDiff.GetType().GetProperties();

            foreach (System.Reflection.PropertyInfo prop in properties)
            {
                vectorRotationList.VectorNameValues.Add(new VectorNameValue
                {
                    Name = prop.Name,
                    Value = (int)prop.GetValue(vectorRotatorDiff)
                });
            }
            return vectorRotationList;
        }

        private VectorRotationList GetJoyStateAndMessage()
        {
            lock (lockObj)
            {
                try
                {
                    if (!aquired)
                    {
                        Joystick.Acquire();
                        aquired = true;
                    }
                    Joystick.Poll();
                }
                catch
                {
                    aTimer.Stop();
                    OnJoyStickDisconnected?.Invoke(this, JoyStickName);
                    return null;
                }

                try
                {
                    JoystickState state = Joystick.GetCurrentState();
                    VectorRotator stateVr = VectorRotator.GetVectorRotatorFromState(state);
                    if (CurrentVectorRotator == null)
                    {
                        CurrentVectorRotator = stateVr;
                    }
                    VectorRotator vectorRotatorDiff = VectorRotator.CalculateDifference(stateVr, CurrentVectorRotator);
                    CurrentVectorRotator = stateVr;
                    VectorRotationList message = CreateMessage(vectorRotatorDiff, state, JoyStickName);
                    return message;
                }
                catch
                {
                    OnJoyStickInvalid?.Invoke(this, JoyStickName);
                    return null;
                }
            }
        }
    }

    public class VectorNameValue
    {
        public string Name { get; set; }

        public int Value { get; set; }

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }
    }

    public class VectorRotationList
    {
        public List<VectorNameValue> VectorNameValues = new List<VectorNameValue>();
        public string JoyStickName { get; set; }
    }

    public class VectorRotator
    {
        public int RotationX { get; set; } = 0;
        public int RotationY { get; set; } = 0;
        public int RotationZ { get; set; } = 0;
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Z { get; set; } = 0;

        public static VectorRotator CalculateDifference(VectorRotator stateVr, VectorRotator currentVectorRotator)
        {
            return new VectorRotator
            {
                X = currentVectorRotator.X - stateVr.X,
                Y = currentVectorRotator.Y - stateVr.Y,
                Z = currentVectorRotator.Z - stateVr.Z,
                RotationX = currentVectorRotator.RotationX - stateVr.RotationX,
                RotationY = currentVectorRotator.RotationY - stateVr.RotationY,
                RotationZ = currentVectorRotator.RotationZ - stateVr.RotationZ,
            };
        }

        public static VectorRotator GetVectorRotator(List<JoystickUpdate> list)
        {
            VectorRotator vectorRotator = new VectorRotator();
            System.Reflection.PropertyInfo[] props = typeof(VectorRotator).GetProperties();
            foreach (System.Reflection.PropertyInfo prop in props)
            {
                JoystickUpdate? propInList = list.FirstOrDefault(x => x.Offset.ToString() == prop.Name);
                if (propInList.HasValue)
                {
                    prop.SetValue(vectorRotator, propInList.Value.Value);
                }
            }
            return vectorRotator;
        }

        public static VectorRotator GetVectorRotatorFromState(JoystickState state)
        {
            VectorRotator vectorRotator = new VectorRotator
            {
                X = state.X,
                Y = state.Y,
                Z = state.Z,
                RotationX = state.RotationX,
                RotationY = state.RotationY,
                RotationZ = state.RotationZ
            };
            return vectorRotator;
        }
    }
}