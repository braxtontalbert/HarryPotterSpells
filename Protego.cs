using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using System.Collections;

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
        public void GetWand(Item item) {
            wand = item;
        }

        public void Awake()
        {
            item = GetComponent<Item>();
            item.rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            item.rb.isKinematic = true;
            
            source = GetComponent<AudioSource>();
            source.loop = true;
            item.gameObject.AddComponent<IgnoreCollider>();
            Loader.local.finiteSpells.Add(typeof(Protego));
            StartCoroutine(Timer());
        }


        public void OnTriggerEnter(Collider other) {

            if (other.gameObject.GetComponentInParent<AvadaKedavra>() && other.gameObject.GetComponentInParent<Item>() is Item itemParent)
            {
                itemParent.IgnoreObjectCollision(this.item);
            }
        }

        /*void Update() {

            if (item != null) {

                item.transform.position = wand.flyDirRef.transform.position;
                item.transform.rotation = wand.flyDirRef.transform.rotation;

            }
        
        }*/

        IEnumerator Timer() {


            yield return new WaitForSeconds(15f);

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
            try
            {
                Catalog.GetData<ItemData>(name + "Object")?.SpawnAsync(projectile =>
                {
                    projectile.gameObject.AddComponent(type);

                    projectile.transform.position = wand.flyDirRef.transform.position;
                    projectile.transform.rotation = wand.flyDirRef.transform.rotation;
                    projectile.IgnoreObjectCollision(wand);
                    projectile.IgnoreRagdollCollision(Player.currentCreature.ragdoll);

                    projectile.Throw();

                    projectile.rb.useGravity = false;
                    projectile.rb.drag = 0.0f;

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
            catch (NullReferenceException e) { Debug.Log(e.Message); }
        }

        public override void UpdateSpell(Type type, string name, Item wand)
        {
            throw new NotImplementedException();
        }
    }

}