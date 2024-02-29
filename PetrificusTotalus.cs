using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ThunderRoad;
using UnityEngine;
using System.Collections;
using UnityEngine.VFX;

namespace WandSpellss
{
    class PetrificusTotalus : MonoBehaviour
    {
        Item item;
        Item npcItem;
        GameObject enemy;
        internal AudioSource source;
        public System.Timers.Timer aTimer;
        Creature currentCreature;
        Animator prevAnimator;
        Animator freezeAnimator;
        AudioSource sourceCurrent;
        GameObject effect;

        public static SpellType spellType = SpellType.Shoot;

        public void Start()
        {
            item = GetComponent<Item>();
        }

        public void OnCollisionEnter(Collision c)
        {
            if (c.gameObject.GetComponentInParent<Creature>() is Creature creature) {
                currentCreature = creature;
                Loader.local.couroutineManager.StartCustomCoroutine(Loader.local.couroutineManager.Timer(creature, c));
            }
            Loader.local.couroutineManager.StartCustomCoroutine(Loader.local.couroutineManager.SpawnSparkEffect(Loader.local.petrificusSparks, c.contacts[0].point));
        }
        

    }
    public class PetrificusTotalusHandler : Spell
    {
        public static SpellType spellType = SpellType.Shoot;
        private float expelliarmusPower = 30f;
        //AudioSource sourceCurrent;

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

                    projectile.physicBody.rigidBody.useGravity = false;
                    projectile.physicBody.rigidBody.drag = 0.0f;

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
        public override void UpdateSpell(Type type, string name, Item wand, String itemType)
        {
            throw new NotImplementedException();
        }
    }

}
