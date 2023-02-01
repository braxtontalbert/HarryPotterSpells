using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    class AccioPerItem : MonoBehaviour
    {
        private bool cantAccio;
        private float elapsedTime;
        private Vector3 endPoint;
        private RagdollHand oppositeHand;
        private GameObject parentLocal;
        private Vector3 startPoint;
        private string componentLevel;

        void Start() {

            cantAccio = false;
        
        
        }

        void Update()
        {

            if (!cantAccio)
            {

                elapsedTime += Time.deltaTime;
                float percentageComplete = elapsedTime / 0.6f;

                endPoint = oppositeHand.transform.position;

                float distanceSqr = (endPoint - parentLocal.transform.position).sqrMagnitude;
                parentLocal.transform.position = Vector3.Lerp(startPoint, endPoint, Mathf.SmoothStep(0, 1, percentageComplete));

                if (distanceSqr <= 0.03f)
                {

                    cantAccio = true;

                    elapsedTime = 0f;
                    if (oppositeHand == Player.local.handLeft.ragdollHand)
                    {
                        switch (componentLevel)
                        {

                            case "Mid":
                                Player.currentCreature.GetHand(Side.Left).Grab(parentLocal.GetComponent<Item>().mainHandleRight);
                                break;
                            case "Parent":
                                Player.currentCreature.GetHand(Side.Left).Grab(parentLocal.GetComponentInParent<Item>().mainHandleRight);
                                break;
                            case "Child":
                                Player.currentCreature.GetHand(Side.Left).Grab(parentLocal.GetComponentInChildren<Item>().mainHandleRight);
                                break;

                        }
                    }

                    else
                    {

                        switch (componentLevel)
                        {

                            case "Mid":
                                Player.currentCreature.GetHand(Side.Right).Grab(parentLocal.GetComponent<Item>().mainHandleRight);
                                break;

                            case "Parent":
                                Player.currentCreature.GetHand(Side.Right).Grab(parentLocal.GetComponentInParent<Item>().mainHandleRight);
                                break;
                            case "Child":
                                Player.currentCreature.GetHand(Side.Right).Grab(parentLocal.GetComponentInChildren<Item>().mainHandleRight);
                                break;

                        }

                    }
                }
            }




     
        }




        public void Setup(GameObject importParentLocal, Vector3 importStartPoint, RagdollHand importOpossiteHand, string importComponentLevel, bool importCantAccio)
        {

            parentLocal = importParentLocal;
            startPoint = importStartPoint;
            oppositeHand = importOpossiteHand;
            componentLevel = importComponentLevel;
            cantAccio = importCantAccio;


        }
    }
}
