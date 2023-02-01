using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ThunderRoad;
using UnityEngine;
using System.Collections;
using UnityEngine.VFX;

namespace WandSpellss
{
    class PetrificusTotalus : Spell
    {
        Item item;
        Item npcItem;
        GameObject enemy;
        internal AudioSource source;
        public System.Timers.Timer aTimer;
        Creature currentCreature;
        Animator prevAnimator;
        Animator freezeAnimator;
        AudioSource sourceCurrent;
        GameObject effect;

        public static SpellType spellType = SpellType.Shoot;

        public void Start()
        {
            item = GetComponent<Item>();
        }

        public override Spell AddGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        public void OnCollisionEnter(Collision c)
        {
            if (c.gameObject.GetComponentInParent<Creature>() is Creature creature) {
                currentCreature = creature;
                StartCoroutine(Timer(creature, c));

            }
            LevelModuleScript.local.couroutineManager.StartCustomCoroutine(SpawnSparkEffect(LevelModuleScript.local.petrificusSparks, c.contacts[0].point));

        }

        public IEnumerator SpawnSparkEffect(GameObject effect, Vector3 position)
        {

            effect.transform.position = position;
            effect = GameObject.Instantiate(effect);


            effect.GetComponentInChildren<VisualEffect>().Play();

            yield return new WaitForSeconds(3f);

            UnityEngine.GameObject.Destroy(effect);

        }

        IEnumerator Timer(Creature creature, Collision c) {

            currentCreature.StopAnimation();
            currentCreature.ToogleTPose();
            yield return new WaitForSeconds(7.5f);

            
            creature.StopAnimation();
            creature.ToogleTPose();
            enemy = c.gameObject;
        }




    }
}
