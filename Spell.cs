using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using UnityEngine.VFX;

namespace WandSpellss
{
    public enum SpellType { Raycast, Shoot, Tip}
    abstract class Spell : MonoBehaviour
    {
        public Item spellObject;
        public VisualEffect vfx;
        public static SpellType spellType;


        public virtual void SpellEffect(Creature creature) { }
        public virtual void SpellEffect() { }


        public abstract Spell AddGameObject(GameObject gameObject);

        public virtual void CastRay() { }

        
    }
}
