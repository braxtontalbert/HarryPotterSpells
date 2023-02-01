using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using System.Timers;

namespace WandSpellss
{
    class DarkMark : MonoBehaviour
    {

        Item item;
        private Timer aTimer;
        float dissolveVal;
        public void Start() {

            item = GetComponent<Item>();
            dissolveVal = 1;

            foreach (Renderer renderer in item.gameObject.GetComponentsInChildren<Renderer>()) {
                foreach (Material mat in renderer.materials) {
                    mat.SetFloat("_dissolve", dissolveVal);
                }
            
            }

            SetTimer();
        
        }


        void Update() {

            if (dissolveVal > 0)
            {
                dissolveVal -= 0.01f;

                foreach (Renderer renderer in item.gameObject.GetComponentsInChildren<Renderer>())
                {
                    foreach (Material mat in renderer.materials) {

                        mat.SetFloat("_dissolve",dissolveVal);
                    
                    
                    }
                    

                }

            }


            item.gameObject.transform.LookAt(Player.local.transform);
        
        
        }


        private void SetTimer()
        {

            aTimer = new System.Timers.Timer(45000);

            aTimer.Elapsed += OnTimedEvent;

            aTimer.AutoReset = false;
            aTimer.Enabled = true;

        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            item.Despawn();
        }

    }
}
