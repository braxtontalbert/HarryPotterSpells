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
        Creature target;
        Item shootItem;
        internal Vector3 startPoint;
        internal Vector3 endPoint;
        internal GameObject parentLocal;
        int castCounter;
        public void Start()
        {
            item = GetComponent<Item>();
            castCounter = 0;

        }


        internal void CastRay()
        {

            castCounter++;
            RaycastHit hit;
            Transform parent;

            if (Physics.Raycast(item.flyDirRef.position, item.flyDirRef.forward, out hit))
            {

                Debug.Log("Did hit.");
                Debug.Log(hit.collider.gameObject.transform.parent.name);

                parent = hit.collider.gameObject.transform.parent;
                parentLocal = parent.gameObject;

                if (parentLocal.GetComponent<Item>() != null) {



                    shootItem = parentLocal.GetComponent<Item>();



                }

                else if (parentLocal.GetComponentInParent<Creature>() != null) {

                    target = parentLocal.GetComponentInParent<Creature>();
                
                
                }
                
                if (shootItem != null && target != null) {

                    shootItem.gameObject.AddComponent<WaddiwassiPerItem>();
                    shootItem.gameObject.GetComponent<WaddiwassiPerItem>().target = target;
                    
                    castCounter = 0;

                }


            }


        }


    }

}