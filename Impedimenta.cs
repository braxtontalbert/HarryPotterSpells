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
            item = GetComponent<ThunderRoad.Item>();
            StartImpedimenta();
            if(go) Loader.local.couroutineManager.StartCustomCoroutine(DestroyImpedimentaEffect(go));
            
        }

        void StartImpedimenta() {
            List<Creature> foundTargets = new List<Creature>();
            try
            {
                foundTargets = Creature.allActive.Where(creature => !creature.isPlayer && (Player.currentCreature.transform.position - creature.transform.position).sqrMagnitude < 5f * 5f).ToList();
                CustomDebug.Debug("Target list size is: " + foundTargets.Count );
            } catch { }
            if (foundTargets.Count <= 0) return;
            foreach (Creature target in foundTargets)
            {
                CustomDebug.Debug("Got into loop");
                target.locomotion.SetSpeedModifier(this, 0.3f, 0.3f, 0.3f, 0.3f, 0.3f);
                target.animator.speed = 0.3f;
            }

            sender = new GameObject();
            sender.AddComponent<CreaturesReversalEvent>().Setup(foundTargets);
            sfx = Instantiate(Loader.local.impedimentaSoundFX);
            go = GameObject.Instantiate(Loader.local.impedimentaEffect);
            despawner = Instantiate(sender);
            go.transform.position = item.flyDirRef.position;

        }

        IEnumerator DestroyImpedimentaEffect(GameObject effect) {

            yield return new WaitForSeconds(5f);
            UnityEngine.GameObject.Destroy(effect);
        }
    }

    public class CreaturesReversalEvent : MonoBehaviour
    {
        private List<Creature> creature;

        public void Setup(List<Creature> creature)
        {
            this.creature = creature.DeepCopyByExpressionTree();
            
        }

        private void Start()
        {
            if (creature != null)
            {
                foreach (Creature temp in creature)
                {
                    temp.OnKillEvent += Target_OnKillEvent;
                }
                
            }
        }

        private void Target_OnKillEvent(CollisionInstance collisionInstance, EventTime eventTime)
        {
            collisionInstance.targetCollider.GetComponentInParent<Creature>().animator.speed = 100f;
            collisionInstance.targetCollider.GetComponentInParent<Creature>().locomotion.RemoveSpeedModifier(this);
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
