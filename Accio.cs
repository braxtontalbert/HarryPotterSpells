using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    class Accio : Spell
    {
        Item item;
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

        public override Spell AddGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            item = GetComponent<Item>();
            bool vectorBreakFlag = false;

            cantAccio = true;

            CastRay();

        }


        void StartAccio(Item currentItem) {

            
            if (currentItem)
            {
                startPoint = currentItem.gameObject.transform.position;
                //endPoint = wand.mainHandler.otherHand.transform.position;

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
        Item CheckHit(RagdollHand opposite, GameObject parentLocal) {

            if (opposite.grabbedHandle == null)
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

                else if (parentLocal.GetComponentInChildren<Item>() is Item accioItem3)
                {
                    componentLevel = "Child";
                    return accioItem3;
                }


                else
                {
                    rayPoints.Reverse();
                    foreach (Vector3 position in rayPoints.Keys)
                    {
                        float found;
                        rayPoints.TryGetValue(position, out found);
                        return (Item)Item.allActive.Where(item => item != null && item != this.item && !Player.currentCreature.equipment.GetAllHolsteredItems().Contains(item) && (item.transform.position - position).magnitude < found).First();
                    }
                }

            }

            return null;
        }

        internal void CastRay() {

            
            RaycastHit hit;
            Transform parent;
            if (Physics.Raycast(item.flyDirRef.transform.position, item.flyDirRef.transform.forward, out hit, float.MaxValue, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                float raycastDistance;
                if ((hit.point - item.flyDirRef.transform.position).magnitude < float.MaxValue)
                {
                    raycastDistance = (hit.point - item.flyDirRef.transform.position).magnitude;

                    float distanceOut = 2f;

                    float sideC = (float)Math.Sqrt(raycastDistance * raycastDistance + distanceOut * distanceOut);

                    int maxAngle = (int)Math.Round(Math.Asin(distanceOut / sideC));

                    Dictionary<int, double> knownAngleDistance = new Dictionary<int, double>();

                    for (int i = 0; i < maxAngle; i++)
                    {
                        if (!knownAngleDistance.ContainsKey(i))
                        {
                            knownAngleDistance.Add(i, (raycastDistance / Math.Cos(i)));
                        }
                    }
                    

                    foreach(Item collectedItem in Item.allActive)
                    {
                        if(collectedItem)
                    }

                }
                else { }

            /*if (Physics.Raycast(item.flyDirRef.transform.position, item.flyDirRef.transform.forward, out hit, float.MaxValue, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                float maxHit = (hit.point - item.flyDirRef.position).magnitude;
                float maxIterative = maxHit / 10;
                float distanceIterative = 0;
                float maxDistance = 5f;
                for (int i = 0; i < maxHit; i++) {
                    
                    if(i == (int)Math.Round(maxIterative)) rayPoints.Add(item.flyDirRef.transform.position + (item.flyDirRef.transform.forward * i), distanceIterative);
                    if (distanceIterative < maxDistance)
                    {
                        distanceIterative += 0.3f;
                    }
                }

                CustomDebug.Debug("Did hit.");
                CustomDebug.Debug(hit.collider.gameObject.name);

                parent = hit.collider.gameObject.transform.parent;
                parentLocal = hit.collider.gameObject;

                oppositeHand = item.mainHandler.otherHand;

                StartAccio(CheckHit(oppositeHand, parentLocal));
                
            }*/
            

        }


        public void Setup(Item wandIn) {

            this.wand = wandIn;

        
        }


    }

}