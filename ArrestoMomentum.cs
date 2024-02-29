using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    class ArrestoMomentum : MonoBehaviour
    {
        public static SpellType spellType = SpellType.Raycast;
        bool canSlow;

        void Start() {
            Player.local.locomotion.OnGroundEvent += Locomotion_OnGroundEvent;
            Loader.local.spellsOnPlayer.Add(typeof(ArrestoMomentum));
            canSlow = true;

        }

        private void Locomotion_OnGroundEvent(Vector3 groundPoint, Vector3 velocity, Collider groundCollider)
        {
            canSlow = false;
        }

        public void StartArrestoMomentum() {

            foreach (Rigidbody rigidbody in Player.currentCreature.ragdoll.parts.Select(part => part.physicBody.rigidBody))
            {
                rigidbody.velocity = -Vector3.down * 5f;
            }


        }

        void Update() {
            if (canSlow) StartArrestoMomentum();
        }

    }

    public class ArrestoMomentumHandler : Spell
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
        public override void UpdateSpell(Type type, string name, Item wand, String itemType)
        {
            if (wand.gameObject.GetComponent(type)) UnityEngine.Object.Destroy(wand.gameObject.GetComponent(type));
            wand.gameObject.AddComponent(type);
        }
    }
}
