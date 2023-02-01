using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    public class Waddiwassi : MonoBehaviour
    {
        Item item;
        public void Start()
        {
            item = GetComponent<Item>();
            CastRay();
        }

        internal void CastRay()
        {
            RaycastHit hit;
            Transform parent;

            if (Physics.Raycast(item.flyDirRef.transform.position, item.flyDirRef.transform.forward, out hit, float.MaxValue, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {

                CustomDebug.Debug("Did hit.");
                CustomDebug.Debug(hit.collider.gameObject.transform.parent.name);

                if(hit.collider.GetComponentInParent<Creature>() is Creature creature)
                {
                    Item itemToForce = (Item)Item.allActive.Where(item => item != null && Vector3.Distance(item.transform.position, creature.transform.position) < 2f)?.First();

                    Vector3 direction = (creature.transform.position - itemToForce.transform.position);
                    itemToForce.rb.AddForce(direction * 8f, ForceMode.Impulse);
                }

            }
        }
    }

}