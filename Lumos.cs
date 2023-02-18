using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    class Lumos : MonoBehaviour
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
            Loader.local.currentTipper = item;
        }

        void Update() {

            if (item != null)
            {
                item.transform.position = wand.flyDirRef.transform.position;
                item.transform.rotation = wand.flyDirRef.transform.rotation;

            }

        
        }
    }

    public class LumosHandler : Spell
    {
        public static SpellType spellType = SpellType.Shoot;

        public override Spell AddGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        public override void SpawnSpell(Type type, string name, Item wand, float spellSpeed)
        {
            
            try
            {
                if (!Loader.local.currentTipper)
                {
                    Catalog.GetData<ItemData>(name + "Object")?.SpawnAsync(projectile =>
                    {
                        projectile.gameObject.AddComponent(type);

                        projectile.transform.position = wand.flyDirRef.transform.position;
                        projectile.transform.rotation = wand.flyDirRef.transform.rotation;
                        projectile.IgnoreObjectCollision(wand);
                        projectile.IgnoreRagdollCollision(Player.currentCreature.ragdoll);

                        projectile.Throw();

                        projectile.rb.useGravity = false;
                        projectile.rb.drag = 0.0f;

                        foreach (AudioSource c in wand.GetComponentsInChildren<AudioSource>())
                        {

                            if (c.name == name) c.Play();
                        }

                        if (projectile.GetComponent<Lumos>() is Lumos lumos) lumos.SetWand(wand);
                    });
                }
            }
            catch (NullReferenceException e) { Debug.Log(e.Message); }
        }

        public override void UpdateSpell(Type type, string name, Item wand)
        {
            throw new NotImplementedException();
        }
    }
}
