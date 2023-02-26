using System;
using System.Collections.Generic;
using System.Linq;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{

    public class DissimuloActive : MonoBehaviour
    {
        public float ogVertFOV;
        

        private void Update()
        {
            foreach (Creature creature in Creature.allActive)
            {
                if (creature.brain.instance.GetModule<BrainModuleDetection>() is BrainModuleDetection detector)
                {
                    if (detector.sightDetectionHorizontalFov != 0 && detector.sightDetectionVerticalFov != 0)
                    {
                        if (!Loader.local.creaturesFOV.ContainsKey(creature))
                        {
                            Loader.local.creaturesFOV.Add(creature, new[] { detector.sightDetectionVerticalFov, detector.sightDetectionHorizontalFov });
                            creature.brain.currentTarget = null;
                            detector.sightDetectionHorizontalFov = 0f;
                            detector.sightDetectionVerticalFov = 0f;
                        }
                        else
                        {
                            creature.brain.currentTarget = null;
                            detector.sightDetectionHorizontalFov = 0f;
                            detector.sightDetectionVerticalFov = 0f;
                        }
                    }
                    else {
                        if (Vector3.Distance(Player.currentCreature.transform.position, creature.transform.position) < 2f)
                        {
                            if (Loader.local.creaturesFOV.ContainsKey(creature))
                            {
                                detector.sightDetectionHorizontalFov = Loader.local.creaturesFOV[creature][1];
                                detector.sightDetectionVerticalFov = Loader.local.creaturesFOV[creature][0];
                            }
                        }
                        
                    }
                    
                }
            }
        }
    }

    public class Dissimulo : MonoBehaviour
    {
        private Material evanescoDissolve;
        private Creature creature;
        private bool canDisillusion;
        private float dissolveVal;
        GameObject dissimuloActive;
        void Start()
        {
            creature = Player.currentCreature;
            evanescoDissolve = Loader.local.dissimuloDissolveMat.DeepCopyByExpressionTree();
            canDisillusion = true;
            dissolveVal = 0;
            Loader.local.dissimuloActive = true;
            dissimuloActive = new GameObject();
            dissimuloActive.AddComponent<DissimuloActive>();
            Loader.local.activeDisillusion = Instantiate(dissimuloActive);
            StartDissimulo();
        }

        

        void StartDissimulo()
        {
            foreach (Creature.RendererData data in creature.renderers)
            {
                Material evanescoTemp = evanescoDissolve = Loader.local.dissimuloDissolveMat.DeepCopyByExpressionTree();
                Loader.local.originalCreatureMaterial.Add(data.renderer.materials);
                Material[] myMaterials = data.renderer.materials;
                Material[] matDefGood = new Material[myMaterials.Length];
                
                for (int i = 0; i < myMaterials.Length; i++)
                {
                    //Debug.Log("Im in the " +i+ " iteration of the loop");
                    evanescoTemp.SetTexture("_Albedo", myMaterials[i].GetTexture("_BaseMap"));
                    evanescoTemp.SetColor("_color", myMaterials[i].GetColor("_BaseColor"));
                    //Debug.Log(evanescoDissolve.GetTexture("_Albedo"));
                    evanescoTemp.SetTexture("_Normal", myMaterials[i].GetTexture("_BumpMap"));
                    //Debug.Log(evanescoDissolve.GetTexture("_Normal"));
                    evanescoTemp.SetTexture("_Metallic", myMaterials[i].GetTexture("_MetallicGlossMap"));
                    //Debug.Log(evanescoDissolve.GetTexture("_Metallic"));

                    matDefGood[i] = evanescoTemp.DeepCopyByExpressionTree();
                }

                data.renderer.materials = matDefGood;
                
                //data.renderer.gameObject.AddComponent<EvanescoPerItem>();
            }
        }

        private void Update()
        {
            if (canDisillusion)
            {
                if (dissolveVal < 1)
                {
                    //dissolveVal += Time.deltaTime / 3f;

                    //Debug.Log(dissolveVal);
                    dissolveVal += 0.01f;
                    foreach (Creature.RendererData var in creature.renderers)
                    {
                        foreach (Material mat in var.renderer.materials)
                        {
                            CustomDebug.Debug("Dissimulo Material: " + mat);
                            mat.SetFloat("_dissolve", dissolveVal);
                        }
                        
                    }
                    


                }


                else if (dissolveVal >= 1f)
                {
                    dissolveVal = 0;
                    canDisillusion = false;
                }

            }
        }
    }
        
        
    
    public class DissimuloHandler : Spell
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
            if (!Loader.local.dissimuloActive)
            {
                if (wand.gameObject.GetComponent(type)) UnityEngine.Object.Destroy(wand.gameObject.GetComponent(type));
                wand.gameObject.AddComponent(type);
            }
        }
    } 
}