using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using UnityEngine.VFX;
using System.Collections;

namespace WandSpellss
{
    class Tarantallegra : Spell
    {
        internal AnimationData animationData;
        System.Random random = new System.Random();
        GameObject effect;

        void Start() {

            animationData = Catalog.GetData<AnimationData>("HPSDances");
        
        }

        public static SpellType spellType = SpellType.Shoot;
        public override Spell AddGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        void OnCollisionEnter(Collision c) {


            if (c.gameObject.GetComponentInParent<Creature>() is Creature creature) {

                int index = random.Next(0, animationData.animationClips.Count - 1);

                creature.PlayAnimation(animationData.animationClips[index].animationClip,false);

            }

            LevelModuleScript.local.couroutineManager.StartCustomCoroutine(SpawnSparkEffect(LevelModuleScript.local.tarantallegraSparks, c.contacts[0].point));

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
}
