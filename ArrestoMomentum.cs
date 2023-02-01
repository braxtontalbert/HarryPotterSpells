using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    class ArrestoMomentum : Spell
    {
        public static SpellType spellType = SpellType.Raycast;
        bool canSlow;

        public override Spell AddGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        void Start() {
            Player.local.locomotion.OnGroundEvent += Locomotion_OnGroundEvent;
            LevelModuleScript.local.spellsOnPlayer.Add(typeof(ArrestoMomentum));
            canSlow = true;

        }

        private void Locomotion_OnGroundEvent(Vector3 groundPoint, Vector3 velocity, Collider groundCollider)
        {
            canSlow = false;
        }

        public void StartArrestoMomentum() {

            foreach (Rigidbody rigidbody in Player.currentCreature.ragdoll.parts.Select(part => part.rb))
            {
                rigidbody.velocity = -Vector3.down * 5f;
            }


        }

        void Update() {
            if (canSlow) StartArrestoMomentum();
        }

    }
}
