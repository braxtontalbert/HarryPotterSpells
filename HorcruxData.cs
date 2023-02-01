using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
    class HorcruxData : ItemModule
    {

        public override void OnItemLoaded(Item item)
        {
            base.OnItemLoaded(item);
            if (item.gameObject.GetComponent<Horcrux>() == null)
            {
                item.gameObject.AddComponent<Horcrux>();
            }
        }


    }
}
