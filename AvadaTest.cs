using System;
using ThunderRoad;
using UnityEngine;
using UnityEngine.VFX;

namespace WandSpellss
{
    public class AvadaTest : MonoBehaviour
    {
        private VisualEffect vfx;
        private Item item;
        private GameObject go;
        public void Start()
        {
            item = GetComponent<Item>();
            go = GameObject.Instantiate(Loader.local.avadaTest);
            go.transform.position = item.flyDirRef.position;
            go.transform.rotation = item.flyDirRef.rotation;
            vfx = go.GetComponentInChildren<VisualEffect>();
            vfx.Play();
        }
    }
    
    public class AvadaTestHandler : Spell
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