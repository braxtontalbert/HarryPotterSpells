﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using System.Collections;
using UnityEngine.VFX;

namespace WandSpellss
{
    class Expelliarmus : MonoBehaviour
    {
        Item item;
        Item npcItem;
        internal AudioSource source;
        GameObject effect;
        public Vector3 currentPosition;

        internal float power;

        public void Start()
        {
            item = GetComponent<Item>();
        }
        
        //TODO: For Everte Statum
        /*foreach (Rigidbody rigidbody in c.gameObject.GetComponentInParent<Creature>().ragdoll.parts.Select(part => part.physicBody.rigidBody))
        {
            var direction = (c.contacts[0].point - currentPosition).normalized;
            CustomDebug.Debug("Rigidbody name: " + rigidbody.name);
            rigidbody.AddForce(direction * (power), ForceMode.Impulse);
        }*/

        //TODO: Affect how it functions based on which ragdoll part is hit.
        public void OnCollisionEnter(Collision c)
        {

            Item disarmedItem = new Item();
            if (c.gameObject.GetComponentInParent<Creature>() is Creature creature)
            {
                disarmedItem = creature.handRight.grabbedHandle.item;
                creature.handRight.UnGrab(false);
                creature.handLeft.UnGrab(false);

            }

            else if (c.gameObject.GetComponentInParent<Item>() is Item itemIn)
            {

                disarmedItem = item;
                itemIn.mainHandler.otherHand.otherHand.UnGrab(false);

            }

            if (Loader.local.currentlyHeldWands.Count < 2)
            {
                RagdollHand oppositeHand = Loader.local.currentlyHeldWands[0].mainHandler.otherHand;
                Vector3 direction =  oppositeHand.transform.position - disarmedItem.transform.position;
                disarmedItem.physicBody.rigidBody.AddForce(direction.normalized * (disarmedItem.physicBody.mass * 1.35f) * Math.Min(Vector3.Distance(oppositeHand.transform.position,disarmedItem.transform.position), 15f), ForceMode.Impulse);
                disarmedItem.physicBody.rigidBody.AddForce(Vector3.up * (disarmedItem.physicBody.mass * 1.35f) * Math.Min(Vector3.Distance(oppositeHand.transform.position,disarmedItem.transform.position),15f), ForceMode.Impulse);
            }
            

            Loader.local.couroutineManager.StartCustomCoroutine(SpawnSparkEffect(Loader.local.expelliarmusSparks, c.contacts[0].point));

        }
        

        public IEnumerator SpawnSparkEffect(GameObject effect, Vector3 position)
        {

            effect.transform.position = position;
            effect = GameObject.Instantiate(effect);


            effect.GetComponentInChildren<VisualEffect>().Play();

            yield return new WaitForSeconds(3f);

            UnityEngine.GameObject.Destroy(effect);

        }
    }
    public class ExpelliarmusHandler : Spell
    {
        public static SpellType spellType = SpellType.Shoot;
        private float expelliarmusPower = 30f;

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
