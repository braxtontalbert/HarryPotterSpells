using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    class VoiceWithDictation : MonoBehaviour
    {

        DictationWand dictWand;

        Item item;
        void Start() {

            item = GetComponent<Item>();

            item.gameObject.AddComponent<DictationWand>();

            
        
        
        }
    }
}
