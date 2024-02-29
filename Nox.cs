using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    class Nox : MonoBehaviour
    {

        Item wand;
        public static SpellType spellType = SpellType.Raycast;

        void Start()
        {
            if (Loader.local.currentTippers.Count > 0)
            {
                foreach (Item currentTipper in Loader.local.currentTippers)
                {
                    Loader.local.currentTippers.Remove(currentTipper);
                    foreach (Item wand in Loader.local.currentlyHeldWands)
                    {
                        if (currentTipper is Item item && item.GetComponent<Lumos>())
                        {
                            foreach (AudioSource c in wand.GetComponentsInChildren<AudioSource>())
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
        }
    }

    public class NoxHandler : Spell
    {
        public static SpellType spellType = SpellType.Raycast;
        public override Spell AddGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        public override void SpawnSpell(Type type, string name, Item wand, float spellSpeed)
        {
            throw new NotImplementedException();
        }

        public override void UpdateSpell(Type type, string name, Item wand)
        {
            if (wand.gameObject.GetComponent(type)) UnityEngine.Object.Destroy(wand.gameObject.GetComponent(type));
            wand.gameObject.AddComponent(type);
        }
        public override void UpdateSpell(Type type, string name, Item wand, String itemType)
        {
            throw new NotImplementedException();
        }
    }
}
