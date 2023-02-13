using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using System.Reflection;
using System.Collections;
using UnityEngine.VFX;
using static ThunderRoad.Creature;

namespace WandSpellss
{
   
    class SpellEntry : MonoBehaviour
    {

        Item wand;
        Item current;
        KeyWordRecogWand recogWand;
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


        bool debug;

        public void Setup(float importSpeed, bool importMagicEffect, float importExpelliarmusPower)
        {
            spellSpeed = importSpeed;
            magicEffect = importMagicEffect;
            expelliarmusPower = importExpelliarmusPower;
        }

        void Start() {
            wand = GetComponent<Item>();
            //recogWand = wand.gameObject.AddComponent<KeyWordRecogWand>();
            wand.OnGrabEvent += Wand_OnGrabEvent;
            wand.OnUngrabEvent += Wand_OnUngrabEvent;
            selectorMaterial = Loader.local.selectorMat;
            debug = false;
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
            Loader.local.currentlyHeldWands.Add(this.wand);
            //recogWand.isEnabled = true;
        }

        public void TypeSelection(Type spell, string name) {

            var spellHandler = Type.GetType(spell.Namespace + "." + name + "Handler");
            var field = spellHandler.GetField("spellType", BindingFlags.Public | BindingFlags.Static);
            var spellType = field.GetValue(null);

            Debug.Log(spellHandler);
            Debug.Log(spellType);
            if ((SpellType)spellType == SpellType.Shoot)
            {
                SpellHandler.SpawnSpell(spellHandler, spell, name, Loader.local.currentlyHeldWands, spellSpeed);
            }

            else if ((SpellType)spellType == SpellType.Raycast)
            {
                SpellHandler.UpdateSpell(spellHandler, spell, name, Loader.local.currentlyHeldWands);
            }
        }

        public void UpdateSpell(Type spell, string name) {

            if (wand.gameObject.GetComponent(spell)) UnityEngine.Object.Destroy(wand.gameObject.GetComponent(spell));
            wand.gameObject.AddComponent(spell);
        }

        public void SpawnSpell(Type spell, string name) {

            float sectumPower = 1f;
            try
            {
                if (name == "Lumos" && currentLumos) return;
                Catalog.GetData<ItemData>(name + "Object")?.SpawnAsync(projectile =>
                {
                    sourceCurrent = null;
                    if (name != "Sectumsempra") projectile.gameObject.AddComponent(spell);

                    else sectumPower = 100f / spellSpeed;
                    if (projectile.gameObject.GetComponent<Expelliarmus>() is Expelliarmus exp)
                    {
                        exp.power = expelliarmusPower;
                    }

                    projectile.transform.position = wand.flyDirRef.transform.position;
                    projectile.transform.rotation = wand.flyDirRef.transform.rotation;
                    projectile.IgnoreObjectCollision(wand);
                    projectile.IgnoreRagdollCollision(Player.currentCreature.ragdoll);
                    
                    projectile.Throw();

                    projectile.rb.useGravity = false;
                    projectile.rb.drag = 0.0f;

                    foreach (AudioSource c in wand.GetComponentsInChildren<AudioSource>())
                    {
                        if (c.name == name) sourceCurrent = c;
                    }
                    if (sourceCurrent != null) sourceCurrent.Play();


                    if (name != "Lumos" && name != "Protego") projectile.GetComponent<Rigidbody>().AddForce(wand.flyDirRef.forward * spellSpeed * sectumPower, ForceMode.Impulse);
                    else currentLumos = projectile;
                    if (name == "Nox") currentLumos = null;
                    if (name != "Lumos" && name != "Protego") projectile.gameObject.AddComponent<SpellDespawn>();
                    current = projectile;

                    if (projectile.gameObject.GetComponent<Protego>() is Protego protego)
                    {
                        protego.GetWand(wand);
                        protego.sourceCurrent = sourceCurrent;
                    }
                    if (projectile.GetComponent<Lumos>() is Lumos lumos) lumos.SetWand(wand);
                });

            }

            catch (NullReferenceException e) { Debug.Log(e.Message); }

        }


        public void CastSpell(Type spell, string name) {
            if (wand.gameObject.GetComponent(spell)) UnityEngine.GameObject.Destroy(wand.gameObject.GetComponent(spell));
            wand.gameObject.AddComponent(spell);
            
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
