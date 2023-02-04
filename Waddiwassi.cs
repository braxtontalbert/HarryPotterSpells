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

                if (hit.collider.GetComponentInParent<Creature>() is Creature creature)
                {
                    float distance = (creature.ragdoll.headPart.transform.position - item.transform.position).magnitude;
                    List<Item> itemsToForce = Item.allActive.Where(item => item != null && distance < 3f && !Player.currentCreature.equipment.GetAllHolsteredItems().Contains(item))?.ToList();
                    Item itemToForce = itemsToForce[UnityEngine.Random.Range(0, itemsToForce.Count - 1)];

                    Vector3 direction = (creature.ragdoll.headPart.transform.position - itemToForce.transform.position).normalized;
                    itemToForce.rb.AddForce(direction * itemToForce.rb.mass * (10f * distance), ForceMode.Impulse);
                }
            }
        }

        public override Spell AddGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }
    }

}