using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using ThunderRoad.AI.Action;

namespace WandSpellss
{
    class BrainModuleHPWands : BrainModuleRanged
    {

        public Item wand;
        public float maxShootAngle = 1f;

        protected Coroutine offHandCouroutine;

        public Side side;
        public bool aimOnTarget;

        protected float preCoroutineOffset;

        public bool poseHand;


        public ItemModuleAI.RangedWeaponData mainWandData { get; protected set; }
        public Transform mainHandAnchor { get; protected set; }


        public override void Load(Creature creature)
        {
            base.Load(creature);
            this.mainHandAnchor = this.aimTransform.Find("MainHandAnchor");

            if (!(bool)(UnityEngine.Object)this.mainHandAnchor) {

                this.mainHandAnchor = new GameObject("MainHandAnchor").transform;
                this.mainHandAnchor.SetParent(this.aimTransform, false);
            
            }

        }

        public override ItemModuleAI.RangedWeaponData GetLaunchData() => this.creature.equipment.GetHeldItem(this.side)?.data?.moduleAI?.rangedWeaponData;


        public override void StartRangedAttack(Transform target, AttackRanged node = null, float aimDelay = -1)
        {
            

            if (this.state != BrainModuleRanged.AttackState.None) return;

            base.StartRangedAttack(target, node);
            this.moduleMove.TopExclusiveStack((object) this);
            Item wandMain = this.wand;

            int num1;

            if (wandMain == null)
            {

                num1 = 0;

            }

            else {


                ItemModuleAI.WeaponHandling? weaponHandling = wandMain.data?.moduleAI?.weaponHandling;
                ItemModuleAI.WeaponHandling? weaponHandling2 = ItemModuleAI.WeaponHandling.TwoHanded;

                num1 = weaponHandling.GetValueOrDefault() == weaponHandling2 & weaponHandling.HasValue ? 1: 0;
            
            }

        }
    }
}
