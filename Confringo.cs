using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ThunderRoad;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

namespace WandSpellss
{
    public class Confringo : MonoBehaviour
    {
        private VisualEffect vfx;
        private Item item;
        public Item wand;
        private bool hit = false;
        private float time = 1.5f;
        private float elapsedTime = 0f;
        float currentTime = 0f;
        private Vector3 hitLocation;
        private float distance;
        private float radius = 3f;
        Vector3 originalLocation;
        private void Start()
        {
            item = GetComponentInParent<Item>();
            
            //CastRay();
            
        }
        
        public void SpellEffect(Collision c)
        {
            Collider[] colliders = Physics.OverlapSphere(c.contacts[0].point, 3f);
            List<Creature> creaturesInColliders = new List<Creature>();
            List<Item> itemsInColliders = new List<Item>();
            foreach (Collider collider in colliders)
            {
                if(collider.GetComponentInParent<Creature>() is Creature creature && !creature.isPlayer && !creaturesInColliders.Contains(creature))
                {
                   creaturesInColliders.Add(creature);
                }
                if (collider.GetComponentInParent<Item>() is Item item && !itemsInColliders.Contains(item))
                {
                    itemsInColliders.Add(item);
                }
            }

            foreach (Creature creature in creaturesInColliders)
            {
                creature.ragdoll.SetState(Ragdoll.State.Destabilized);
                foreach (Rigidbody body in creature.ragdoll.parts.Select(part =>part.physicBody.rigidBody))
                {
                    float mass = body.mass;
                    body.AddExplosionForce(mass * 2000f, c.contacts[0].point, 3f, 30f);
                }
                
            }
            foreach (Item item in itemsInColliders)
            {
                
                float mass = item.physicBody.rigidBody.mass;
                item.physicBody.rigidBody.AddExplosionForce(mass * 2000f, c.contacts[0].point, 3f, 30f);
                
            }
            

            Loader.local.explosion.transform.position = c.contacts[0].point;
            Instantiate(Loader.local.explosion);
        }
        
        public void OnCollisionEnter(Collision c)
        {
            SpellEffect(c);
            
        }
        
        internal void CastRay() {

            
            RaycastHit hit;
            Transform parent;
            Debug.Log("Hit Cast Ray method");
            if (Physics.Raycast(wand.flyDirRef.transform.position, wand.flyDirRef.transform.forward, out hit, float.MaxValue, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                Debug.Log("Hit raycasted");
                Catalog.GetData<ItemData>("StupefyObject")?.SpawnAsync(projectile =>
                {
                    Debug.Log("Hit spawner");
                    this.item = projectile;
                    item.transform.position = wand.transform.position;
                    hitLocation = hit.collider.gameObject.transform.position;
                    distance = Vector3.Distance(wand.flyDirRef.transform.position, hitLocation);
                    this.hit = true;
                    if (projectile.gameObject.GetComponent<Expelliarmus>() is Expelliarmus armus)
                    {
                        Destroy(armus);
                    }
                });
                originalLocation = wand.flyDirRef.transform.position;

            }
            

        }


        Vector3 GetBezier(Vector3 p0, Vector3 p1, Vector3 p2, float time)
        {
            float tt = time * time;
            float u = 1f - time;
            float uu = u * u;

            Vector3 result = uu * p0;
            result += 2f * u * time * p1;
            result += tt * p2;

            return result;
        }

        Vector3 random;
        private bool randomFound = false;
        /*private void Update()
        {
            if (hit)
            {
                if (!randomFound)
                {
                    random = new Vector3(
                        Random.Range(originalLocation.x, hitLocation.x),
                        Random.Range((originalLocation + new Vector3(0, 1, 0)).y,
                            (originalLocation + new Vector3(0, -1, 0)).y),
                        Random.Range(originalLocation.z, hitLocation.z));
                    randomFound = true;
                }

                elapsedTime += Time.deltaTime;
                float percentageComplete = elapsedTime / (distance / time);
                Vector3 bezierCurrentPosition = GetBezier(originalLocation, random, hitLocation, time);
                Vector3 bezierNextPosition =
                    GetBezier(originalLocation, random, hitLocation, time + 0.01f);
                
                item.transform.position = Vector3.Lerp(bezierCurrentPosition, bezierNextPosition, Mathf.SmoothStep(0, 1, percentageComplete));

                if (percentageComplete >= 1f)
                {
                    Collider[] colliders = Physics.OverlapSphere(hitLocation, 3f);
                    foreach (Collider collider in colliders)
                    {

                        if (collider.GetComponentInParent<Rigidbody>() is Rigidbody body)
                        {
                            body.AddExplosionForce(5f, hitLocation, 3f, 1.5f);
                        }
                    }
                }
            }
        }*/
    }

    public class ConfringoHandler : Spell
    {
        public static SpellType spellType = SpellType.Shoot;
        public override Spell AddGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        public override void SpawnSpell(Type type, string name, Item wand, float spellSpeed)
        {
            Debug.Log("Got to spawn spell method");
            try
            {
                Catalog.GetData<ItemData>(name + "Object")?.SpawnAsync(projectile =>
                {
                    

                    projectile.transform.position = wand.flyDirRef.transform.position;
                    projectile.transform.rotation = wand.flyDirRef.transform.rotation;
                    projectile.IgnoreObjectCollision(wand);
                    projectile.IgnoreRagdollCollision(Player.currentCreature.ragdoll);
                    projectile.gameObject.AddComponent(type);
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