using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using static ThunderRoad.HandPoseData;
using System.Collections;
using UnityEngine.VFX;


namespace WandSpellss
{
    class Flipendo : MonoBehaviour
    {
        public static SpellType spellType = SpellType.Shoot;
        Item item;

        void Start() { 
        
            item = GetComponent<Item>();

        }

        public void OnCollisionEnter(Collision c)
        {
            if (c.gameObject.GetComponentInParent<Creature>() is Creature creature)
            {

                creature.ragdoll.SetState(Ragdoll.State.Destabilized);
                foreach (Rigidbody rigidbody in c.gameObject.GetComponentInParent<Creature>().ragdoll.parts.Select(part => part.physicBody.rigidBody))
                {

                    CustomDebug.Debug("Rigidbody name: " + rigidbody.name);
                    rigidbody.AddForce(Vector3.up * 30f, ForceMode.Impulse);
                    if (rigidbody.name.Contains("Head"))
                    {
                        rigidbody.AddForce(item.flyDirRef.transform.forward * 10f, ForceMode.Impulse);   
                    }
                    
                }
            }
            Loader.local.couroutineManager.StartCustomCoroutine(SpawnSparkEffect(Loader.local.flipendoSparks, c.contacts[0].point));
        }
        public IEnumerator SpawnSparkEffect(GameObject effect, Vector3 position)
        {

            effect.transform.position = position;
            effect = GameObject.Instantiate(effect);


            effect.GetComponentInChildren<VisualEffect>().Play();

            yield return new WaitForSeconds(3f);

            UnityEngine.GameObject.Destroy(effect);

        }

        IEnumerator FloatingTimer(Rigidbody rb) {

            rb.useGravity = false;
            yield return new WaitForSeconds(2f);

            rb.useGravity = true;
        }
        
        void ExecuteFlipendo() {

        }
    }

    public class FlipendoHandler : Spell
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
    }
}
