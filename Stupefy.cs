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
    class Stupefy : Spell
    {
        public static SpellType spellType = SpellType.Shoot;
        GameObject toUpdate;
        public void Awake() {
            base.spellObject = GetComponent<Item>();
            spellType = SpellType.Shoot;
        }

        public override void SpellEffect(Creature creature)
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

            LevelModuleScript.local.couroutineManager.StartCustomCoroutine(SpawnSparkEffect(LevelModuleScript.local.stupefySparks, c.contacts[0].point));
            

        }

        public override Spell AddGameObject(GameObject gameObject)
        {
            
            return gameObject.AddComponent<Stupefy>();
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
}
