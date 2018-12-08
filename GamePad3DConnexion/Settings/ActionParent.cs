using System;
using System.Collections.Generic;

namespace GamePad3DConnexion.Settings
{
    public class ActionParent
    {
        public List<MouseKeyAction> FirstActions = new List<MouseKeyAction>();
        public List<MouseKeyAction> CleanUpActions = new List<MouseKeyAction>();
    }

    public class MouseKeyAction
    {
        public Action Action { get; set; }
    }
}