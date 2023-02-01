using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;
using System.Collections;
using UnityEngine.VFX;
using static ThunderRoad.ItemData;

namespace WandSpellss
{
    class SpellDespawn : MonoBehaviour
    {
        Item item;
        //VisualEffect vfx;
        public void Start() {


            item = GetComponent<Item>();
            
            //vfx = item.GetComponentInChildren<VisualEffect>();

        }

        public void OnCollisionEnter(Collision c) {


            if (c.collider.GetComponentInParent<IgnoreCollider>()) { }
            else item.Despawn();
        
        }





        /*IEnumerator StartDespawnEvent() {

            vfx.Stop();
            yield return new WaitForSeconds(3f);

            item.Despawn();
        
        
        }*/
    }
}
