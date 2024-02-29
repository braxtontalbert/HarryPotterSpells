using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    class Incendio : MonoBehaviour
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


        void Update() {


            
            toUpdate.transform.position = wand.flyDirRef.transform.position;
            toUpdate.transform.rotation = wand.flyDirRef.transform.rotation;
            if (Physics.SphereCast(wand.flyDirRef.transform.position, 0.4f, wand.flyDirRef.transform.forward, out RaycastHit hit, float.MaxValue, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {

                if (hit.collider.GetComponentInParent<Creature>() is Creature creature)
                {

                    creature.ragdoll.targetPart.physicBody.rigidBody.AddForce(wand.flyDirRef.transform.forward * creature.ragdoll.totalMass * 2f, ForceMode.Acceleration);


                }


            }


        }
    }

    public class IncendioHandler : Spell
    {
        public static SpellType spellType = SpellType.Raycast;
        public override Spell AddGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        public override void SpawnSpell(Type type, string name, Item wand, float spellSpeed)
        {
            throw new NotImplementedException();
        }

        public override void UpdateSpell(Type type, string name, Item wand)
        {
            if (wand.gameObject.GetComponent(type)) UnityEngine.Object.Destroy(wand.gameObject.GetComponent(type));
            wand.gameObject.AddComponent(type);
        }
        public override void UpdateSpell(Type type, string name, Item wand, String itemType)
        {
            throw new NotImplementedException();
        }
    }
}
