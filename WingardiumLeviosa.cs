using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using System.Collections;
using UnityEngine.VFX;

namespace WandSpellss
{
    class WingardiumLeviosa : MonoBehaviour
    {
        Item item;
        Item wand;
        internal bool canLift;
        internal GameObject parentLocal;
        Vector3 radius;
        Vector3 direction;
        float distance;
        Rigidbody currentRigidbody;
        Creature currentCreature;
        private GameObject go;
        private VisualEffect vfx;
        private Texture3D sdf;
        private Mesh itemMesh;

        public static SpellType spellType = SpellType.Raycast;

       
        public void Start()
        {
            item = GetComponent<Item>();
            canLift = false;
            item.OnHeldActionEvent += Item_OnHeldActionEvent;

            CastRay();
        }


        private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
        {
            if (action == Interactable.Action.AlternateUseStart && canLift == true) {

                canLift = false;
                currentRigidbody = null;
            }
        }

        void SpawnVFX()
        {
            go = Instantiate(Loader.local.wingardiumLeviosaEffect);
            vfx = go.GetComponentInChildren<VisualEffect>();
            
            /*Debug.Log(currentRigidbody.GetComponentInParent<Mesh>());
            Debug.Log(currentRigidbody.GetComponent<Mesh>());*/
            if (itemMesh)
            {
                Loader.local.sdfg.mesh = itemMesh;
                sdf = Loader.local.sdfg.Generate();
                Debug.Log("SDF: " + sdf);
                vfx.SetTexture("sdf", sdf);
            }
        }

        void UpdateVFX()
        {
            if (!vfx) SpawnVFX();
            else if (vfx)
            {
                go.transform.position = item.flyDirRef.transform.position;
                vfx.SetVector3("lineStart", item.flyDirRef.transform.position);
                vfx.SetVector3("lineEnd", currentRigidbody.transform.position);
            }
        }

        internal void CastRay()
        {

            RaycastHit hit;
            Transform parent;
            
            if (Physics.Raycast(item.flyDirRef.transform.position,item.flyDirRef.transform.forward, out hit, float.MaxValue, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                CustomDebug.Debug("Did hit.");
                CustomDebug.Debug(hit.collider.gameObject.transform.parent.name);

                parent = hit.collider.gameObject.transform.parent;
                parentLocal = parent.gameObject;
                

                if (parentLocal.gameObject.GetComponent<Item>() is Item item1)
                {
                    itemMesh = item1.gameObject.GetComponentInChildren<MeshFilter>().mesh;
                    currentRigidbody = item1.GetComponent<Rigidbody>();
                    canLift = true;
                    distance = Math.Abs(Vector3.Distance(parentLocal.transform.position, item.flyDirRef.position));

                }
                else if (parentLocal.gameObject.GetComponentInParent<Item>() is Item item2)
                {
                    itemMesh = item2.gameObject.GetComponentInChildren<MeshFilter>().mesh;
                    currentRigidbody = item2.GetComponent<Rigidbody>();
                    canLift = true;
                    distance = Math.Abs(Vector3.Distance(parentLocal.transform.position, item.flyDirRef.position));

                }
                else if (parentLocal.gameObject.GetComponentInChildren<Item>() is Item item3)
                {
                    itemMesh = item3.gameObject.GetComponentInChildren<MeshFilter>().mesh;
                    currentRigidbody = item3.GetComponent<Rigidbody>();
                    canLift = true;
                    distance = Math.Abs(Vector3.Distance(parentLocal.transform.position, item.flyDirRef.position));

                }
                else if (parentLocal.GetComponentInParent<Creature>() is Creature creature1) {

                    currentCreature = creature1;
                    if(currentCreature.ragdoll.state != Ragdoll.State.Frozen) currentCreature.ragdoll.SetState(Ragdoll.State.Destabilized);
                    
                    currentRigidbody = parentLocal.GetComponent<Rigidbody>();
                    canLift = true;
                    distance = Math.Abs(Vector3.Distance(parentLocal.transform.position, item.flyDirRef.position));

                }
                else if (parentLocal.GetComponent<Creature>() is Creature creature3) {

                    currentCreature = creature3;
                    if(currentCreature.ragdoll.state != Ragdoll.State.Frozen) currentCreature.ragdoll.SetState(Ragdoll.State.Destabilized);
                    currentRigidbody = parentLocal.GetComponent<Rigidbody>();
                    canLift = true;
                    distance = Math.Abs(Vector3.Distance(parentLocal.transform.position, item.flyDirRef.position));

                }
                else if (parentLocal.GetComponentInChildren<Creature>() is Creature creature2)
                {
                    currentCreature = creature2;
                    if(currentCreature.ragdoll.state != Ragdoll.State.Frozen) currentCreature.ragdoll.SetState(Ragdoll.State.Destabilized);
                    currentRigidbody = parentLocal.GetComponent<Rigidbody>();
                    canLift = true;
                    distance = Math.Abs(Vector3.Distance(parentLocal.transform.position, item.flyDirRef.position));
                }
            }
        }

        void Update()
        {
            direction = item.flyDirRef.forward;

                if (canLift == true)
                {
                    UpdateVFX();
                    currentRigidbody.velocity = ((item.flyDirRef.position + (direction * distance)) - currentRigidbody.position) * (3f);

                }

        }
    }

    public class WingardiumLeviosaHandler : Spell
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
