using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using UnityEngine.VFX;

namespace WandSpellss
{
    public class Coroutines : MonoBehaviour
    {
        public Coroutines local;
        private void Start()
        {
            local = this;
        }

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
        
        //PETERIFUCUS TOTALLUS COUROUTINES
        public IEnumerator SpawnSparkEffect(GameObject effect, Vector3 position)
        {
            effect.transform.position = position;
            effect = GameObject.Instantiate(effect);


            effect.GetComponentInChildren<VisualEffect>().Play();

            yield return new WaitForSeconds(3f);

            UnityEngine.GameObject.Destroy(effect);
        }

        public bool CounterCurse()
        {

            int returnVal = 0;
            
            returnVal = UnityEngine.Random.Range(1, 101);
            if (returnVal % 5 == 0)
            {
                return true;
            }
            return false;
        }

        public IEnumerator Timer(Creature creature, Collision c)
        {
            bool started = true;
            
            creature.ragdoll.SetState(Ragdoll.State.Frozen);
            creature.brain.instance.Stop();
            while (started)
            {
                if (creature.isKilled)
                {
                    break;
                } 
                bool canStart = true;
                int returnVal = UnityEngine.Random.Range(1, 101);
                if (returnVal % 5 == 0)
                {
                    canStart = false;
                }
                else canStart = true;

                yield return new WaitForSeconds(5f);
                started = canStart;
            }
            if(!creature.isKilled) creature.ragdoll.SetState(Ragdoll.State.Destabilized);
            if(!creature.isKilled) creature.brain.instance.Start();
        }
        
        
        public IEnumerator ImperioCounterCurse(Creature creature, int factionOriginal)
        {
            bool started = true;
            
            while (started)
            {
                if (creature.isKilled)
                {
                    break;
                } 
                bool canStart = true;
                int returnVal = UnityEngine.Random.Range(1, 101);
                if (returnVal == 67)
                {
                    canStart = false;
                }
                else canStart = true;

                yield return new WaitForSeconds(5f);
                started = canStart;
            }
            creature.SetFaction(factionOriginal);
            if(!creature.isKilled) creature.brain.Load(creature.brain.instance.id);
        }
        
    }
}
