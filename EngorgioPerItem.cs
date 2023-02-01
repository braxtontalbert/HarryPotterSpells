using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    class EngorgioPerItem : MonoBehaviour
    {

        internal bool cantEngorgio = true;
        Item item;
        private float elapsedTime;
        private Vector3 engorgioMaxSize;
        internal bool canReducio = false;
        GameObject hit;
        Vector3 reducioMinSize;
        internal Vector3 ogScale;
        bool hasStarted = false;

        void Start() {
            if (GetComponent<Item>() != null)
            {
                item = GetComponent<Item>();
            }
            else if (GetComponentInParent<Item>() != null)
            {
                item = GetComponentInParent<Item>();
            }
            
            engorgioMaxSize = new Vector3(item.transform.localScale.x * 2f, item.transform.localScale.y * 2f, item.transform.localScale.z * 2f);
            reducioMinSize = new Vector3(item.transform.localScale.x * 0.5f, item.transform.localScale.y * 0.5f, item.transform.localScale.z * 0.5f);
            ogScale = item.transform.localScale;

        }


        void Update() {

            if (cantEngorgio == false )
            {
                hasStarted = false;
                elapsedTime += Time.deltaTime;
                float percentageComplete = elapsedTime / 0.2f;


                item.transform.localScale = Vector3.Lerp(item.transform.localScale, engorgioMaxSize, Mathf.SmoothStep(0, 1, percentageComplete));



                elapsedTime = 0f;

                if (item.transform.localScale == engorgioMaxSize)
                {


                    cantEngorgio = true;

                }
            }


            else if (canReducio) {

                elapsedTime += Time.deltaTime;
                float percentageComplete = elapsedTime / 0.2f;


                item.transform.localScale = Vector3.Lerp(item.transform.localScale, reducioMinSize, Mathf.SmoothStep(0, 1, percentageComplete));

                elapsedTime = 0f;

                if (item.transform.localScale == reducioMinSize)
                {

                    canReducio = false;

                }




            }


        }
    }
}
