using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
    public class Stupefy : SpellCastProjectile
    {
        public bool isCasting;
        public float spellSpeed;
        public Item item;
        public Item weaponItem;

        public Stupefy(Item weaponItem)
        {
            isCasting = true;
            this.weaponItem = weaponItem;
            Catalog.GetData<ItemData>("StupefyObject").SpawnAsync(new System.Action<Item>(this.SpawnStupefy));
        }

        public override void UpdateCaster()
        {
            base.UpdateCaster();
            this.item = this.spellCaster.ragdollHand.grabbedHandle?.item;
            if (isCasting)
            {
                // Run once per frame while the spell is being cast

                // --- REMOVE ME ---
                // Useful stuff:
                // The spellCaster object with info about the player
                Debug.Log(spellCaster);
                // The center of the spell casting orb
                Debug.Log(spellCaster.magicSource);
                // The caster's amount of mana
                Debug.Log(spellCaster.mana.currentMana);
                // The current charge of the spell
                Debug.Log(currentCharge);
            }
        }
        public void SpawnStupefy(Item spawnedItem)
        {

            UnityEngine.Vector3 vector3;
            vector3.x = UnityEngine.Random.Range(-0.15f, 0.15f);
            vector3.y = UnityEngine.Random.Range(-0.15f, 0.15f);
            vector3.z = UnityEngine.Random.Range(-0.15f, 0.15f);

            spawnedItem.transform.position = new UnityEngine.Vector3(weaponItem.transform.position.x,
                                                                         weaponItem.transform.position.y,
                                                                         weaponItem.transform.position.z);

            spawnedItem.rb.useGravity = false;
            spawnedItem.rb.drag = 0.0f;
            spawnedItem.rb.AddForce(weaponItem.transform.forward * this.spellSpeed, ForceMode.Impulse);
            spawnedItem.RefreshCollision(true);
            spawnedItem.IgnoreRagdollCollision(Player.currentCreature.ragdoll);

        }
        public override void Throw(Vector3 velocity)
        {
            base.Throw(velocity);
            // Run when the spell is thrown (think fireball / gravity throws)
        }

        public override void OnSprayStart()
        {
            base.OnSprayStart();
            // Run when the spell begins to be sprayed (think lightning)
        }

        public override void OnSprayLoop()
        {
            base.OnSprayLoop();
            // Run once per frame as the spell is being sprayed
        }

    }

}
}