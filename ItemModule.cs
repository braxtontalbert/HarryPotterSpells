using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{

    public class MyItemModule : ItemModule
    {
        public float spellSpeed;
        public bool magicEffect;
        public float expelliarmusPower;
        public override void OnItemLoaded(Item item)
        {
            base.OnItemLoaded(item);
            item.gameObject.AddComponent<MyWeaponComponent>().Setup(spellSpeed,magicEffect,expelliarmusPower);
        }

    }

}
