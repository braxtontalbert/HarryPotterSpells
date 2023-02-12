using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    class Evanesco : MonoBehaviour
    {
        Item item;
        Item npcItem;
        internal Vector3 startPoint;
        internal Vector3 endPoint;
        internal GameObject parentLocal;
        internal Vector3 ogScale;
        internal bool cantEvanesco;
        internal Material evanescoDissolve;
        List<Material> myMaterials;

        public static SpellType spellType = SpellType.Raycast;


        public void Start()
        {
            item = GetComponent<Item>();
            //Catalog.LoadAssetAsync<Material>("apoz123Wand.SpellEffect.Evanesco.Mat", callback => { this.evanescoDissolve = callback; }, "HarryPotterSpells");
            evanescoDissolve = Loader.local.evanescoDissolveMat.DeepCopyByExpressionTree();
            CastRay();

        }

        internal void CastRay()
        {

            RaycastHit hit;
            Transform parent;

            if (Physics.Raycast(item.flyDirRef.transform.position, item.flyDirRef.transform.forward, out hit, float.MaxValue, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {


                parent = hit.collider.gameObject.transform.parent;
                parentLocal = hit.collider.gameObject;
                if (parentLocal.GetComponentInParent<Item>() is Item evanescoItem)
                {
                    if (evanescoItem.gameObject.GetComponent<Renderer>() is Renderer renderer)
                    {
                        myMaterials = renderer.materials.ToList();
                        Material[] matDefGood = new Material[myMaterials.Count];

                        for (int i = 0; i < myMaterials.Count; i++)
                        {
                            //Debug.Log("Im in the " +i+ " iteration of the loop");
                            evanescoDissolve.SetTexture("_Albedo", myMaterials[i].GetTexture("_BaseMap"));
                            evanescoDissolve.SetColor("_color", myMaterials[i].GetColor("_BaseColor"));
                            //Debug.Log(evanescoDissolve.GetTexture("_Albedo"));
                            evanescoDissolve.SetTexture("_Normal", myMaterials[i].GetTexture("_BumpMap"));
                            //Debug.Log(evanescoDissolve.GetTexture("_Normal"));
                            evanescoDissolve.SetTexture("_Metallic", myMaterials[i].GetTexture("_MetallicGlossMap"));
                            //Debug.Log(evanescoDissolve.GetTexture("_Metallic"));

                            matDefGood[i] = evanescoDissolve;
                        }
                        renderer.materials = matDefGood;





                        cantEvanesco = false;
                        evanescoItem.gameObject.AddComponent<EvanescoPerItem>();

                    }

                    else if (evanescoItem.gameObject.GetComponentInParent<Renderer>() is Renderer renderer2)
                    {
                        myMaterials = renderer2.materials.ToList();
                        Material[] matDefGood = new Material[myMaterials.Count];

                        for (int i = 0; i < myMaterials.Count; i++)
                        {
                            //Debug.Log("Im in the " + i + " iteration of the loop");
                            evanescoDissolve.SetTexture("_Albedo", myMaterials[i].GetTexture("_BaseMap"));
                            evanescoDissolve.SetColor("_color", myMaterials[i].GetColor("_BaseColor"));
                            //Debug.Log(evanescoDissolve.GetTexture("_Albedo"));
                            evanescoDissolve.SetTexture("_Normal", myMaterials[i].GetTexture("_BumpMap"));
                            //Debug.Log(evanescoDissolve.GetTexture("_Normal"));
                            evanescoDissolve.SetTexture("_Metallic", myMaterials[i].GetTexture("_MetallicGlossMap"));
                            //Debug.Log(evanescoDissolve.GetTexture("_Metallic"));

                            matDefGood[i] = evanescoDissolve;
                        }
                        renderer2.materials = matDefGood;
                        cantEvanesco = false;
                        evanescoItem.gameObject.AddComponent<EvanescoPerItem>();
                    }
                    else if (evanescoItem.gameObject.GetComponentInChildren<Renderer>() is Renderer renderer3)
                    {
                        myMaterials = renderer3.materials.ToList();
                        Material[] matDefGood = new Material[myMaterials.Count];

                        for (int i = 0; i < myMaterials.Count; i++)
                        {
                            //Debug.Log("Im in the " + i + " iteration of the loop");
                            evanescoDissolve.SetTexture("_Albedo", myMaterials[i].GetTexture("_BaseMap"));
                            evanescoDissolve.SetColor("_color", myMaterials[i].GetColor("_BaseColor"));
                            //Debug.Log(evanescoDissolve.GetTexture("_Albedo"));
                            evanescoDissolve.SetTexture("_Normal", myMaterials[i].GetTexture("_BumpMap"));
                            //Debug.Log(evanescoDissolve.GetTexture("_Normal"));
                            evanescoDissolve.SetTexture("_Metallic", myMaterials[i].GetTexture("_MetallicGlossMap"));
                            //Debug.Log(evanescoDissolve.GetTexture("_Metallic"));

                            matDefGood[i] = evanescoDissolve;
                        }
                        renderer3.materials = matDefGood;
                        cantEvanesco = false;
                        evanescoItem.gameObject.AddComponent<EvanescoPerItem>();
                    }
                    
                }

                else if (parentLocal.GetComponent<Item>() is Item evanescoItem1)
                {
                    if (evanescoItem1.gameObject.GetComponent<Renderer>() is Renderer renderer)
                    {
                        myMaterials = renderer.materials.ToList();
                        Material[] matDefGood = new Material[myMaterials.Count];

                        for (int i = 0; i < myMaterials.Count; i++)
                        {
                            //Debug.Log("Im in the " +i+ " iteration of the loop");
                            evanescoDissolve.SetTexture("_Albedo", myMaterials[i].GetTexture("_BaseMap"));
                            evanescoDissolve.SetColor("_color", myMaterials[i].GetColor("_BaseColor"));
                            //Debug.Log(evanescoDissolve.GetTexture("_Albedo"));
                            evanescoDissolve.SetTexture("_Normal", myMaterials[i].GetTexture("_BumpMap"));
                            //Debug.Log(evanescoDissolve.GetTexture("_Normal"));
                            evanescoDissolve.SetTexture("_Metallic", myMaterials[i].GetTexture("_MetallicGlossMap"));
                            //Debug.Log(evanescoDissolve.GetTexture("_Metallic"));

                            matDefGood[i] = evanescoDissolve;
                        }
                        renderer.materials = matDefGood;





                        cantEvanesco = false;
                        evanescoItem1.gameObject.AddComponent<EvanescoPerItem>();

                    }

                    else if (evanescoItem1.GetComponentInParent<Renderer>() is Renderer renderer2)
                    {
                        myMaterials = renderer2.materials.ToList();
                        Material[] matDefGood = new Material[myMaterials.Count];

                        for (int i = 0; i < myMaterials.Count; i++)
                        {
                            //Debug.Log("Im in the " + i + " iteration of the loop");
                            evanescoDissolve.SetTexture("_Albedo", myMaterials[i].GetTexture("_BaseMap"));
                            evanescoDissolve.SetColor("_color", myMaterials[i].GetColor("_BaseColor"));
                            //Debug.Log(evanescoDissolve.GetTexture("_Albedo"));
                            evanescoDissolve.SetTexture("_Normal", myMaterials[i].GetTexture("_BumpMap"));
                            //Debug.Log(evanescoDissolve.GetTexture("_Normal"));
                            evanescoDissolve.SetTexture("_Metallic", myMaterials[i].GetTexture("_MetallicGlossMap"));
                            //Debug.Log(evanescoDissolve.GetTexture("_Metallic"));

                            matDefGood[i] = evanescoDissolve;
                        }
                        renderer2.materials = matDefGood;
                        cantEvanesco = false;
                        evanescoItem1.gameObject.AddComponent<EvanescoPerItem>();
                    }
                    else if (evanescoItem1.GetComponentInChildren<Renderer>() is Renderer renderer3)
                    {
                        myMaterials = renderer3.materials.ToList();
                        Material[] matDefGood = new Material[myMaterials.Count];

                        for (int i = 0; i < myMaterials.Count; i++)
                        {
                            //Debug.Log("Im in the " + i + " iteration of the loop");
                            evanescoDissolve.SetTexture("_Albedo", myMaterials[i].GetTexture("_BaseMap"));
                            evanescoDissolve.SetColor("_color", myMaterials[i].GetColor("_BaseColor"));
                            //Debug.Log(evanescoDissolve.GetTexture("_Albedo"));
                            evanescoDissolve.SetTexture("_Normal", myMaterials[i].GetTexture("_BumpMap"));
                            //Debug.Log(evanescoDissolve.GetTexture("_Normal"));
                            evanescoDissolve.SetTexture("_Metallic", myMaterials[i].GetTexture("_MetallicGlossMap"));
                            //Debug.Log(evanescoDissolve.GetTexture("_Metallic"));

                            matDefGood[i] = evanescoDissolve;
                        }
                        renderer3.materials = matDefGood;
                        cantEvanesco = false;
                        evanescoItem1.gameObject.AddComponent<EvanescoPerItem>();
                    }

                }

                else
                {
                    
                    cantEvanesco = true;

                }




            }


        }


    }

    public class EvanescoHandler : Spell
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