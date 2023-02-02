using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
    class sempraDespawn : MonoBehaviour
    {
        Item item;
        public void Start()
        {

           item = GetComponent<Item>();

        }
        public void OnCollisionEnter(Collision c)
        {
            item.Despawn();
        }
    }
}
