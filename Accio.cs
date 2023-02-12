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
                
                parent = hit.collider.gameObject.transform.parent;
                parentLocal = hit.collider.gameObject;

                oppositeHand = item.mainHandler.otherHand;

                StartAccio(CheckHit(oppositeHand, parentLocal));
                
            }
            

        }


        public void Setup(Item wandIn) {

            this.wand = wandIn;

        
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
    }

}