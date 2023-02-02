using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using System.Collections;

namespace WandSpellss
{
    class WingardiumLeviosa : Spell
    {
        Item item;
        Item wand;
        internal bool canLift;
        internal GameObject parentLocal;
        Vector3 radius;
        Vector3 direction;
        float distance;
        Rigidbody currentRigidbody;
        Creature currentCreature;

        public static SpellType spellType = SpellType.Raycast;

       

        public void Start()
        {
            item = GetComponent<Item>();
            canLift = false;
            item.OnHeldActionEvent += Item_OnHeldActionEvent;

            CastRay();
        }


        private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
        {
            if (action == Interactable.Action.AlternateUseStart && canLift == true) {

                canLift = false;
                currentRigidbody = null;
            }
        }

        internal void CastRay()
        {

            RaycastHit hit;
            Transform parent;

            if (Physics.Raycast(item.flyDirRef.transform.position,item.flyDirRef.transform.forward, out hit, float.MaxValue, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                CustomDebug.Debug("Did hit.");
                CustomDebug.Debug(hit.collider.gameObject.transform.parent.name);

                parent = hit.collider.gameObject.transform.parent;
                parentLocal = parent.gameObject;
                

                if (parentLocal.gameObject.GetComponent<Item>() is Item item1)
                {
                    currentRigidbody = item1.GetComponent<Rigidbody>();
                    canLift = true;
                    distance = Math.Abs(Vector3.Distance(parentLocal.transform.position, item.flyDirRef.position));

                }
                else if (parentLocal.gameObject.GetComponentInParent<Item>() is Item item2)
                {
                    
                    currentRigidbody = item2.GetComponent<Rigidbody>();
                    canLift = true;
                    distance = Math.Abs(Vector3.Distance(parentLocal.transform.position, item.flyDirRef.position));

                }
                else if (parentLocal.gameObject.GetComponentInChildren<Item>() is Item item3)
                {
                    currentRigidbody = item3.GetComponent<Rigidbody>();
                    canLift = true;
                    distance = Math.Abs(Vector3.Distance(parentLocal.transform.position, item.flyDirRef.position));

                }
                else if (parentLocal.GetComponentInParent<Creature>() is Creature creature1) {

                    currentCreature = creature1;
                    currentCreature.ragdoll.SetState(Ragdoll.State.Destabilized);
                    currentRigidbody = parentLocal.GetComponent<Rigidbody>();
                    canLift = true;
                    distance = Math.Abs(Vector3.Distance(parentLocal.transform.position, item.flyDirRef.position));

                }
                else if (parentLocal.GetComponentInChildren<Creature>() is Creature creature2)
                {
                    currentCreature = creature2;
                    currentCreature.ragdoll.SetState(Ragdoll.State.Destabilized);
                    currentRigidbody = parentLocal.GetComponent<Rigidbody>();
                    canLift = true;
                    distance = Math.Abs(Vector3.Distance(parentLocal.transform.position, item.flyDirRef.position));
                }

            }
        }

        void Update()
        {
            direction = item.flyDirRef.forward;

                if (canLift == true)
                {
                    currentRigidbody.velocity = ((item.flyDirRef.position + (direction * distance)) - currentRigidbody.position) * (3f);

                }

        }


        public override Spell AddGameObject(GameObject gameObject)
        {
            return gameObject.AddComponent<WingardiumLeviosa>();
        }


    }

}
