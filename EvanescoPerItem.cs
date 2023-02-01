using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    class EvanescoPerItem : MonoBehaviour
    {

        bool cantEvanesco;

        
        Item item;
        private float elapsedTime;
        float dissolveVal;

        void Start()
        {
            if (GetComponent<Item>() is Item item)
            {
                this.item = item;
            }
            else if (GetComponentInParent<Item>() is Item item2) {

                item = item2;

            }

            else if (GetComponentInChildren<Item>() is Item item3) {

                item = item3;
            
            
            }
            cantEvanesco = false;
            dissolveVal = 0;

        }


        void Update()
        {

            if (cantEvanesco == false)
            {
                if (item.gameObject.GetComponent<Renderer>() != null)
                {

                    if (dissolveVal < 1)
                    {
                        //dissolveVal += Time.deltaTime / 3f;

                        //Debug.Log(dissolveVal);
                        dissolveVal += 0.01f;
                        foreach (Material mat in item.gameObject.GetComponent<Renderer>().materials)
                        {
                            CustomDebug.Debug("Evanesco Material: " + mat);
                            mat.SetFloat("_dissolve", dissolveVal);
                        }


                    }


                    else if (dissolveVal >= 1f)
                    {
                        dissolveVal = 0;
                        cantEvanesco = true;
                        Destroy(item.gameObject);


                    }
                }

                else if (item.gameObject.GetComponentInChildren<Renderer>() != null)
                {

                    if (dissolveVal < 1)
                    {
                        CustomDebug.Debug(dissolveVal);
                        dissolveVal += 0.01f;
                        foreach (Material mat in item.gameObject.GetComponentInChildren<Renderer>().materials)
                        {
                            CustomDebug.Debug("Evanesco Material: " + mat);
                            mat.SetFloat("_dissolve", dissolveVal);
                        }

                    }


                    else if (dissolveVal >= 1f)
                    {


                        dissolveVal = 0;
                        cantEvanesco = true;
                        Destroy(item.gameObject);


                    }
                }

                else if (item.gameObject.GetComponentInParent<Renderer>() != null)
                {

                    if (dissolveVal < 1)
                    {
                        //dissolveVal += Time.deltaTime / 3f;

                        //Debug.Log(dissolveVal);
                        dissolveVal += 0.01f;
                        foreach (Material mat in item.gameObject.GetComponentInParent<Renderer>().materials)
                        {
                            CustomDebug.Debug("Evanesco Material: " + mat);
                            mat.SetFloat("_dissolve", dissolveVal);
                        }

                    }

                    else if (dissolveVal >= 1f)
                    {
                        dissolveVal = 0;
                        cantEvanesco = true;
                        Destroy(item.gameObject);


                    }
                }

            }
        }
    }
}
