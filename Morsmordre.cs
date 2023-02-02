using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ThunderRoad;
using UnityEngine;
using System.Collections;

namespace WandSpellss
{
    class Morsmordre : Spell
    {

        Item item;
        internal ItemData darkMark;
        private Timer aTimer;

        public static SpellType spellType = SpellType.Shoot;

        public override Spell AddGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        public void Start() {

            item = GetComponent<Item>();

            darkMark = Catalog.GetData<ItemData>("TheDarkMark");
            StartCoroutine(Timer());
        }


        IEnumerator Timer() {

            
            yield return new WaitForSeconds(2.5f);

            darkMark.SpawnAsync(projectile => {

                projectile.gameObject.AddComponent<DarkMark>();
                projectile.transform.position = item.transform.position;
                projectile.rb.useGravity = false;
                projectile.rb.drag = 0.0f;

                item.Despawn();

            });

        }

    }
}
