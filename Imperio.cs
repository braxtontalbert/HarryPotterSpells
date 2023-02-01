using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    public class Imperio : MonoBehaviour
    {
        Item item;
        Creature playerCreature;
        Creature ogCreature;
        internal GameObject parentLocal;
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

                Debug.Log("Did hit.");
                Debug.Log(hit.collider.gameObject.transform.parent.name);

                parent = hit.collider.gameObject.transform.parent;
                parentLocal = parent.gameObject;

                if (parentLocal.GetComponent<Creature>() != null)
                {
                    playerCreature = Player.local.creature;
                    ogCreature = parentLocal.GetComponent<Creature>();
                    Player.local.SetCreature(parentLocal.GetComponent<Creature>());
                    
                    Player.selfCollision = true;
                    Player.local.creature.currentHealth = 30f;
                    Player.local.creature.ragdoll.allowSelfDamage = true;

                    Debug.Log("playerCreature1st: " + playerCreature);
                    Debug.Log("ogCreature: " + ogCreature);
                    Debug.Log("playerCreature2nd: " + Player.local.creature);




                }

            }


        }

        void Update() {
            if (playerCreature != null) {
                if (playerCreature.isKilled) {

                    Player.currentCreature.Kill();

                }

                else if (Player.local.creature.currentHealth <= 1f) {

                    Player.local.creature.OnKillEvent += Creature_OnKillEvent;
                    //Player.local.SetCreature(playerCreature);
                    //Player.local.creature.currentHealth = 50f;

                }
            }
            
        }

        private void Creature_OnKillEvent(CollisionInstance collisionInstance, EventTime eventTime)
        {
            Player.local.SetCreature(playerCreature);
            Player.local.creature.currentHealth = 50;
            Player.selfCollision = false;
            Player.local.creature.ragdoll.allowSelfDamage = false;
        }
    }

}
