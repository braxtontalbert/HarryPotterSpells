using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;
namespace WandSpellss
{
    class Flagrate : MonoBehaviour
    {
        public static SpellType spellType = SpellType.Raycast;
        LineRenderer lineRenderer;
        Item item;
        bool allowStart;
        Vector3 startPoint;
        Vector3 lastPoint;
        List<Vector3> positions;
        void Start() {

            item = GetComponent<Item>();
            allowStart = false;
            lineRenderer = item.gameObject.AddComponent<LineRenderer>();
            lineRenderer.widthMultiplier = 0.004f;
            lineRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Unlit")) { color = Color.red };

            item.OnHeldActionEvent += Item_OnHeldActionEvent;
        }

        private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
        {
            if(action == Interactable.Action.AlternateUseStart)
            {
                startPoint = item.flyDirRef.transform.position;
                allowStart = true;
                
            }
            if(action == Interactable.Action.AlternateUseStop)
            {
                allowStart = false;
            }
        }

        public void Update() {

            if (allowStart)
            {
                CustomDebug.Debug("Got past allowStart");
                if(item.flyDirRef.transform.position != startPoint)
                {
                    lastPoint = startPoint;
                }
                CustomDebug.Debug("Got past start point check");
                float pointDistance = (item.flyDirRef.transform.position - lastPoint).magnitude;
                
                if (pointDistance >= 0.001f) {
                    if (!positions.Contains(lastPoint))
                    {
                        positions.Add(lastPoint);
                        positions.Add(item.flyDirRef.transform.position);
                    }
                    else positions.Add(item.flyDirRef.transform.position);

                    
                    lineRenderer.SetPositions(positions.ToArray());
                    lastPoint = item.flyDirRef.transform.position;
                }
            }

        
        }

    }

    public class FlagrateHandler : Spell
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
