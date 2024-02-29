using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using UnityEngine.VFX;
using System.Collections;

namespace WandSpellss
{
    class Tarantallegra : MonoBehaviour
    {
        internal AnimationData animationData;
        System.Random random = new System.Random();
        GameObject effect;

        void Start() {

            animationData = Catalog.GetData<AnimationData>("HPSDances");
        
        }

        public static SpellType spellType = SpellType.Shoot;

        void OnCollisionEnter(Collision c) {


            if (c.gameObject.GetComponentInParent<Creature>() is Creature creature) {

                int index = random.Next(0, animationData.animationClips.Count - 1);

                creature.PlayAnimation(animationData.animationClips[index].animationClip,false);

            }

            Loader.local.couroutineManager.StartCustomCoroutine(SpawnSparkEffect(Loader.local.tarantallegraSparks, c.contacts[0].point));

        }

        public IEnumerator SpawnSparkEffect(GameObject effect, Vector3 position)
        {

            effect.transform.position = position;
            effect = GameObject.Instantiate(effect);


            effect.GetComponentInChildren<VisualEffect>().Play();

            yield return new WaitForSeconds(3f);

            UnityEngine.GameObject.Destroy(effect);

        }
    }
    public class TarantallegraHandler : Spell
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
