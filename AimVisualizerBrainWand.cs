using System.Reflection;

namespace WandSpellss{
    
using ThunderRoad;
using UnityEngine;
    
    public class AimVisualizerBrainWand : MonoBehaviour
    {
        private Creature _creature;
        private BrainModuleFirearm _moduleFirearm;
        private LineRenderer _lineRendererMain;
        private LineRenderer _lineRendererOffhand;
        private static FieldInfo _lastNode = typeof (BrainModuleFirearm).GetField("lastNode", BindingFlags.Instance | BindingFlags.NonPublic);

        private void Awake()
        {
            this._creature = this.GetComponent<Creature>();
            this._moduleFirearm = this._creature.brain.instance.GetModule<BrainModuleFirearm>();
        }

        private void Update()
        {
            bool flag = false;
            if (this._creature.brain.currentTarget != (Object) null)
                flag = this._creature.brain.CanSight(this._creature.brain.currentTarget.ragdoll.rootPart.transform.position, 0.0f, 0.0f, 100f);
            if ((Object) this._moduleFirearm.fireableMain != (Object) null)
            {
                if (flag && (Object) this._creature.handRight.grabbedHandle != (Object) null)
                {
                    this._moduleFirearm.mainHandAnchor.localRotation = Quaternion.Euler(0.0f, 90f, 75f);
                    this._creature.ragdoll.ik.SetHandAnchor(Side.Right, this._moduleFirearm.mainHandAnchor);
                }
                else
                    this._creature.ragdoll.ik.SetHandAnchor(Side.Right, (Transform) null);
            }
            if (!((Object) this._moduleFirearm.fireableOffhand != (Object) null))
                return;
            if (flag && (Object) this._creature.handLeft.grabbedHandle != (Object) null)
            {
                this._moduleFirearm.offhandAnchor.localRotation = Quaternion.Euler(180f, 90f, 90f);
                this._creature.ragdoll.ik.SetHandAnchor(Side.Left, this._moduleFirearm.offhandAnchor);
            }
            else
                this._creature.ragdoll.ik.SetHandAnchor(Side.Left, (Transform) null);
        }
    }
}