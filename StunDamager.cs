using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    public class StunDamager : MonoBehaviour
    {
        Item item;
        float currentAlpha = 1;
        public void Start()
        {
            item = GetComponent<Item>();
            


        }


        public void OnCollisionEnter(Collision c) {
            if (c.gameObject.GetComponentInParent<Creature>() is Creature creature)
            {
                creature.ragdoll.SetState(Ragdoll.State.Destabilized);
                creature.TryElectrocute(1, 3, true, false);
            }


        }
    }
}


