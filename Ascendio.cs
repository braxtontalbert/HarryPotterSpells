using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;
namespace WandSpellss
{
    class Ascendio : MonoBehaviour
    {
        Item item;
        Creature player;
        float ascendioPower;
        float ascendioDefault;

        public static SpellType spellType = SpellType.Raycast;

        public void Start() {

            item = GetComponent<Item>();

            player = Player.local.creature;
            ascendioDefault = 2000f;
            ascendioPower = ascendioDefault;
            Player.local.creature.waterHandler.OnWaterEnter += WaterHandler_OnWaterEnter;
            Player.local.creature.waterHandler.OnWaterExit += WaterHandler_OnWaterExit;

            Ascend();
        }

        private void WaterHandler_OnWaterExit()
        {
            ascendioPower = ascendioDefault;
        }

        private void WaterHandler_OnWaterEnter()
        {
            ascendioPower = ascendioDefault * 2f;
        }

        public void Ascend() {

            
            foreach (Rigidbody rigidbody in player.ragdoll.parts.Select(part => part.physicBody.rigidBody)) {

                if (rigidbody != null)
                {

                    rigidbody.AddForce(item.flyDirRef.transform.forward * ascendioPower, ForceMode.Impulse);

                }

            
            }


        }


    }

    public class AscendioHandler : Spell
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
    }
}
