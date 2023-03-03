using System;
using System.Collections.Generic;
using System.Linq;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
    public class Dissimulare : MonoBehaviour
    {
        private Material evanescoDissolve;
        private Creature creatureRendering;
        List<Material> myMaterials;
        private bool canDisillusion;
        private float dissolveVal;
        private Material[] original;
        void Start()
        {
            creatureRendering = Player.currentCreature;
            evanescoDissolve = Loader.local.dissimuloDissolveMat.DeepCopyByExpressionTree();
            dissolveVal = 1f;
            canDisillusion = true;
            Loader.local.dissimuloActive = false;
            Destroy(Loader.local.activeDisillusion);
            foreach (Creature creature in Creature.allActive)
            {
                if (creature.brain.instance.GetModule<BrainModuleDetection>() is BrainModuleDetection detector)
                {
                    if (Loader.local.creaturesFOV.ContainsKey(creature))
                    {
                        Debug.Log("Vertical: " + Loader.local.creaturesFOV[creature][0]);
                        Debug.Log("Horizontal: " + Loader.local.creaturesFOV[creature][1]);
                        float vertical = Loader.local.creaturesFOV[creature][0];
                        float horizontal = Loader.local.creaturesFOV[creature][1];
                        detector.sightDetectionHorizontalFov = horizontal;
                        detector.sightDetectionVerticalFov = vertical;
                        Debug.Log("Creature: " + creature + " | " + "Values: {Horizontal | Vertical}" + detector.sightDetectionHorizontalFov + " | " + detector.sightDetectionVerticalFov);
                    }
                }
            }
        }

        private void Update()
        {
            if (canDisillusion)
            {
                if (dissolveVal > 0)
                { 
                    dissolveVal -= 0.01f;
                    foreach (Creature.RendererData var in creatureRendering.renderers)
                    {
                        foreach (Material mat in var.renderer.materials)
                        {
                            CustomDebug.Debug("Dissimulo Material: " + mat);
                            mat.SetFloat("_dissolve", dissolveVal);
                        }
                    }
                }
                else if (dissolveVal <= 0)
                {
                    dissolveVal = 1f;
                    canDisillusion = false;
                    for (int i = 0; i < creatureRendering.renderers.Count; i++) {
                        creatureRendering.renderers[i].renderer.materials = Loader.local.originalCreatureMaterial[i];
                    }

                }

            }
        }
    }

    public class DissimulareHandler : Spell
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
            Debug.Log("Got here after before dissimulo check");
            if (Loader.local.dissimuloActive)
            {
                if (wand.gameObject.GetComponent(type)) UnityEngine.Object.Destroy(wand.gameObject.GetComponent(type));
                wand.gameObject.AddComponent(type);
            }
        }
    }
}