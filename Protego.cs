﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using System.Collections;
using UnityEngine.VFX;

namespace WandSpellss
{
    class Protego : MonoBehaviour
    {
        Item item;
        Item wand;
        Item npcItem;
        internal AudioSource source;
        public static SpellType spellType = SpellType.Shoot;
        internal AudioSource sourceCurrent;
        private GameObject go;
        private GameObject GO;
        private VisualEffect vfx;
        private bool playing;
        private bool buttonPressed;
        public void GetWand(Item item)
        {
            wand = item;
            wand.OnHeldActionEvent += HeldActionEvent;
        }
        
        public void Start()
        {
            item = GetComponent<Item>();
            buttonPressed = false;
            item.transform.position = wand.flyDirRef.transform.position;
        }

        private void HeldActionEvent(RagdollHand ragdollhand, Handle handle, Interactable.Action action)
        {
            if (action == Interactable.Action.AlternateUseStart)
            {
                item.Despawn();
            }
        }


        public void OnTriggerEnter(Collider other) {

            if (other.gameObject.GetComponentInParent<AvadaKedavra>() && other.gameObject.GetComponentInParent<Item>() is Item itemParent)
            {
                itemParent.IgnoreObjectCollision(this.item);
            }
        }

        void Update() {
            item.transform.position = wand.flyDirRef.transform.position;
            item.transform.rotation = wand.flyDirRef.transform.rotation;
        
        }

        IEnumerator Timer() {
            yield return new WaitForSeconds(3f);
            Loader.local.protegoSpawned = false;
            item.Despawn();
        }
    }

    public class ProtegoHandler : Spell
    {
        public static SpellType spellType = SpellType.Shoot;

        private float expelliarmusPower = 30f;
        //AudioSource sourceCurrent;

        public override Spell AddGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        public override void SpawnSpell(Type type, string name, Item wand, float spellSpeed)
        {
            Catalog.GetData<ItemData>(name + "Object")?.SpawnAsync(projectile =>
            {
                Loader.local.protegoSpawned = true;
                projectile.gameObject.AddComponent(type);


                projectile.transform.position = wand.flyDirRef.transform.position;
                projectile.transform.rotation = wand.flyDirRef.transform.rotation;
                projectile.IgnoreObjectCollision(wand);
                projectile.IgnoreRagdollCollision(Player.currentCreature.ragdoll);

                projectile.Throw();

                projectile.physicBody.rigidBody.useGravity = false;
                projectile.physicBody.rigidBody.drag = 0.0f;

                foreach (AudioSource c in wand.GetComponentsInChildren<AudioSource>())
                {

                    if (c.name == name) c.Play();
                }

                if (projectile.gameObject.GetComponent<Protego>() is Protego protego)
                {
                    protego.GetWand(wand);
                }
            });
        }

        public override void UpdateSpell(Type type, string name, Item wand)
        {
            /*if (wand.gameObject.GetComponent(type)) UnityEngine.Object.Destroy(wand.gameObject.GetComponent(type));
            wand.gameObject.AddComponent(type);*/
            throw new NotImplementedException();
        }
        public override void UpdateSpell(Type type, string name, Item wand, String itemType)
        {
            throw new NotImplementedException();
        }
    }

}