using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{

    public class ItemVoiceModule : ItemModule
    {
        public float spellSpeed;
        public bool magicEffect;
        public float expelliarmusPower;
        
        public override void OnItemLoaded(Item item)
        {
            base.OnItemLoaded(item);
            item.gameObject.AddComponent<SpellEntry>().Setup(spellSpeed, magicEffect, expelliarmusPower);
            EventManager.onItemEquip += item1 => { if(item1.Equals(item)) Loader.local.currentlyHeldWands.Add(item1);};
        }

    }

}
