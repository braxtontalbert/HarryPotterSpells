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
    class AvadaKedavra : Spell
    {
        Item item;
        internal ItemData avadaLightning;
        internal AudioSource source;
        Item lightningItem;
        public Creature hitCreatures;
        public GameObject effect;
        public static SpellType spellType = SpellType.Shoot;
        public void Awake()
        {
            item = GetComponent<Item>();
            
           
            
        }

        public override Spell AddGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        public void OnCollisionEnter(Collision c)
        {
            if (c.gameObject.GetComponentInParent<Creature>() is Creature creature)
            {
                creature.Kill();
            }

            LevelModuleScript.local.couroutineManager.StartCustomCoroutine(SpawnSparkEffect(LevelModuleScript.local.avadaSparks, c.contacts[0].point));
            
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
