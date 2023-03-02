using System;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    public class Depulso : MonoBehaviour
    {
        private Item item;
        private List<Creature> destabilizedList = new List<Creature>();
        private void Start()
        {
            item = GetComponent<Item>();
            Collider[] colliderArray = Physics.OverlapSphere(item.flyDirRef.position + item.flyDirRef.forward * 2f, 2f);
            Vector3 currentItemPos = item.flyDirRef.transform.position;
            foreach (Collider collider in colliderArray)
            {
                if (collider.GetComponentInParent<Creature>() is Creature creature && !creature.isPlayer && !destabilizedList.Contains(creature))
                {
                    creature.ragdoll.SetState(Ragdoll.State.Destabilized);
                    destabilizedList.Add(creature);
                }
                if (collider.attachedRigidbody is Rigidbody rigidbody && !rigidbody.isKinematic)
                {
                    if (collider.attachedRigidbody.gameObject.layer == GameManager.GetLayer(LayerName.Ragdoll))
                    {
                        RagdollPart ragdollPart = collider.attachedRigidbody.gameObject.GetComponent<RagdollPart>();

                        if (ragdollPart && collider.GetComponentInParent<Creature>() is Creature creature1 && !creature1.isPlayer)
                        {
                            
                            Vector3 direction = ragdollPart.transform.position - currentItemPos;
                            float distance = Vector3.Distance(item.flyDirRef.position, ragdollPart.transform.position);
                            ragdollPart.ragdoll.creature.TryPush(Creature.PushType.Magic,  direction.normalized * 2f,
                                (int)Mathf.Round(Mathf.Lerp(1f, 3f, Mathf.InverseLerp(2f, 0.0f, distance))));
                            ragdollPart.rb.AddForce(direction.normalized * ragdollPart.rb.mass * 8f, ForceMode.Impulse);
                        }
                    }
                    else 
                    {
                        if (collider.attachedRigidbody.GetComponentInParent<Item>())
                        {
                            Vector3 direction = collider.attachedRigidbody.transform.position - currentItemPos;
                            rigidbody.AddForce(direction.normalized * rigidbody.mass * 1.1f, ForceMode.Impulse);
                        }
                    }
                }
            }

            foreach (AudioSource c in item.GetComponentsInChildren<AudioSource>())
            {
                if (c.name == this.GetType().Name)
                {
                    c.Play();
                }
            }
        }
    }
    
    
    public class DepulsoHandler : Spell
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