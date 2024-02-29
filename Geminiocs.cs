using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    class Geminio : MonoBehaviour
    {
        Item item;
        internal GameObject parentLocal;
        internal bool cantEvanesco;
        GameObject duplicate;
        System.Random random;
        ItemData copyData;

        public static SpellType spellType = SpellType.Raycast;

        public void Start()
        {
            item = GetComponent<Item>();

            random = new System.Random();

            CastRay();

        }

        internal void CastRay()
        {


            RaycastHit hit;
            Transform parent;

            if (Physics.Raycast(item.flyDirRef.transform.position, item.flyDirRef.transform.forward, out hit, float.MaxValue, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {

                CustomDebug.Debug(hit.collider.gameObject.transform.parent.name);

                parent = hit.collider.gameObject.transform.parent;
                parentLocal = parent.gameObject;
                if (parentLocal.GetComponent<Item>() is Item item)
                {

                    copyData = item.data;
                    copyData.SpawnAsync(copy => {

                        double range = (double)-0.1 - (double)(-0.5);
                        double sample = random.NextDouble();
                        double scaled = (sample * range);
                        copy.transform.position = new Vector3(parent.gameObject.transform.position.x + (float)scaled, parent.gameObject.transform.position.y + (float)scaled, parent.gameObject.transform.position.z);



                    });
                }

                else if (parentLocal.GetComponentInParent<Item>() is Item item2)
                {

                    copyData = item2.data;
                    copyData.SpawnAsync(copy => {

                        double range = (double)-0.1 - (double)(-0.5);
                        double sample = random.NextDouble();
                        double scaled = (sample * range);
                        copy.transform.position = new Vector3(parent.gameObject.transform.position.x + (float)scaled, parent.gameObject.transform.position.y + (float)scaled, parent.gameObject.transform.position.z);



                    });
                }

                else if (parentLocal.GetComponentInChildren<Item>() is Item item3)
                {

                    copyData = item3.data;
                    copyData.SpawnAsync(copy => {

                        double range = (double)-0.1 - (double)(-0.5);
                        double sample = random.NextDouble();
                        double scaled = (sample * range);
                        copy.transform.position = new Vector3(parent.gameObject.transform.position.x + (float)scaled, parent.gameObject.transform.position.y + (float)scaled, parent.gameObject.transform.position.z);



                    });
                }





            }


        }


    }
    public class GeminioHandler : Spell
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