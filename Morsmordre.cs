using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ThunderRoad;
using UnityEngine;
using System.Collections;

namespace WandSpellss
{
    class Morsmordre : MonoBehaviour
    {

        Item item;
        internal ItemData darkMark;
        private Timer aTimer;

        public static SpellType spellType = SpellType.Shoot;

        public void Start() {

            item = GetComponent<Item>();

            darkMark = Catalog.GetData<ItemData>("TheDarkMark");
            StartCoroutine(Timer());
        }


        IEnumerator Timer() {

            
            yield return new WaitForSeconds(2.5f);

            darkMark.SpawnAsync(projectile => {

                projectile.gameObject.AddComponent<DarkMark>();
                projectile.transform.position = item.transform.position;
                projectile.physicBody.rigidBody.useGravity = false;
                projectile.physicBody.rigidBody.drag = 0.0f;

                item.Despawn();

            });

        }

    }

    public class MorsmordreHandler : Spell
    {
        public static SpellType spellType = SpellType.Shoot;

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

                    projectile.physicBody.rigidBody.useGravity = false;
                    projectile.physicBody.rigidBody.drag = 0.0f;

                    foreach (AudioSource c in wand.GetComponentsInChildren<AudioSource>())
                    {

                        if (c.name == name) c.Play();
                    }

                    projectile.GetComponent<Rigidbody>().AddForce(wand.flyDirRef.forward * spellSpeed, ForceMode.Impulse);
                    projectile.gameObject.AddComponent<SpellDespawn>();
                });
            }
            catch (NullReferenceException e) { Debug.Log(e.Message); }
        }

        public override void UpdateSpell(Type type, string name, Item wand)
        {
            throw new NotImplementedException();
        }
        public override void UpdateSpell(Type type, string name, Item wand, String itemType)
        {
            throw new NotImplementedException();
        }
    }
}
