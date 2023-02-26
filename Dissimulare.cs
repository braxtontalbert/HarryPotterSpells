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
        private Creature creature;
        List<Material> myMaterials;
        private bool canDisillusion;
        private float dissolveVal;
        private Material[] original;
        void Start()
        {
            creature = Player.currentCreature;
            evanescoDissolve = Loader.local.dissimuloDissolveMat.DeepCopyByExpressionTree();
            canDisillusion = true;
            dissolveVal = 1f;
            Loader.local.dissimuloActive = false;
            Destroy(Loader.local.activeDisillusion);
            foreach (Creature creature in Creature.allActive)
            {
                if (Loader.local.creaturesFOV.ContainsKey(creature))
                {
                    Debug.Log("Vertical: " + Loader.local.creaturesFOV[creature][0]);
                    Debug.Log("Horizontal: " + Loader.local.creaturesFOV[creature][1]);
                    creature.brain.instance.GetModule<BrainModuleDetection>().sightDetectionHorizontalFov = Loader.local.creaturesFOV[creature][1];
                    creature.brain.instance.GetModule<BrainModuleDetection>().sightDetectionVerticalFov = Loader.local.creaturesFOV[creature][0];
                }
            }
        }

        private void Update()
        {
            if (canDisillusion)
            {
                if (dissolveVal > 0)
                {
                    //dissolveVal += Time.deltaTime / 3f;

                    //Debug.Log(dissolveVal);
                    dissolveVal -= 0.01f;
                    foreach (Creature.RendererData var in creature.renderers)
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
                    for (int i = 0; i < creature.renderers.Count; i++) {
                        creature.renderers[i].renderer.materials = Loader.local.originalCreatureMaterial[i];
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