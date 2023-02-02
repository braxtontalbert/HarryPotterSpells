using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    public class Coroutines : MonoBehaviour
    {

        public void StartCustomCoroutine(IEnumerator methodIn) {

            StartCoroutine(methodIn);

        }

        public bool startAccio;
        public Item currentWand;
        public RagdollHand currentHandPosition;
        public void StartAccio(Item wand, RagdollHand handPosition) {

            startAccio = true;
            currentWand= wand;
            currentHandPosition = handPosition;
        }

        void Update() {

            if (startAccio) {

                currentWand.rb.useGravity = false;
                float distance = (currentWand.transform.position - currentHandPosition.transform.position).magnitude;
                currentWand.rb.velocity = (currentHandPosition.transform.position - currentWand.transform.position).normalized * 15f;

                if (distance < 0.1f) {

                    if (!currentHandPosition.isGrabbed)
                    {
                        currentHandPosition.Grab(currentWand.mainHandleRight);
                        startAccio = false;
                        currentWand.rb.useGravity = true;
                        currentWand = null;
                        currentHandPosition= null;

                    }
                }

            }
        
        }
        
    }
}
