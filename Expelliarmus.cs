﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using System.Collections;
using UnityEngine.VFX;
 using Random = UnityEngine.Random;

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
        
        public void OnCollisionEnter(Collision c)
        {

            Item disarmedItem = new Item();
            if (c.gameObject.GetComponentInParent<Creature>() is Creature creature)
            {
                int random = Random.Range(0, 1);
                if (random == 0)
                {
                    disarmedItem = creature.handRight.grabbedHandle.item; 
                    if(creature.handLeft.grabbedHandle) creature.handRight.UnGrab(false);
                    if(creature.handLeft.grabbedHandle) creature.handLeft.UnGrab(false);
                }
                else
                {
                    disarmedItem = creature.handLeft.grabbedHandle.item;
                    if(creature.handLeft.grabbedHandle) creature.handRight.UnGrab(false);
                    if(creature.handLeft.grabbedHandle) creature.handLeft.UnGrab(false);
                }

            }

            else if (c.gameObject.GetComponentInParent<Item>() is Item itemIn)
            {
                disarmedItem = item;
                disarmedItem.physicBody.velocity = new Vector3(0,0,0);
                itemIn.mainHandler.otherHand.UnGrab(false);

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
            effect = Instantiate(effect);
            effect.GetComponentInChildren<VisualEffect>().Play();

            yield return new WaitForSeconds(3f);

            Destroy(effect);
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
