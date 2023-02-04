using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    class Incendio : Spell
    {
        public static SpellType spellType = SpellType.Raycast;

        Item wand;
        GameObject effect;
        GameObject toUpdate;

        void Start() {

            wand = GetComponent<Item>();
            effect = Loader.local.incendioEffect;

            SpawnEffect(effect);
        }

        public void SpawnEffect(GameObject effect) {

            effect.transform.position = wand.flyDirRef.transform.position;
            effect.transform.rotation = wand.flyDirRef.transform.rotation;
            toUpdate = GameObject.Instantiate(effect);

            toUpdate.GetComponentInChildren<ParticleSystem>().Play();

            

        }
        public override Spell AddGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }


        void Update() {


            
            toUpdate.transform.position = wand.flyDirRef.transform.position;
            toUpdate.transform.rotation = wand.flyDirRef.transform.rotation;
            if (Physics.SphereCast(wand.flyDirRef.transform.position, 0.4f, wand.flyDirRef.transform.forward, out RaycastHit hit, float.MaxValue, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {

                if (hit.collider.GetComponentInParent<Creature>() is Creature creature)
                {

                    creature.ragdoll.targetPart.rb.AddForce(wand.flyDirRef.transform.forward * creature.ragdoll.totalMass * 2f, ForceMode.Acceleration);


                }


            }


        }
    }
}
