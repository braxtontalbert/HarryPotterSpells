using System;
using ThunderRoad;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine.Events;

namespace WandSpellss
{
    public class ItemTracker : ThunderScript
    {
        public static ItemTracker local;
        public bool itemSpawned = false;
        public int expectedAmount = 0;
        public int actualAmount = 0;
        public override void ScriptLoaded(ModManager.ModData modData)
        {
            Debug.Log("LEVEL COROUTINE FOR ITEM TRACKER STARTED");
            local = this;
            EventManager.OnItemBrokenEnd += ItemsBrokeEnd;
            base.ScriptLoaded(modData);
        }

        private void ItemsBrokeEnd(Breakable breakable, PhysicBody[] pieces)
        {
            actualAmount = 0;
            GameObject tracker = new GameObject();
            tracker.AddComponent<TrackPieces>();
            GameObject tempr;
            int temp = 0;
            tempr = GameObject.Instantiate(tracker);
            List<Item> returnList = breakable.subBrokenItems.Concat(breakable.subUnbrokenItems).Distinct().ToList();
            for (int i = 0; i < returnList.Count;i++)
            {
                temp++;
            }
            
            if (tempr.gameObject.GetComponent<TrackPieces>() is TrackPieces tp)
            {
                tp.expectedAmount = temp;
                tp.actualAmount = 0;
            }

            foreach (Item subs in returnList)
            {
                subs.disallowDespawn = true;
                List<Item> others = returnList.Where(item => item != subs).ToList();
                subs.gameObject.GetComponentInParent<Item>().gameObject.AddComponent<BreakableUpdated>().Setup(
                    breakable.linkedItem.data,
                    breakable.linkedItem.transform.position,
                    breakable.linkedItem.transform.rotation, tempr.gameObject.GetComponent<TrackPieces>(), others);
            }
        }
    }
    public class Repairo : MonoBehaviour
    {
        private Item item;

        private void Start()
        {
            item = GetComponent<Item>();
            RayCast();
        }

        void RayCast()
        {
            if (Physics.Raycast(item.flyDirRef.transform.position, item.flyDirRef.transform.forward, out RaycastHit hit,
                    float.MaxValue, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                Debug.Log(hit.collider);
                Item hitItem = hit.collider.gameObject.GetComponentInParent<Item>();
                if (hit.collider.gameObject.GetComponentInParent<BreakableUpdated>() is BreakableUpdated breakable)
                {
                    hitItem.physicBody.rigidBody.useGravity = false;
                    foreach (Collider collider in hitItem.gameObject.GetComponentsInChildren<Collider>())           
                    {                                                                                            
                        Destroy(collider);                                                                       
                    }
                    breakable.SetupLerping(true);
                    foreach (Item item in breakable.otherBreakables)
                    {
                        item.GetComponent<BreakableUpdated>().SetupLerping(true);
                    }
                }
                else if (hit.collider.gameObject.GetComponentInChildren<BreakableUpdated>() is BreakableUpdated
                         breakable1)
                {
                    hitItem.physicBody.rigidBody.useGravity = false;
                    foreach (Collider collider in hitItem.gameObject.GetComponentsInChildren<Collider>())           
                    {                                                                                            
                        Destroy(collider);                                                                       
                    }
                    breakable1.SetupLerping(true);
                    foreach (Item item in breakable1.otherBreakables)
                    {
                        item.GetComponent<BreakableUpdated>().SetupLerping(true);
                    }
                }
            }
        }
    }

    public class TrackPieces : MonoBehaviour
    {
        public int expectedAmount { get; set; }
        public int actualAmount { get; set; }

    }

    public class BreakableUpdated : MonoBehaviour
    {
        private Item item;
        public ItemData itemData;
        private Vector3 position;
        private Quaternion rotation;
        private List<Vector3> positions = new List<Vector3>();
        private List<Quaternion> rotations = new List<Quaternion>();
        private bool startTracking = false;
        public List<Item> otherBreakables;
        
        //lerping

        private bool start;
        private float elapsedTime = 0f;
        private Vector3 startPoint;
        private Vector3 endPoint;
        private int index = 0;
        private TrackPieces tracker;
        private int actualAmount = 0;
        private void Start()
        {
            item = GetComponent<Item>();
            start = false;
            elapsedTime = 0f;
            startTracking = true;
            List<Breakable> breakables = item.GetComponentsInChildren<Breakable>().ToList();
            for (int i = 0; i < breakables.Count; i++)
            {
                Destroy(breakables[i]);
            }
            StartCoroutine(TimerTracker());
        }

        public void Setup(ItemData dataIn, Vector3 pos, Quaternion rot, TrackPieces tracker, List<Item> otherBreakables)
        {
            itemData = dataIn;
            position = pos;
            rotation = rot;
            this.tracker = tracker;
            this.otherBreakables = otherBreakables;
        }

        public void SetupLerping(bool start)
        {
            this.start = start;
        }

        private void Update()
        {
            if (startTracking && !start)
            {
                positions.Add(item.transform.position);
                rotations.Add(item.transform.rotation);
            }
            else if (start)
            {
                if (positions.Count > 1)
                {
                    index = positions.Count - 1;
                    //Debug.Log(index);
                    elapsedTime += Time.deltaTime;
                    float percentageComplete = elapsedTime / 0.0000001f;
                    item.transform.position = 
                        Vector3.Lerp(positions[index], positions[index - 1], Mathf.SmoothStep(0,1,percentageComplete));
                    if (item.transform.position == positions[index - 1])
                    { 
                        positions.RemoveAt(index);                                      
                        positions.TrimExcess();
                        elapsedTime = 0f;
                    }
                }

                if (rotations.Count > 1)
                {
                    int indexRot = rotations.Count - 1;
                    elapsedTime += Time.deltaTime;
                    float percentageComplete = elapsedTime / 0.0000001f;
                    item.transform.rotation =
                        Quaternion.Lerp(rotations[indexRot], rotations[indexRot - 1], Mathf.SmoothStep(0,1,percentageComplete));
                    
                    if (item.transform.rotation == rotations[indexRot - 1])
                    {
                        rotations.RemoveAt(indexRot);                                   
                        positions.TrimExcess();                                         
                        elapsedTime = 0f;
                    }
                }

                if(rotations.Count <= 1 && positions.Count <= 1)
                {
                    tracker.actualAmount++;
                    Debug.Log(actualAmount);
                    start = false; 
                    Debug.Log(tracker.expectedAmount);
                    if (tracker.actualAmount == tracker.expectedAmount - 1)
                    {
                        itemData.SpawnAsync(callback =>
                        {
                            Debug.Log("Succesfully spawned item");
                            callback.transform.position = position;
                            callback.transform.rotation = rotation;
                            tracker.expectedAmount = 0;
                        });
                        item.disallowDespawn = false;
                    }
                    item.Despawn();
                }
            }
        }

        IEnumerator TimerTracker()
        {
            yield return new WaitForSeconds(5f);
            startTracking = false;
        }
    }

    public class RepairoHandler : Spell
    {
        public static SpellType spellType = SpellType.Raycast;
        public override Spell AddGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        public override void SpawnSpell(Type type, string name, Item wand, float spellSpeed)
        {
            throw new NotImplementedException();
        }

        public override void UpdateSpell(Type type, string name, Item wand)
        {
            if (wand.gameObject.GetComponent(type)) UnityEngine.Object.Destroy(wand.gameObject.GetComponent(type));
            wand.gameObject.AddComponent(type);
        }
    }
}