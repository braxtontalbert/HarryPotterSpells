using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    public class Aguamenti : MonoBehaviour
    {
        Item item;
        Item npcItem;
        public void Start()
        {
            item = GetComponent<Item>();




        }

        public void OnParticleCollision(Collision c) {

            //c.gameObject.GetComponentInChildren<EffectParticle>().Despawn();
        
        
        }

    }

}




