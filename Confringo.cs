using System;
using UnityEngine;
using ThunderRoad;
using UnityEngine.VFX;

namespace WandSpellss
{
    public class Confringo : MonoBehaviour
    {
        private Item item;
        private VisualEffect vfx;
        public Item wand;
        private void Start()
        {
            item = GetComponent<Item>();

            vfx = GetComponentInChildren<VisualEffect>();
            vfx.Play();
            Debug.Log(vfx);
        }

        private void Update()
        {
            if(vfx && wand) vfx.SetVector3("bounds",wand.transform.position);
        }
    }

    public class ConfringoHandler : Spell
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
                Catalog.GetData<ItemData>(name + "Object")?.SpawnAsync(projectile =>
                {
                    Debug.Log("Confringo Spawned");
                    projectile.gameObject.AddComponent(type);

                    projectile.transform.position = wand.flyDirRef.transform.position;
                    projectile.transform.rotation = wand.flyDirRef.transform.rotation;
                    projectile.IgnoreObjectCollision(wand);
                    projectile.IgnoreRagdollCollision(Player.currentCreature.ragdoll);

                    projectile.Throw();

                    projectile.physicBody.rigidBody.useGravity = false;
                    projectile.physicBody.rigidBody.drag = 0.0f;

                    foreach (AudioSource c in wand.GetComponentsInChildren<AudioSource>())
                    {

                        if (c.name == name) c.Play();
                    }

                    if (projectile.gameObject.GetComponent<Confringo>() is Confringo conf) conf.wand = wand; 

                    projectile.GetComponent<Rigidbody>().AddForce(wand.flyDirRef.forward * spellSpeed, ForceMode.Impulse);
                    projectile.gameObject.AddComponent<SpellDespawn>();
                });
            }
            catch (NullReferenceException e) { Debug.Log(e.Message); }
        }

        public override void UpdateSpell(Type type, string name, Item wand)
        {
            throw new NotImplementedException();
        }
    }
}