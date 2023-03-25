using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;
using UnityEngine.VFX;
using static ThunderRoad.PlayerControl;

namespace WandSpellss
{
    class Stupefy : MonoBehaviour
    {
        public static SpellType spellType = SpellType.Shoot;
        GameObject toUpdate;

        public void SpellEffect(Creature creature)
        {
            creature.ragdoll.SetState(Ragdoll.State.Destabilized);
            creature.TryElectrocute(1, 3, true, false);
        }
        public void OnCollisionEnter(Collision c)
        {
            if (c.gameObject.GetComponentInParent<Creature>() is Creature creature)
            {
                SpellEffect(creature);
            }
            Loader.local.couroutineManager.StartCustomCoroutine(Loader.local.couroutineManager.SpawnSparkEffect(Loader.local.stupefySparks, c.contacts[0].point));
        }

        public IEnumerator SpawnSparkEffect(GameObject effect, Vector3 position)
        {
            effect.transform.position = position;
            toUpdate = GameObject.Instantiate(effect);
            

            toUpdate.GetComponentInChildren<VisualEffect>().Play();

            yield return new WaitForSeconds(3f);

            UnityEngine.GameObject.Destroy(toUpdate);

        }
    }
    public class StupefyHandler : Spell
    {
        public static SpellType spellType = SpellType.Shoot;
        public override Spell AddGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        public override void SpawnSpell(Type type, string name, Item wand,float spellSpeed)
        {
            Debug.Log("Got to spawn spell method");
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
