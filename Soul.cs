using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{

    class Soul : MonoBehaviour
    {

        internal float maxSoul = 100f;

        internal float currentSoul;


        public void Start() {

            currentSoul = maxSoul;
        
        }

        public float DivideSoul()
        {
            currentSoul = currentSoul / 2;

            return currentSoul;

        }

        

    }
}
