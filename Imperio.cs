using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    public class Imperio : MonoBehaviour
    {
        Item item;
        private GameObject go;
        public void Start()
        {
            item = GetComponent<Item>();
            go = Instantiate(Loader.local.imperioEffect);
        }

        private void Update()
        {
            if (go)
            {
                go.transform.position = item.flyDirRef.transform.position;
                go.transform.rotation = item.flyDirRef.transform.rotation;
            }
        }
    }
    public class ImperioHandler : Spell
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
