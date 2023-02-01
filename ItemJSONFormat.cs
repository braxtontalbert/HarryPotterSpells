using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace WandSpellss
{
    class ItemJSONFormat : CatalogData
    {
        public string localizationId;
        public string displayName;
        public string description;
        public string author;
        public float price;
        public bool purchasable;
        public int tier;
        public string weight;
        public string size;
        public int levelRequired;
        public string category;
        public string iconEffectId;
        public EffectData iconEffectData;
        public string prefabAddress;
        public string iconAddress;
        public IResourceLocation iconLocation;
        public string closeUpIconAddress;
        public IResourceLocation closeUpIconLocation;
        public Texture icon;
        public Texture closeUpIcon;
        public int pooledCount;
        public int androidPooledCount;
        public ItemData.Type type;
        public List<Item.IconMarker> iconDamagerMarkers = new List<Item.IconMarker>();
        public bool canBeStoredInPlayerInventory;
        public bool limitMaxStorableQuantity;
        public int maxStorableQuantity;
        public bool isStackable;
        public int maxStackQuantity;
        public GameData.AudioContainerAddressAndVolume inventoryHoverSounds;
        public GameData.AudioContainerAddressAndVolume inventorySelectSounds;
        public GameData.AudioContainerAddressAndVolume inventoryStoreSounds;
        public string slot;
        public string snapAudioContainerAddress;
        public float snapAudioVolume_dB;
        public IResourceLocation snapAudioContainerLocation;
        public string unsnapAudioContainerAddress;
        public float unsnapAudioVolume_dB;
        public IResourceLocation unsnapAudioContainerLocation;
        public bool overrideMassAndDrag;
        public float mass = 1f;
        public float drag = 1f;
        public float angularDrag = 1f;
        public float manaRegenMultiplier = 1f;
        public float spellChargeSpeedPlayerMultiplier = 1f;
        public float spellChargeSpeedNPCMultiplier = 1f;
        public int collisionMaxOverride;
        public bool collisionEnterOnly;
        public bool collisionNoMinVelocityCheck;
        public LayerName forceLayer;
        public AnimationCurve waterHandSpringMultiplierCurve = AnimationCurve.EaseInOut(0.0f, 0.3f, 1f, 0.15f);
        public AnimationCurve waterDragMultiplierCurve = AnimationCurve.EaseInOut(0.0f, 1f, 1f, 10f);
        public float waterSampleMinRadius = 0.2f;
        public bool flyFromThrow;
        public float flyRotationSpeed = 2f;
        public float flyThrowAngle;
        public float telekinesisSafeDistance = 1f;
        public bool telekinesisSpinEnabled = true;
        public float telekinesisThrowRatio = 1f;
        public bool telekinesisAutoGrabAnyHandle;
        public bool grippable;







    }
}
