using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using static ThunderRoad.TutorialInteraction;

namespace WandSpellss
{
    class Accio : MonoBehaviour
    {
        Item item;
        private String itemType;
        internal Item wand;
        Item npcItem;
        internal Vector3 startPoint;
        internal Vector3 endPoint;
        internal GameObject parentLocal;
        internal bool cantAccio;
        private float elapsedTime;
        public string componentLevel;
        RagdollHand oppositeHand;
        Dictionary<Vector3, float> rayPoints = new Dictionary<Vector3, float>();

        public static SpellType spellType = SpellType.Raycast;


        public void Start()
        {
            item = GetComponent<Item>();
            bool vectorBreakFlag = false;


            
            if (itemType != null)
            {
                itemType = ParseItemType(this.itemType);
                Debug.Log("In Accio " + this.itemType);
            }
            else Debug.Log("In Accio");

            //Debug.Log("Start method itemType value is: " + itemType);
            cantAccio = true;
            CastRay();

        }


        void StartAccio(Item currentItem) {

            
            if (currentItem)
            {
                startPoint = currentItem.gameObject.transform.position;
                //endPoint = wand.mainHandler.otherHand.transform.position;

                if (currentItem.mainHandler && !currentItem.mainHandler.creature.isPlayer)
                {
                    currentItem.mainHandler.playerHand.ragdollHand.UnGrab(false);
                }

                if (currentItem.holder != null && currentItem.holder.creature && !currentItem.holder.creature.isPlayer)
                {
                    currentItem.holder.GrabFromHandle();
                }
                else if(currentItem.holder)
                {
                    currentItem.holder.GrabFromHandle();
                }
                if (currentItem.gameObject.GetComponent<AccioPerItem>() is AccioPerItem api)
                {

                    api.Setup(currentItem.gameObject, startPoint, oppositeHand, componentLevel, false);
                }

                else
                {
                    item.gameObject.AddComponent<AccioPerItem>().Setup(currentItem.gameObject, startPoint, oppositeHand, componentLevel, false);
                }
            }
            else CustomDebug.Debug("ACCIO HIT RETURNED NULL");



        }
        Item CheckHit(RagdollHand opposite, GameObject parentLocal, Vector3 hit) {

            if (!opposite.grabbedHandle)
            {

                if (itemType != null && !itemType.ToLower().Equals("weapon"))
                {
                    if (parentLocal.GetComponent<Item>() is Item accioItem1 &&
                        accioItem1.name.ToLower().Contains(itemType))
                    {
                        componentLevel = "Mid";
                        return accioItem1;
                    }

                    if (parentLocal.GetComponentInParent<Item>() is Item accioItem2 &&
                        accioItem2.name.ToLower().Contains(itemType))
                    {
                        componentLevel = "Parent";
                        return accioItem2;

                    }

                    if (parentLocal.GetComponentInChildren<Item>() is Item accioItem3 &&
                        accioItem3.name.ToLower().Contains(itemType))
                    {
                        componentLevel = "Child";
                        return accioItem3;
                    }

                    Dictionary<Item, float> toCompare = new Dictionary<Item, float>();
                    componentLevel = "Mid";
                    foreach (Item selected in Item.allActive)
                    {
                        Debug.Log("Item name: " + selected.name);
                        if(!Player.local.creature.equipment.GetAllHolsteredItems().Contains(selected))
                        {
                                float distance = (selected.transform.position - hit)
                                    .sqrMagnitude;
                                if (distance < 5f * 5f)
                                {
                                    if (selected.data.displayName.ToLower().Contains(itemType) && !toCompare.ContainsKey(selected))
                                    {
                                        toCompare.Add(selected, distance);
                                    }
                                }
                        }
                    }

                    return toCompare.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                }
                
                if (itemType != null && itemType.ToLower().Equals("weapon"))
                {
                    componentLevel = "Mid";
                    Dictionary<Item, float> toCompare = new Dictionary<Item, float>();
                    foreach (Item selected in Item.allActive)
                    {
                        if(!Player.local.creature.equipment.GetAllHolsteredItems().Contains(selected))
                        {
                            float distance = (selected.transform.position - hit)
                                .sqrMagnitude;
                            if (distance < 5f * 5f)
                            {
                                if (selected.data.type.ToString().ToLower().Contains(itemType) && !toCompare.ContainsKey(selected))
                                {
                                    toCompare.Add(selected, distance);
                                }
                            }
                        }
                    }

                    return toCompare.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                }
                else
                {
                    if (parentLocal.GetComponent<Item>() is Item accioItem1)
                    {
                        componentLevel = "Mid";
                        return accioItem1;
                    }

                    if (parentLocal.GetComponentInParent<Item>() is Item accioItem2)
                    {
                        componentLevel = "Parent";
                        return accioItem2;

                    }

                    if (parentLocal.GetComponentInChildren<Item>() is Item accioItem3)
                    {
                        componentLevel = "Child";
                        return accioItem3;
                    }
                    rayPoints.Reverse();
                    foreach (Vector3 position in rayPoints.Keys)
                    {
                        float found;
                        rayPoints.TryGetValue(position, out found);
                        return Item.allActive.Where(item => item != null && item != this.item && !Player.currentCreature.equipment.GetAllHolsteredItems().Contains(item) && (item.transform.position - position).magnitude < found).First();
                    }
                }

            }
            return null;
        }

        internal void CastRay() {
            
            RaycastHit hit;
            Transform parent;
            if (itemType != null)
            {
                if (Physics.Raycast(item.flyDirRef.transform.position, item.flyDirRef.transform.forward, out hit,
                        float.MaxValue, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                {

                    parent = hit.collider.gameObject.transform.parent;
                    parentLocal = hit.collider.gameObject;

                    oppositeHand = item.mainHandler.otherHand;

                    StartAccio(CheckHit(oppositeHand, parentLocal, hit.point));

                }

            }
            else
            {
                if (Physics.Raycast(item.flyDirRef.transform.position, item.flyDirRef.transform.forward, out hit,
                        float.MaxValue, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                {

                    parent = hit.collider.gameObject.transform.parent;
                    parentLocal = hit.collider.gameObject;

                    oppositeHand = item.mainHandler.otherHand;

                    StartAccio(CheckHit(oppositeHand, parentLocal, hit.point));

                }
            }

        }

        public string ParseItemType(String itemType)
        {
            return this.itemType.Remove(0, 6);
            
        }


        public void Setup(String itemType) {
            this.itemType = itemType;
        }


    }

    public class AccioHandler : Spell
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
        public override void UpdateSpell(Type type, string name, Item wand, String itemType)
        {
            Debug.Log("before type check");
            if (type == Type.GetType("WandSpellss." +"Accio" +""))
            {
                Debug.Log("Types are equal");
                if (wand.gameObject.GetComponent(type)) UnityEngine.Object.Destroy(wand.gameObject.GetComponent(type));
                wand.gameObject.AddComponent<Accio>().Setup(itemType);
            }
        }
    }

}