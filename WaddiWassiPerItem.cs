using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    class WaddiwassiPerItem : MonoBehaviour
    {

        bool cantWaddiwassi;
        Item item;
        internal Creature target;
        internal Item shootItem;

        void Start()
        {

            item = GetComponent<Item>();
            cantWaddiwassi = false;

        }
        void Update()
        {

            if (cantWaddiwassi == false)
            {
                Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y + 0.3f, target.transform.position.z);
                Vector3 direction = targetPos - item.transform.position;

                item.physicBody.rigidBody.AddForce(direction * (10f * item.physicBody.rigidBody.mass),ForceMode.Impulse);

                cantWaddiwassi = true;
                Destroy(this);
            }
        }
    }
}
