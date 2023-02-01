using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
    class WingardiumLeviosaJoint : MonoBehaviour
    {

        Item item;
        internal Item wand;
        Item npcItem;
        Creature creature;
        internal Item hitObjectItem;
        internal bool objectIsHovering;
        internal GameObject parentLocal;
        internal Vector3 targetPos;
        FixedJoint joint;

        public void Start()
        {

            item = GetComponent<Item>();

        }

        internal void CastRay()
        {

            RaycastHit hit;
            Transform parent;


            if (Physics.Raycast(item.flyDirRef.position, item.flyDirRef.forward, out hit))
            {

                parent = hit.collider.gameObject.transform.parent;
                parentLocal = parent.gameObject;

                if (parentLocal.GetComponent<Item>() is Item hitItem)
                {
                    Rigidbody rigidBod = wand.gameObject.GetComponent<Rigidbody>();
                    rigidBod.mass = 0.1f;
                    hitItem.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    hitItem.gameObject.AddComponent<FixedJoint>();
                    joint = hitItem.gameObject.GetComponent<FixedJoint>();
                    joint.connectedBody = rigidBod;
                    joint.anchor = hit.transform.position;
                    joint.autoConfigureConnectedAnchor = false;
                    joint.connectedAnchor = wand.flyDirRef.transform.position;

                }


                else if (parentLocal.GetComponentInParent<Item>() is Item hitItem2) {

                    Rigidbody rigidBod = wand.gameObject.GetComponent<Rigidbody>();
                    rigidBod.mass = 0.1f;
                    hitItem2.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    hitItem2.gameObject.AddComponent<FixedJoint>();
                    joint = hitItem2.gameObject.GetComponent<FixedJoint>();
                    joint.connectedBody = rigidBod;
                    joint.anchor = hit.transform.position;
                    joint.autoConfigureConnectedAnchor = false;
                    joint.connectedAnchor = wand.flyDirRef.transform.position;
                    

                }

            }
        }
            
        

    }
}
