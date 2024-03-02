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
                            
                            detector.sightDetectionHorizontalFov = 0f;
                            detector.sightDetectionVerticalFov = 0f;
                        }
                        else
                        {
                            detector.sightDetectionHorizontalFov = 0f;
                            detector.sightDetectionVerticalFov = 0f;
                        }
                    }
                    else {
                        if ((Player.currentCreature.transform.position - creature.transform.position).sqrMagnitude < 1f * 1f)
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
        void Start()
        {
            creature = Player.currentCreature;
            evanescoDissolve = Loader.local.dissimuloDissolveMat.DeepCopyByExpressionTree();
            canDisillusion = true;
            dissolveVal = 0;
            Loader.local.dissimuloActive = true;

            Loader.local.activeDisillusion = new GameObject();
            Loader.local.activeDisillusion.AddComponent<DissimuloActive>();
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
                    evanescoTemp.SetTexture("_Albedo", myMaterials[i].GetTexture("_BaseMap"));
                    evanescoTemp.SetColor("_color", myMaterials[i].GetColor("_BaseColor"));
                    evanescoTemp.SetTexture("_Normal", myMaterials[i].GetTexture("_BumpMap"));
                    evanescoTemp.SetTexture("_Metallic", myMaterials[i].GetTexture("_MetallicGlossMap"));

                    matDefGood[i] = evanescoTemp.DeepCopyByExpressionTree();
                }

                data.renderer.materials = matDefGood;
            }
        }

        private void Update()
        {
            if (canDisillusion)
            {
                if (dissolveVal < 1)
                {
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
        public override void UpdateSpell(Type type, string name, Item wand, String itemType)
        {
            throw new NotImplementedException();
        }
    } 
}