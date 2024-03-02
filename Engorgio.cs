using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    class Engorgio : MonoBehaviour
    {
        Item item;
        Item npcItem;
        internal Vector3 startPoint;
        internal Vector3 endPoint;
        internal GameObject parentLocal;
        internal Vector3 ogScale;
        internal bool cantEngorgio;
        private float elapsedTime;
        private float duration;
        private Vector3 engorgioMaxSize;
        internal string command;

        public static SpellType spellType = SpellType.Raycast;
        

        public void Start()
        {
            item = GetComponent<Item>();
            CastRay();
        }


        internal void CastRay()
        {
            RaycastHit hit;
            Transform parent;

            if (Physics.Raycast(item.flyDirRef.transform.position, item.flyDirRef.transform.forward, out hit, float.MaxValue, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {

                parent = hit.collider.gameObject.transform.parent;
                ogScale = parent.gameObject.transform.localScale;
                parentLocal = hit.collider.gameObject;
                if (parentLocal.GetComponentInParent<Item>() is Item itemParent)
                {
                    cantEngorgio = false;

                    if (parentLocal.gameObject.GetComponent<EngorgioPerItem>() == null)
                    {
                        EngorgioPerItem EPI = parentLocal.gameObject.AddComponent<EngorgioPerItem>();

                        EPI.cantEngorgio = false;
                        EPI.canReducio = false;
                    }

                    else if (parentLocal.gameObject.GetComponent<EngorgioPerItem>() is EngorgioPerItem epi)
                    {

                        epi.cantEngorgio = false;
                        epi.canReducio = false;


                    }
                }

                else if (parentLocal.GetComponent<Item>() is Item itemCurrent)
                {

                    if (parentLocal.gameObject.GetComponent<EngorgioPerItem>() == null)
                    {
                        EngorgioPerItem EPI = parentLocal.gameObject.AddComponent<EngorgioPerItem>();

                        EPI.cantEngorgio = false;
                        EPI.canReducio = false;
                    }

                    else if (parentLocal.gameObject.GetComponent<EngorgioPerItem>() is EngorgioPerItem epi)
                    {

                        epi.cantEngorgio = false;
                        epi.canReducio = false;


                    }
                }
            }

            else if (parentLocal.GetComponentInChildren<Item>() is Item itemChild)
            {

                if (parentLocal.gameObject.GetComponentInChildren<EngorgioPerItem>() == null)
                {
                    EngorgioPerItem EPI = parentLocal.gameObject.AddComponent<EngorgioPerItem>();

                    EPI.cantEngorgio = false;
                    EPI.canReducio = false;
                }

                else if (parentLocal.gameObject.GetComponentInChildren<EngorgioPerItem>() is EngorgioPerItem epi)
                {

                    epi.cantEngorgio = false;
                    epi.canReducio = false;


                }

            }
            else
            {

                cantEngorgio = true;

            }



        }


    }

    public class EngorgioHandler : Spell
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
            throw new NotImplementedException();
        }
    }
}



