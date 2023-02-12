using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Speech.Recognition;
using ThunderRoad;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;



namespace WandSpellss
{
    public class SpellListData : CustomData
    {

        public List<JSONSpell> spellList;
    }

    public class JSONSpell
    {

        public string name;
        public string classType;
        public string nameSpace;

    }
}
