using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    class Lumos : Spell
    {

        Item item;
        Item wand;
        public static SpellType spellType = SpellType.Shoot;
        internal string command;

        public void SetWand(Item item) {

            this.wand = item;


        }

        void Start() {

            item = GetComponent<Item>();
            LevelModuleScript.local.currentTipper = item;
        }

        void Update() {

            if (item != null)
            {
                item.transform.position = wand.flyDirRef.transform.position;
                item.transform.rotation = wand.flyDirRef.transform.rotation;

            }

        
        }
        public override Spell AddGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }
    }
}
