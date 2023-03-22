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
    class Levicorpus : MonoBehaviour
    {
        Item item;
        Item npcItem;
        GameObject floater1;
        GameObject floater2;
        SpringJoint joint;
        SpringJoint joint2;
        internal Item spawnerWeapon;
        Creature despawnCreature;
        GameObject effect;

        public static SpellType spellType = SpellType.Shoot;

        public void Start()
        {
            item = GetComponent<Item>();


        }
        public void OnCollisionEnter(Collision c)
        {


            if (c.gameObject.GetComponentInParent<Creature>() is Creature creature)
            {

                despawnCreature = creature;


                floater1 = new GameObject();
                floater1.AddComponent<Rigidbody>();
                floater1.GetComponent<Rigidbody>().useGravity = false;

                
                floater2 = new GameObject();
                floater2.AddComponent<Rigidbody>();
                floater2.GetComponent<Rigidbody>().useGravity = false;

                creature.ragdoll.SetState(Ragdoll.State.Destabilized);
                joint =  creature.footLeft.gameObject.AddComponent<SpringJoint>();
                joint2 = creature.footRight.gameObject.AddComponent<SpringJoint>();

                floater1.transform.position = new Vector3(creature.ragdoll.headPart.transform.position.x, creature.ragdoll.headPart.transform.position.y + 2f, creature.ragdoll.headPart.transform.position.z);
                floater2.transform.position = new Vector3(creature.ragdoll.headPart.transform.position.x, creature.ragdoll.headPart.transform.position.y + 2f, creature.ragdoll.headPart.transform.position.z);


                joint.connectedBody = floater1.GetComponent<Rigidbody>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = new Vector3(0,0,0);
                joint.spring = 3000f;
                joint.damper = 100f;


                joint2.connectedBody = floater2.GetComponent<Rigidbody>();
                joint2.autoConfigureConnectedAnchor = false;
                joint2.connectedAnchor = new Vector3(0, 0, 0);
                joint2.spring = 3000f;
                joint2.damper = 100f;


                floater1.AddComponent<FixedJoint>();
                floater2.AddComponent<FixedJoint>();


                Loader.local.floaters.Add(floater1);
                Loader.local.floaters.Add(floater2);
                Loader.local.levicorpusedCreatures.Add(creature);

                creature.OnDespawnEvent += Creature_OnDespawnEvent;
            }

            Loader.local.couroutineManager.StartCustomCoroutine(SpawnSparkEffect(Loader.local.levicorpusSparks, c.contacts[0].point));

        }

        public IEnumerator SpawnSparkEffect(GameObject effect, Vector3 position)
        {

            effect.transform.position = position;
            effect = GameObject.Instantiate(effect);


            effect.GetComponentInChildren<VisualEffect>().Play();

            yield return new WaitForSeconds(3f);

            UnityEngine.GameObject.Destroy(effect);

        }
        private void Creature_OnDespawnEvent(EventTime eventTime)
        {
            if (despawnCreature.footLeft.gameObject.GetComponent<SpringJoint>() != null && despawnCreature.footRight.gameObject.GetComponent<SpringJoint>() != null)
            {
                CustomDebug.Debug("Got past double foot check.");
                Destroy(despawnCreature.footLeft.gameObject.GetComponent<SpringJoint>());

                Destroy(despawnCreature.footRight.gameObject.GetComponent<SpringJoint>());
            }
        }
    }

    public class LevicorpusHandler : Spell
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
    }

}




