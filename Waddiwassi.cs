using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
     class Waddiwassi : Spell
    {
        Item item;
        public static SpellType spellType = SpellType.Raycast;
        public void Start()
        {
            item = GetComponent<Item>();
            CastRay();
        }

        new public void CastRay()
        {
            RaycastHit hit;
            Transform parent;

            if (Physics.Raycast(item.flyDirRef.transform.position, item.flyDirRef.transform.forward, out hit, float.MaxValue, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {

                CustomDebug.Debug("Did hit.");
                CustomDebug.Debug(hit.collider.gameObject.transform.parent.name);
                try
                {
                    if (hit.collider.GetComponentInParent<Creature>() is Creature creature)
                    {
                        Item itemToForce = (Item)Item.allActive.Where(item => item != null && (creature.ragdoll.headPart.transform.position - item.transform.position).magnitude < 10f)?.First();

                        Vector3 direction = (creature.ragdoll.headPart.transform.position - itemToForce.transform.position);
                        itemToForce.rb.AddForce(direction * 10f, ForceMode.Acceleration);
                    }
                }


                catch {

                    CustomDebug.Debug("Did not hit creature");
                }

            }
        }

        public override Spell AddGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }
    }

}