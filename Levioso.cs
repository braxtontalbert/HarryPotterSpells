using System;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    public class Levioso : MonoBehaviour
    {
        private GameObject leviosoUpdate;
        public void Start()
        {
            leviosoUpdate = new GameObject();
        }

        private void OnCollisionEnter(Collision collision)
        {
            Rigidbody rigid = collision.gameObject.GetComponent<Rigidbody>();

            leviosoUpdate.AddComponent<LeviosoUpdate>();

        }
    }

    public class LeviosoUpdate : MonoBehaviour
    {
        private Rigidbody rigidbody;
        
        public void Setup()
        {
        }

        void Update()
        {
            
        }
    }

    public class LeviosoHandler : Spell
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