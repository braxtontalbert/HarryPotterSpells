using System;
using System.Collections;
using UnityEngine;
using ThunderRoad;
using UnityEngine.VFX;

namespace WandSpellss
{
    public class Levioso : MonoBehaviour
    {
        private GameObject leviosoUpdate;
        private Rigidbody currentCreature;
        private bool startLevitate;
        private GameObject go;
        private Vector3 position;
        public void Start()
        {
            go = new GameObject();
            startLevitate = false;
            leviosoUpdate = new GameObject();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponentInParent<Creature>() is Creature creature)
            {
                creature.ragdoll.SetState(Ragdoll.State.Destabilized);
                currentCreature = creature.ragdoll.targetPart.physicBody.rigidBody;
                startLevitate = true;
                position = creature.ragdoll.targetPart.transform.position;
            }
            else if(collision.gameObject.GetComponentInParent<Item>() is Item item)
            {
                currentCreature = item.physicBody.rigidBody;
                startLevitate = true;
                position = item.transform.position;
            }
            go.AddComponent<LeviosoUpdate>().Setup(startLevitate, currentCreature,position);

            Loader.local.couroutineManager.StartCustomCoroutine(SpawnSparkEffect(Loader.local.leviosoSparks, collision.contacts[0].point));
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

    public class LeviosoUpdate : MonoBehaviour
    {
        private Rigidbody rigidbody;
        private bool startLevitate;
        private Vector3 position;

        private void Start()
        {
            Loader.local.couroutineManager.StartCustomCoroutine(Loader.local.couroutineManager.StopLeviate(this.gameObject));
        }

        public void Setup(bool startLevitate, Rigidbody rigid, Vector3 pos)
        {
            this.rigidbody = rigid;
            this.startLevitate = startLevitate;
            this.position = pos;
        }

        private void Update()
        {
            if (rigidbody)
            {
                rigidbody.velocity = ((position + (Vector3.up * 4f)) - rigidbody.transform.position) * 5f;
            }
        }
    }
    public class LeviosoHandler : Spell
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