using System;
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
    class Evertestatum : MonoBehaviour
    {
        Item item;
        Item npcItem;
        internal AudioSource source;
        GameObject effect;
        public Vector3 currentPosition;

        internal float power;

        public void Start()
        {
            item = GetComponent<Item>();
        }

        public void OnCollisionEnter(Collision c)
        {

            if (c.gameObject.GetComponentInParent<Creature>() is Creature creature)
            {
                creature.ragdoll.SetState(Ragdoll.State.Destabilized);
                foreach (Rigidbody rigidbody in creature.ragdoll.parts.Select(part => part.physicBody.rigidBody))
                {
                    var direction = (c.contacts[0].point - currentPosition).normalized;
                    rigidbody.AddForce(direction * (power), ForceMode.Impulse);
                    rigidbody.AddForce(Vector3.up * (power / 1.5f), ForceMode.Impulse);
                }


            }
            

            Loader.local.couroutineManager.StartCustomCoroutine(SpawnSparkEffect(Loader.local.evertestatumSparks, c.contacts[0].point));

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
    public class EvertestatumHandler : Spell
    {
        public static SpellType spellType = SpellType.Shoot;
        private float evertestatumPower = 50f;

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

                    if (projectile.gameObject.GetComponent<Evertestatum>() is Evertestatum exp)
                    {
                        exp.power = evertestatumPower;
                        exp.currentPosition = wand.flyDirRef.position;

                    }

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