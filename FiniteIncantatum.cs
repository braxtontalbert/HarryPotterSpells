using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using static ThunderRoad.ItemMagicAreaProjectile;

namespace WandSpellss
{
    class FiniteIncantatum : MonoBehaviour
    {
        public static SpellType spellType = SpellType.Raycast;

        Item item;

        void Start() {

            item = GetComponent<Item>();

            CastRay();
        }

        new void CastRay() {


            if (Physics.Raycast(item.flyDirRef.transform.position, item.flyDirRef.transform.forward, out RaycastHit hit, float.MaxValue, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) {


                if (hit.collider.GetComponentInParent<Creature>() is Creature creature && creature == Player.currentCreature)
                {


                    foreach (Type spell in Loader.local.spellsOnPlayer)
                    {
                        UnityEngine.GameObject.Destroy(item.GetComponent(spell));
                    }
                }
                else if(hit.collider.GetComponent<Item>() is Item spellItem){

                    foreach (Type spellType in Loader.local.finiteSpells) { 
                    
                        if(spellItem.GetComponent(spellType) != null) UnityEngine.GameObject.Destroy(spellItem.gameObject);

                        break;
                    }
                
                }
                else if (hit.collider.GetComponentInChildren<Item>() is Item spellItem2)
                {

                    foreach (Type spellType in Loader.local.finiteSpells)
                    {

                        if (spellItem2.GetComponent(spellType) != null) UnityEngine.GameObject.Destroy(spellItem2.gameObject);

                        break;
                    }

                }
                else if (hit.collider.GetComponentInParent<Item>() is Item spellItem3)
                {

                    foreach (Type spellType in Loader.local.finiteSpells)
                    {

                        if (spellItem3.GetComponent(spellType) != null) UnityEngine.GameObject.Destroy(spellItem3.gameObject);

                        break;
                    }

                }

            }
        
        }
    }
    public class FiniteIncantatumHandler : Spell
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
            throw new NotImplementedException();
        }
    }

}
