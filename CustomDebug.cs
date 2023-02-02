using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WandSpellss
{
    public static class CustomDebug
    {
        public static bool debugOn;
        public static void Debug(object message) {

            if (debugOn)
            {
                UnityEngine.Debug.Log(message);
            }
        
        }
    }
}
