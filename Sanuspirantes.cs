using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    class Sanuspirantes : MonoBehaviour
    
    {
        Item item;
        internal GameObject parentLocal;
        GameObject bubble;
        GameObject bubbleUpdate;
        bool activated;
        readonly float defaultDamage = Player.currentCreature.data.waterDrowningDamage;
        readonly float defaultTime = Player.currentCreature.data.waterDrowningStartTime;
        public static SpellType spellType = SpellType.Raycast;
        int valuesChanged = 0;


        public void Start()
        {
            item = GetComponent<Item>();
            bubble = Loader.local.bubbleHeadEffect;
            activated = false;
            CastRay();
        }

        void Update() {

            if (bubbleUpdate) {

                bubbleUpdate.transform.position = Player.local.head.transform.position;
            }

            if (activated)
            {
                valuesChanged++;
                if (valuesChanged == 1)
                {
                    Player.currentCreature.data.waterDrowningDamage = defaultDamage;
                    Player.currentCreature.data.waterDrowningStartTime = float.MaxValue;
                    Loader.local.spellsOnPlayer.Add(typeof(Sanuspirantes));
                }

            }

        }

        
        void OnDestroy() {

            Player.currentCreature.data.waterDrowningDamage = defaultDamage;
            Player.currentCreature.data.waterDrowningStartTime = defaultTime;
            Loader.local.StartBubbleHeadDestroy(bubbleUpdate);
        }

        internal void CastRay()
        {
            RaycastHit hit;
            Transform parent;

            if (Physics.Raycast(item.flyDirRef.transform.position, item.flyDirRef.transform.forward, out hit, float.MaxValue, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.GetComponentInParent<Creature>() is Creature creature && creature == Player.currentCreature) {

                    bubbleUpdate = GameObject.Instantiate(bubble);
                    activated = true;
                }
            }
        }
    }
    public class SanuspiranteHandler : Spell
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
