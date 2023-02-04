using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;
namespace WandSpellss
{
    class Flagrate : Spell
    {
        public static SpellType spellType = SpellType.Raycast;
        LineRenderer lineRenderer;
        Item item;
        bool allowStart;
        Vector3 startPoint;
        Vector3 lastPoint;
        List<Vector3> positions = new List<Vector3>();
        public override Spell AddGameObject(GameObject gameObject)
        {
            throw new NotImplementedException();
        }
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
                        CustomDebug.Debug("Got past checking empty positions");
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
}
