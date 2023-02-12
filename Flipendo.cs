using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using static ThunderRoad.HandPoseData;
using System.Collections;


namespace WandSpellss
{
    class Flipendo : Spell
    {
        public static SpellType spellType = SpellType.Shoot;
        Item item;
        public override Spell AddGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        void Start() { 
        
            item = GetComponent<Item>();

        }

        public void OnCollisionEnter(Collision c)
        {
            if (c.gameObject.GetComponentInParent<Creature>() is Creature creature)
            {

                creature.ragdoll.SetState(Ragdoll.State.Destabilized);
                foreach (Rigidbody rigidbody in c.gameObject.GetComponentInParent<Creature>().ragdoll.parts.Select(part => part.rb))
                {

                    CustomDebug.Debug("Rigidbody name: " + rigidbody.name);
                    rigidbody.AddForce(Vector3.up * 30f, ForceMode.Impulse);
                    if (rigidbody.name.Contains("Head"))
                    {
                        rigidbody.AddForce(item.flyDirRef.transform.forward * 10f, ForceMode.Impulse);   
                    }
                    
                }
            }
        }

        IEnumerator FloatingTimer(Rigidbody rb) {

            rb.useGravity = false;
            yield return new WaitForSeconds(2f);

            rb.useGravity = true;
        }
        
        void ExecuteFlipendo() {

        }
    }
}
