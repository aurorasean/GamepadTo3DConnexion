using System;

namespace GamePad3DConnexion.Settings
{
    public static class SettingKeyValueExtension
    {
        public static ActionParent GetActions(this SettingKeyValue settingKeyValue, int v)
        {
            ActionParent actionParent = new ActionParent();

            #region Modifiers

            if (settingKeyValue.Alt)
            {
                actionParent.FirstActions.Add(new MouseKeyAction
                {
                    Action = new Action(() =>
                    {
                        MouseHelper.SendKeyBoardDown(KeyBoardValues.Alt);
                    })
                });
                actionParent.CleanUpActions.Add(new MouseKeyAction
                {
                    Action = new Action(() =>
                    {
                        MouseHelper.SendKeyBoardUp(KeyBoardValues.Alt);
                    })
                });
            }
            if (settingKeyValue.Ctrl)
            {
                actionParent.FirstActions.Add(new MouseKeyAction
                {
                    Action = new Action(() =>
                    {
                        MouseHelper.SendKeyBoardDown(KeyBoardValues.Ctrl);
                    })
                });
                actionParent.CleanUpActions.Add(new MouseKeyAction
                {
                    Action = new Action(() =>
                    {
                        MouseHelper.SendKeyBoardUp(KeyBoardValues.Ctrl);
                    })
                });
            }
            if (settingKeyValue.Shift)
            {
                actionParent.FirstActions.Add(new MouseKeyAction
                {
                    Action = new Action(() =>
                    {
                        MouseHelper.SendKeyBoardDown(KeyBoardValues.Shift);
                    })
                });
                actionParent.CleanUpActions.Add(new MouseKeyAction
                {
                    Action = new Action(() =>
                    {
                        MouseHelper.SendKeyBoardUp(KeyBoardValues.Shift);
                    })
                });
            }

            #endregion Modifiers

            if (settingKeyValue.IsKeySend)
            {
                //don't support this yet, this is for buttons on gamepad
            }
            else
            {
                if (settingKeyValue.LeftClick || settingKeyValue.RightClick || settingKeyValue.MiddleClick)
                {
                    System.Windows.Point currentMousePos = MouseHelper.GetCursorPosition();
                    System.Windows.Point newMousePos = GetNewMousePosition(settingKeyValue, v);
                    System.Windows.Point afterMouse = GetAfterMousePosition(settingKeyValue, v);
                    MouseButtons mouseButton = settingKeyValue.LeftClick ? MouseButtons.Left : settingKeyValue.RightClick ? MouseButtons.Right : settingKeyValue.MiddleClick ? MouseButtons.Middle : MouseButtons.Left;

                    actionParent.FirstActions.Add(new MouseKeyAction
                    {
                        Action = new Action(() =>
                        {
                            Console.WriteLine($"SendMouseEvent {mouseButton} {MouseAction.Down} X:{(int)currentMousePos.X} Y:{(int)currentMousePos.Y}");
                            MouseHelper.SendMouseEvent(mouseButton, MouseAction.Down, (int)currentMousePos.X, (int)currentMousePos.Y, 0);
                        })
                    });
                    actionParent.FirstActions.Add(new MouseKeyAction
                    {
                        Action = new Action(() =>
                        {
                            Console.WriteLine($"SetCursorPosition X:{(int)newMousePos.X} Y:{(int)newMousePos.Y}");
                            MouseHelper.SetCursorPosition(newMousePos.X, newMousePos.Y);
                        })
                    });
                    actionParent.FirstActions.Add(new MouseKeyAction
                    {
                        Action = new Action(() =>
                        {
                            Console.WriteLine($"SendMouseEvent {mouseButton} {MouseAction.Up} X:{(int)currentMousePos.X} Y:{(int)currentMousePos.Y}");
                            MouseHelper.SendMouseEvent(mouseButton, MouseAction.Up, (int)currentMousePos.X, (int)currentMousePos.Y, 0);
                        })
                    });
                    actionParent.CleanUpActions.Add(new MouseKeyAction
                    {
                        Action = new Action(() =>
                        {
                            Console.WriteLine($"SetCursorPosition X:{(int)currentMousePos.X} Y:{(int)currentMousePos.Y}");
                            MouseHelper.SetCursorPosition(currentMousePos.X, currentMousePos.Y);
                        })
                    });
                }
                else
                {
                    #region Move Mouse

                    bool moveMouse = true;

                    System.Windows.Point currentMousePos = GetNewMousePosition(settingKeyValue, v);

                    if (settingKeyValue.MouseW)
                    {
                        moveMouse = false;
                        int newV = (int)(v * settingKeyValue.Multiplier);
                        if (v > 0)
                        {
                            actionParent.FirstActions.Add(new MouseKeyAction
                            {
                                Action = new Action(() =>
                                {
                                    MouseHelper.SendMouseEvent(MouseButtons.Wheel, MouseAction.Up, (int)currentMousePos.X, (int)currentMousePos.Y, newV);
                                })
                            });
                        }
                        else
                        {
                            actionParent.FirstActions.Add(new MouseKeyAction
                            {
                                Action = new Action(() =>
                                {
                                    MouseHelper.SendMouseEvent(MouseButtons.Wheel, MouseAction.Down, (int)currentMousePos.X, (int)currentMousePos.Y, newV);
                                })
                            });
                        }
                    }
                    if (moveMouse)
                    {
                        MouseHelper.SetCursorPosition(currentMousePos.X, currentMousePos.Y);
                    }

                    #endregion Move Mouse
                }
            }
            return actionParent;
        }

        private static System.Windows.Point GetNewMousePosition(SettingKeyValue settingKeyValue, int v)
        {
            System.Windows.Point currentMousePos = MouseHelper.GetCursorPosition();
            if (settingKeyValue.MouseX)
            {
                currentMousePos.X = currentMousePos.X + (int)(v * settingKeyValue.Multiplier);
            }
            else if (settingKeyValue.MouseY)
            {
                currentMousePos.Y = currentMousePos.Y + (int)(v * settingKeyValue.Multiplier);
            }

            return currentMousePos;
        }

        private static System.Windows.Point GetAfterMousePosition(SettingKeyValue settingKeyValue, int v)
        {
            System.Windows.Point currentMousePos = MouseHelper.GetCursorPosition();
            if (settingKeyValue.MouseX)
            {
                currentMousePos.X = currentMousePos.X + (int)(v * settingKeyValue.Multiplier);
            }
            else if (settingKeyValue.MouseY)
            {
                currentMousePos.Y = currentMousePos.Y + (int)(v * settingKeyValue.Multiplier);
            }

            return currentMousePos;
        }
    }
}