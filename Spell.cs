using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using UnityEngine.VFX;

using System.Reflection;
using System.Collections;
using static ThunderRoad.TutorialInteraction;

namespace WandSpellss
{
    public enum SpellType { Raycast, Shoot, Tip}
    public abstract class Spell : MonoBehaviour
    {
        public Item spellObject;
        public VisualEffect vfx;
        public static SpellType spellType;
        public readonly float spellSpeed = 10f;
        public virtual void SpellEffect(Creature creature) { }
        public virtual void SpellEffect() { }


        public abstract Spell AddGameObject(GameObject gameObject);
        public abstract void SpawnSpell(Type type, string name, Item wand, float spellSpeed);
        public abstract void UpdateSpell(Type type, string name, Item wand);

        public virtual void CastRay() { }

        
    }

    public static class SpellHandler
    {
        public static void SpawnSpell(Type handler, Type type, string name, Item wand,float spellSpeed) {

            Debug.Log("Invoking on wand");
            handler.GetMethod("SpawnSpell").Invoke(Activator.CreateInstance(handler), new object[] { type,name,wand,spellSpeed});
        }

        public static void UpdateSpell(Type handler, Type type, string name, Item wand) {

            handler.GetMethod("UpdateSpell").Invoke(Activator.CreateInstance(handler), new object[] { type, name, wand});
        }
    }
}
