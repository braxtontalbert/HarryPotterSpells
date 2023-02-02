using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    class Nox : Spell
    {

        Item wand;
        public static SpellType spellType = SpellType.Tip;
        
        public override Spell AddGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        void Start() {

            if (LevelModuleScript.local.currentTipper is Item item && item.GetComponent<Lumos>())
            {
                foreach (AudioSource c in LevelModuleScript.local.currentWand.GetComponentsInChildren<AudioSource>())
                {

                    switch (c.name)
                    {
                        case "NoxSound":
                            c.Play();
                            break;
                    }
                }

                item.Despawn();
            }
        
        
        }
    }
}
