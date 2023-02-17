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
        
        public IEnumerator StopLeviate(GameObject go)
        {
            
            Debug.Log(go);
            Debug.Log("Starting levioso timer");
            yield return new WaitForSeconds(10f);
            Debug.Log("Got past timer for levioso");
            UnityEngine.GameObject.Destroy(go);
            
        }

        public bool startAccio;
        public Item currentWand;
        public RagdollHand currentHandPosition;
        public void StartAccio(Item wand, RagdollHand handPosition) {

            startAccio = true;
            currentWand= wand;
            currentHandPosition = handPosition;
        }
        IEnumerator StopLeviate(Rigidbody currentCreature, GameObject leviosoUpdate)
        {
            Debug.Log("Starting levioso timer");
            yield return new WaitForSeconds(10f);
            UnityEngine.GameObject.Destroy(leviosoUpdate);
            
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
