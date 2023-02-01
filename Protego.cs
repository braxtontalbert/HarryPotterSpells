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
    class Protego : Spell
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

        public override Spell AddGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        public void Awake()
        {
            
            item = GetComponent<Item>();
            item.rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            item.rb.isKinematic = true;
            
            source = GetComponent<AudioSource>();
            source.loop = true;
            item.gameObject.AddComponent<IgnoreCollider>();
            LevelModuleScript.local.finiteSpells.Add(typeof(Protego));
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

}