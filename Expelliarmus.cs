using System;
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
        

        internal float power;

        public void Start()
        {
            item = GetComponent<Item>();
        }

        public void OnCollisionEnter(Collision c)
        {
            if (c.gameObject.GetComponentInParent<Creature>() is Creature creature)
            {
                creature.handRight.UnGrab(false);



                creature.handLeft.UnGrab(false);


                creature.ragdoll.SetState(Ragdoll.State.Destabilized);
                foreach (Rigidbody rigidbody in c.gameObject.GetComponentInParent<Creature>().ragdoll.parts.Select(part => part.rb))
                {

                    CustomDebug.Debug("Rigidbody name: " + rigidbody.name);
                    rigidbody.AddForce(item.flyDirRef.transform.forward * (power), ForceMode.Impulse);
                }


            }

            else if (c.gameObject.GetComponentInParent<Item>() is Item itemIn) {

                
                itemIn.mainHandler.otherHand.otherHand.UnGrab(false);
                itemIn.mainHandler.otherHand.creature.ragdoll.SetState(Ragdoll.State.Destabilized);
            
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

                    
                    if (projectile.gameObject.GetComponent<Expelliarmus>() is Expelliarmus exp)
                    {
                        exp.power = expelliarmusPower;
                    }

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
            
            catch (NullReferenceException e) { Debug.Log(e.Message);}
        }

        public override void UpdateSpell(Type type, string name, Item wand)
        {
            throw new NotImplementedException();
        }
    }

}
