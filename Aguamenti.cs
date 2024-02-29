using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using ThunderRoad.AI.Action;

namespace WandSpellss
{
    public class Aguamenti : MonoBehaviour
    {
        Item item;
        private Item vfx;
        private Creature hitCreature;
        private Rigidbody currentRigidbody;
        private bool startEnd = false;
        private bool startStart = false;
        private ParticleSystemRenderer particleSystemRenderer;
        private ParticleSystem particleSystem;
        private float distance = 0f;
        public void Start()
        {
            item = GetComponent<Item>();
            item.OnHeldActionEvent += Item_OnHeldActionEvent;
            CastRay();
        }
        private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
        {
            if (action == Interactable.Action.AlternateUseStart)
            {
                startEnd = true;
            }
        }
        
        void StartAguamenti()
        {
            Catalog.GetData<ItemData>("AguamentiObject")?.SpawnAsync(projectile =>
            {
                vfx = projectile;
                particleSystem = vfx.GetComponentInChildren<ParticleSystem>();
                particleSystemRenderer = particleSystem.gameObject.GetComponent<ParticleSystemRenderer>();
                startStart = true;
                hitCreature.gameObject.AddComponent<AguamentiFollow>().Setup(projectile,hitCreature);
                if(hitCreature.ragdoll.state != Ragdoll.State.Frozen) hitCreature.ragdoll.SetState(Ragdoll.State.Destabilized);
            });
        }

        void Update()
        {
            if (currentRigidbody)
            {
                Vector3 direction = item.flyDirRef.forward;

                currentRigidbody.velocity =
                    ((item.flyDirRef.position + (direction * distance)) - currentRigidbody.position) * (3f);
            }

            if (startStart)
            {
                float dissolveVal = particleSystemRenderer.material.GetFloat("_disolve");

                if (dissolveVal >= 0)
                {
                    dissolveVal -= 0.01f;
                    particleSystem.gameObject.GetComponent<ParticleSystemRenderer>().material.SetFloat("_disolve", dissolveVal);
                }
                else
                {
                    startStart = false;
                    currentRigidbody = hitCreature.ragdoll.targetPart.physicBody.rigidBody;
                }
            }

            if (startEnd)
            {
                float dissolveVal = particleSystemRenderer.material.GetFloat("_disolve");

                if (dissolveVal < 1)
                {
                    dissolveVal += 0.01f;
                    particleSystem.gameObject.GetComponent<ParticleSystemRenderer>().material.SetFloat("_disolve", dissolveVal);
                }
                else
                {
                    startEnd = false;
                    particleSystem = null;
                    particleSystemRenderer = null;
                    vfx.Despawn();
                    if(hitCreature) Destroy(hitCreature.gameObject.GetComponent<AguamentiFollow>());
                    currentRigidbody = null;
                    hitCreature = null;
                }
            }
        }
        internal void CastRay() {
            RaycastHit hit;
            if (Physics.Raycast(item.flyDirRef.transform.position, item.flyDirRef.transform.forward, out hit, float.MaxValue, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                try
                {
                    hitCreature = hit.collider.GetComponentInParent<Locomotion>().creature;
                    distance = Mathf.Abs(Vector3.Distance(hitCreature.ragdoll.targetPart.transform.position,
                        item.transform.position));
                }

                catch (NullReferenceException e)
                {
                    Debug.Log(e);
                }
                StartAguamenti();
            }
            

        }
    }

    public class AguamentiFollow : MonoBehaviour
    {
        private Item item;
        private Creature creature;
        public void Setup(Item item, Creature creature)
        {
            this.item = item;
            this.creature = creature;
        }

        private void Update()
        {
            if (item && creature)
            {
                item.transform.position = creature.ragdoll.targetPart.transform.position;
            }
        }
    }
    
    public class AguamentiHandler : Spell
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
            if (wand.gameObject.GetComponent(type)) UnityEngine.Object.Destroy(wand.gameObject.GetComponent(type));
            wand.gameObject.AddComponent(type);
        }
    }

}




