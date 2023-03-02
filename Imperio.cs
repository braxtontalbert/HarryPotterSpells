using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using UnityEngine.VFX;

namespace WandSpellss
{
    public class Imperio : MonoBehaviour
    {
        Item item;
        private GameObject go;
        private GameObject visible;
        
        public void Start()
        {
            item = GetComponent<Item>();
            go = Instantiate(Loader.local.imperioEffect);
            visible = Instantiate(Loader.local.imperioShown);
            go.GetComponent<ParticleSystem>().Play();
            visible.GetComponent<ParticleSystem>().Play();
            go.gameObject.AddComponent<SpellParticles>();

        }

        void Update()
        {
            if (go && visible)
            {
                go.transform.rotation = item.flyDirRef.transform.rotation;
                go.transform.position = item.flyDirRef.transform.position;
                visible.transform.rotation = item.flyDirRef.transform.rotation;
                visible.transform.position = item.flyDirRef.transform.position;
            }
        }

        
    }

    public class OnCreature : MonoBehaviour
    {
        
    }

    public class SpellParticles : MonoBehaviour
    {
        private Creature creature;
        private List<ParticleCollisionEvent> collisionEvents;
        private ParticleSystem ps;
        void Start()
        {
            ps = this.GetComponent<ParticleSystem>();
        }

        void OnParticleCollision(GameObject other)
        {
            Debug.Log("Particle Collided!");
            if (creature) return;
            creature = other.GetComponentInParent<Creature>();
            if (!creature.GetComponent<OnCreature>())
            {
                creature.gameObject.AddComponent<OnCreature>();
                StartImperio(creature);
            }
            /*int numCollisionEvents = go.GetComponent<ParticleSystem>().GetCollisionEvents(other, collisionEvents);

            Creature creature = other.GetComponentInParent<Creature>();
            

            if (numCollisionEvents > 10)
            {
                StartImperio(creature);
            }*/
        }
        void StartImperio(Creature creature)
        {
            Loader.local.couroutineManager.StartCustomCoroutine(Loader.local.couroutineManager.ImperioCounterCurse(creature, creature.factionId));
            creature.SetFaction(2);
            creature.brain.Load(creature.brain.instance.id);
            
        }
    }

    public class ImperioHandler : Spell
    {
        public static SpellType spellType = SpellType.Raycast;
        public override Spell AddGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        public override void SpawnSpell(Type type, string name, Item wand, float spellSpeed)
        {
            throw new NotImplementedException();
        }

        public override void UpdateSpell(Type type, string name, Item wand)
        {
            if (wand.gameObject.GetComponent(type)) UnityEngine.Object.Destroy(wand.gameObject.GetComponent(type));
            wand.gameObject.AddComponent(type);
        }
    }
}
