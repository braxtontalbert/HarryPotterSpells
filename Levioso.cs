using System;
using UnityEngine;
using ThunderRoad;

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
                currentCreature = creature.ragdoll.targetPart.rb;
                startLevitate = true;
                position = creature.ragdoll.targetPart.transform.position;
            }
            else if(collision.gameObject.GetComponentInParent<Item>() is Item item)
            {
                currentCreature = item.rb;
                startLevitate = true;
                position = item.transform.position;
            }
            go.AddComponent<LeviosoUpdate>().Setup(startLevitate, currentCreature,position);
            leviosoUpdate = GameObject.Instantiate(go);

        }
    }

    public class LeviosoUpdate : MonoBehaviour
    {
        private Rigidbody rigidbody;
        private bool startLevitate;
        private Vector3 position;
        
        public void Setup(bool startLevitate, Rigidbody rigid, Vector3 pos)
        {
            this.rigidbody = rigid;
            this.startLevitate = startLevitate;
            this.position = pos;
        }

        private void Update()
        {
            
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

                    projectile.rb.useGravity = false;
                    projectile.rb.drag = 0.0f;
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