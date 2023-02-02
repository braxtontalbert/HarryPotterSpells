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
    class Expelliarmus : Spell
    {
        Item item;
        Item npcItem;
        internal AudioSource source;
        GameObject effect;
        public static SpellType spellType = SpellType.Shoot;

        internal float power;

        public void Start()
        {
            item = GetComponent<Item>();
        }

        public override Spell AddGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
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

            LevelModuleScript.local.couroutineManager.StartCustomCoroutine(SpawnSparkEffect(LevelModuleScript.local.expelliarmusSparks, c.contacts[0].point));

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

}
