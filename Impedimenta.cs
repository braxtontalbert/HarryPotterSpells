using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.VFX;


namespace WandSpellss
{
    class Impedimenta : MonoBehaviour
    {
        public static SpellType spellType = SpellType.Raycast;
        
        ThunderRoad.Item item;
        GameObject go;
        private GameObject sfx;
        private GameObject despawner;
        private GameObject sender;

        void Start() {
            item = GetComponent<Item>();
            StartImpedimenta();
            if(go) Loader.local.couroutineManager.StartCustomCoroutine(DestroyImpedimentaEffect(go));
            
        }
        void StartImpedimenta() {
            foreach (var creature in Creature.allActive)
            {
                if((Player.currentCreature.transform.position - creature.transform.position).sqrMagnitude < 5f * 5f)
                {
                    creature.locomotion.SetSpeedModifier(this, 0.3f, 0.3f, 0.3f, 0.3f, 0.3f);
                    Debug.Log("Creature animator speed default: " + creature.animator.speed);
                    creature.animator.speed = 0.3f;
                    creature.gameObject.AddComponent<CreaturesReversalEvent>();
                }
            }
            sfx = Instantiate(Loader.local.impedimentaSoundFX);
            Loader.local.impedimentaEffect.transform.position = item.flyDirRef.transform.position;
            go = GameObject.Instantiate(Loader.local.impedimentaEffect);

        }

        IEnumerator DestroyImpedimentaEffect(GameObject effect) {

            yield return new WaitForSeconds(5f);
            UnityEngine.GameObject.Destroy(effect);
        }
    }

    public class CreaturesReversalEvent : MonoBehaviour
    {
        private Creature creature;


        private void Start()
        {
            creature = GetComponentInParent<Creature>();
            creature.OnKillEvent += Target_OnKillEvent;
        }

        private void Target_OnKillEvent(CollisionInstance collisionInstance, EventTime eventTime)
        {
            creature.animator.speed = 1f;
            creature.locomotion.ClearSpeedModifiers();
            Destroy(this);
        }
        
    }

    public class ImpedimentaHandler : Spell
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
