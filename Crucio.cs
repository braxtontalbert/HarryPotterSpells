using System;
using System.Collections.Generic;
using System.Linq;
using ThunderRoad;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

namespace WandSpellss
{
    public class Crucio: MonoBehaviour
    {
        private Item item;
        private VisualEffect vfx;
        private Creature hitCreature;
        private float distance = 0f;
        private Transform pos1;
        private Transform pos2;
        private Transform pos3;
        private Transform pos4;
        private List<Transform> positions;
        private bool starting = false;
        private void Start()
        {
            item = GetComponent<Item>();
            vfx = Instantiate(Loader.local.crucioEffect.GetComponentInChildren<VisualEffect>());

            CastRay();
        }

        void SetStartEndBezier()
        {
            starting = true;
        }

        private void Update()
        {
            if (starting)
            {
                vfx.SetVector3("transform1_position", item.transform.position);
                vfx.SetVector3("transform2_position", (item.transform.position + hitCreature.ragdoll.targetPart.transform.position) / 2);
                vfx.SetVector3("transform3_position", (item.transform.position + hitCreature.ragdoll.targetPart.transform.position) / 2);
                vfx.SetVector3("transform4_position", hitCreature.ragdoll.targetPart.transform.position);
            }
        }

        internal void CastRay() {
            RaycastHit hit;
            if (Physics.Raycast(item.flyDirRef.transform.position, item.flyDirRef.transform.forward, out hit, float.MaxValue, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                try
                {
                    hitCreature = hit.collider.GetComponentInParent<Locomotion>().creature;
                }

                catch (NullReferenceException e)
                {
                    Debug.Log(e);
                }

                if (hitCreature)
                {
                    distance = Mathf.Abs(Vector3.Distance(hitCreature.ragdoll.targetPart.transform.position,
                        item.transform.position));
                    SetStartEndBezier();
                    vfx.Play();
                }
            }
            

        }
    }

    public class CrucioHandler : Spell
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
    }
}