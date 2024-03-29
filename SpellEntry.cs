﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using System.Reflection;
using System.Collections;
using UnityEngine.VFX;
//using FirearmAIFix;
using static ThunderRoad.Creature;

namespace WandSpellss
{
   
    class SpellEntry : MonoBehaviour
    {

        Item wand;
        ItemVoiceModule module;
        private ItemModuleAI itemModuleAI;
        Item current;
        Transform parent;
        GameObject parentLocal;
        float spellSpeed;
        bool magicEffect;
        float expelliarmusPower;
        AudioSource sourceCurrent;
        Material evanescoMat;
        Item currentItem;
        VisualEffect highlighter;
        Creature currentCreature;
        Color initialItemColor;
        Queue<SkinnedMeshRenderer> creatureRenders = new Queue<SkinnedMeshRenderer>();
        Queue<Material> materialQueue = new Queue<Material>();
        Queue<Renderer> renderersQueue = new Queue<Renderer>();
        Material selectorMaterial;
        Item currentLumos;
        private double castDelayTime;
        private bool isDelayingCast;
        
        bool debug;

        public void Setup(float importSpeed, bool importMagicEffect, float importExpelliarmusPower)
        {
            spellSpeed = importSpeed;
            magicEffect = importMagicEffect;
            expelliarmusPower = importExpelliarmusPower;
        }
        void Start() {
            wand = GetComponent<Item>();
            module = wand.data.GetModule<ItemVoiceModule>();
            itemModuleAI = wand.data.GetModule<ItemModuleAI>();
            AIFireable component = GetComponent<AIFireable>();
            //Debug.Log(component.gameObject.GetComponent<FirearmAIFix.AimVisualiserFireable>());
            if (component)
            {
                component.OnAIFire = OnAiFire;
            }
            //recogWand = wand.gameObject.AddComponent<KeyWordRecogWand>();
            wand.OnGrabEvent += Wand_OnGrabEvent;
            wand.OnUngrabEvent += Wand_OnUngrabEvent;
            selectorMaterial = Loader.local.selectorMat;
            debug = false;
        }

        public bool OnAiFire(AIFireable fireable, RagdollHand ragdollHand, bool finished)
        {
            var spelLType = "Stupefy";
            var spellType = Type.GetType("WandSpellss." + spelLType + "");
            TypeSelection(spellType, "Stupefy", GetComponentInParent<Item>());
            return true;
        }

        private void Wand_OnUngrabEvent(Handle handle, RagdollHand ragdollHand, bool throwing)
        {
            //recogWand.isEnabled = false;
            Loader.local.currentlyHeldWands.Remove(this.wand);
            Loader.local.currentlyHeldWands.TrimExcess();
        }

        private void Wand_OnGrabEvent(Handle handle, RagdollHand ragdollHand)
        {
            Loader.local.currentWand = this.wand;
            if (!Loader.local.currentlyHeldWands.Contains(this.wand))
            {
                Loader.local.currentlyHeldWands.Add(this.wand);
            }
        }

        public void TypeSelection(Type spell, string name, Item wand) {

            var spellHandler = Type.GetType(spell.Namespace + "." + name + "Handler");
            var field = spellHandler.GetField("spellType", BindingFlags.Public | BindingFlags.Static);
            var spellType = field.GetValue(null);

            Debug.Log(spellHandler);
            Debug.Log(spellType);
            if ((SpellType)spellType == SpellType.Shoot)
            {
                
                SpellHandler.SpawnSpell(spellHandler, spell, name, wand, spellSpeed);
            }

            else if ((SpellType)spellType == SpellType.Raycast)
            {
                SpellHandler.UpdateSpell(spellHandler, spell, name, wand);
            }
        }
        
        public void TypeSelection(Type spell, string name, Item wand, String itemType)
        {
            Debug.Log("In Type selection");
            name = name.Remove(5).Trim();
            Debug.Log(name);
            var spellHandler = Type.GetType(spell.Namespace + "." + name + "Handler");
            Debug.Log("After handler");
            var field = spellHandler.GetField("spellType", BindingFlags.Public | BindingFlags.Static);
            Debug.Log("After field");
            var spellType = field.GetValue(null);
            Debug.Log("After spell type value");
            if ((SpellType)spellType == SpellType.Shoot)
            {
                SpellHandler.SpawnSpell(spellHandler, spell, name, wand, spellSpeed);
            }

            else if ((SpellType)spellType == SpellType.Raycast)
            {
                SpellHandler.UpdateSpell(spellHandler, spell, name, wand, itemType);
            }
        }
        
        void Update() {
            //debug for raycast objects being hit
            if (debug)
            {
                
                if (Physics.Raycast(wand.flyDirRef.transform.position, wand.flyDirRef.transform.forward, out RaycastHit hit, float.MaxValue, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                {
                    if (hit.collider.GetComponentInParent<Item>() is Item item)
                    {
                        Renderer renderer = item.GetComponent<Renderer>();

                        if (!renderersQueue.Contains(renderer) && !materialQueue.Contains(renderer.material))
                        {
                            initialItemColor = renderer.material.GetColor("_BaseColor");
                            Texture initialAlbedo = renderer.material.GetTexture("_BaseMap");
                            Texture initialMetallic = renderer.material.GetTexture("_MetallicGlossMap");
                            Texture initialNormal = renderer.material.GetTexture("_BumpMap");
                            materialQueue.Enqueue(renderer.material);
                            renderersQueue.Enqueue(renderer);

                            selectorMaterial.SetTexture("_ShaderAlbedo", initialAlbedo);
                            selectorMaterial.SetTexture("_ShaderMetallic", initialMetallic);
                            selectorMaterial.SetTexture("_ShaderNormal", initialNormal);

                            renderer.material = selectorMaterial;
                        }

                    }
                    else
                    {
                        if (renderersQueue.Count > 0 && renderersQueue.Peek()) renderersQueue.Dequeue().material = materialQueue.Dequeue();
                    }

                }

            }

        }
    }
}
