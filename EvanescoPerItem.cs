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
                
                if (item.gameObject.GetComponent<Renderer>() is Renderer renderer)
                {

                    if (dissolveVal < 1)
                    {
                        dissolveVal += 0.01f;
                        foreach (Material mat in renderer.materials)
                        {
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

                else if (item.gameObject.GetComponentInChildren<Renderer>() is Renderer rendererChild)
                {

                    if (dissolveVal < 1)
                    {
                        dissolveVal += 0.01f;
                        foreach (Material mat in rendererChild.materials)
                        {
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

                else if (item.gameObject.GetComponentInParent<Renderer>() is Renderer rendererParent)
                {

                    if (dissolveVal < 1)
                    {
                        dissolveVal += 0.01f;
                        foreach (Material mat in rendererParent.materials)
                        {
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
