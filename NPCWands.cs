using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using System.Timers;

namespace WandSpellss
{
    class NPCWands : BrainData.Module
    {

        Creature npc;
        Player player;
        Creature target;
        float distanceSqr;
        float attackRange = 15f;
        Item weapon;
        AudioSource sourceCurrent;
        bool playSound;
        AnimationData animation;
        bool isAttacking;
        private Timer aTimer;

        public override void Load(Creature creature)
        {
            base.Load(creature);
            npc = creature;
            npc.brain.OnAttackEvent += Brain_OnAttackEvent;
            playSound = false;
            animation = Catalog.GetData<AnimationData>("HumanWandCast");
            isAttacking = false;

            Debug.Log("This is the animation: " + animation.animationClips[0].animationClip);
        }


        public override void Update()
        {
            base.Update();
            if (npc.isPlayingDynamicAnimation == false) {

                isAttacking = false;
            
            
            }
            if (npc.handRight.grabbedHandle != null) {


                weapon = npc.handRight.grabbedHandle.item;
            }
            distanceSqr = (npc.transform.position - Player.local.transform.position).sqrMagnitude;

            if (distanceSqr <= attackRange && isAttacking == false) {

                //weapon.transform.LookAt(Player.local.transform);
                npc.brain.InvokeAttackEvent(Brain.AttackType.Cast,false,Player.local.creature);
                
            }

            
        }

        private void Brain_OnAttackEvent(Brain.AttackType attackType, bool strong, Creature target)
        {
            isAttacking = true;
            npc.PlayAnimation(animation.animationClips[0].animationClip,false);
            if (distanceSqr <= attackRange)
            {

                if (weapon.GetComponent<MyWeaponComponent>() != null)
                {


                    weapon.GetComponent<MyWeaponComponent>().spellsList[0].SpawnAsync(projectile => {


                        projectile.transform.position = weapon.flyDirRef.position;
                        projectile.transform.rotation = weapon.flyDirRef.rotation;
                        // Same as usual
                        projectile.IgnoreRagdollCollision(npc.ragdoll);
                        projectile.IgnoreObjectCollision(weapon);
                        projectile.Throw();



                        projectile.gameObject.AddComponent<Stupefy>();

                        projectile.rb.useGravity = false;
                        projectile.rb.drag = 0.0f;

                        //currentShooters = projectile;

                        //Add the force in the direction of the flyDirRef (the blue axis in unity)
                        projectile.rb.AddForce(weapon.flyDirRef.transform.forward * 5f, ForceMode.Impulse);
                        projectile.gameObject.AddComponent<SpellDespawn>();

                        foreach (AudioSource c in weapon.GetComponentsInChildren<AudioSource>())
                        {
                            switch (c.name)
                            {
                                case "StupefySound":
                                    sourceCurrent = c;
                                    break;
                            }

                        }

                        sourceCurrent.Play();
                        playSound = true;


                    });


                }

                SetTimer();

            }
        }

        private void SetTimer()
        {

            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(5000);
            // Hook up the Elapsed event for the timer. 

            aTimer.Elapsed += OnTimedEvent;

            aTimer.AutoReset = false;
            aTimer.Enabled = true;

        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            isAttacking = false;
        }
    }
}
