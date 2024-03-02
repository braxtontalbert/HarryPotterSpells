using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine.VFX;
using UnityEngine;
using System.Diagnostics;
using JetBrains.Annotations;
//using FirearmAIFix;
using Debug = UnityEngine.Debug;

namespace WandSpellss
{
    class Loader : CustomData
    {
        public static Loader local;


        public List<Creature> levicorpusedCreatures = new List<Creature>();
        public List<GameObject> floaters = new List<GameObject>();
        public List<Item> currentTippers = new List<Item>();
        public Item currentWand;
        public Material evanescoDissolveMat;
        public Material dissimuloDissolveMat;
        public GameObject highlighter;
        public Material selectorMat;
        public GameObject incendioEffect;
        public GameObject bubbleHeadEffect;
        public GameObject impedimentaEffect;
        public GameObject imperioShown;
        public List<GameObject> sparksEffect = new List<GameObject>();
        public GameObject stupefySparks;
        public GameObject expelliarmusSparks;
        public GameObject petrificusSparks;
        public GameObject avadaSparks;
        public GameObject levicorpusSparks;
        public GameObject tarantallegraSparks;
        public GameObject flipendoSparks;
        public GameObject leviosoSparks;
        public GameObject evertestatumSparks;
        public GameObject wingardiumLeviosaEffect;
        public GameObject imperioEffect;
        public GameObject depulsoEffect;
        public GameObject explosion;
        public GameObject avadaTest;
        public GameObject protegoNew;
        public GameObject crucioEffect;
        public List<Item> currentlyHeldWands = new List<Item>();
        public List<Type> spellsOnPlayer = new List<Type>();
        public List<Type> finiteSpells = new List<Type>();
        public Dictionary<Creature, float[]> creaturesFOV = new Dictionary<Creature,float[]>();
        public SDFGenerator sdfg;
        public ComputeShader compute;
        
        [ModOptionCategory("Spells", 1)]
        [ModOptionOrder(1)]
        [ModOption]
        [ModOptionUI]
        public static string spellList;
        
        
        //SOUNDFX
        public GameObject impedimentaSoundFX;

        //SPEECH RECOGNITION STUFF
        GrammarBuilder findServices;
        SpeechRecognitionEngine recognizer;
        public string knownCurrent;
        Dictionary<string, Type> spellDict = new Dictionary<string, System.Type>();
        SpellEntry itemStuff;
        public string fileLocation;
        public Coroutines couroutineManager;
        public SpellEntry nvt;
        GameObject coroutineManagerGO = new GameObject();
        Item paramItem;
        public bool dissimuloActive;
        public GameObject activeDisillusion;
        public List<Material[]> originalCreatureMaterial = new List<Material[]>();

        public long epochTimeStart;
        public long epochEndTime;
        
        //protego
        public bool protegoSpawned { get; set; }


        public List<String> itemNames = new List<string>();

        public override void OnCatalogRefresh()
        {
            if (local != null) return;
            local = this;
            AsyncSetup();
            
            CustomDebug.debugOn = true;
        }

        public Choices spells = new Choices();
        async void AsyncSetup() {

            await Task.Run(() =>
            {
                couroutineManager = coroutineManagerGO.AddComponent<Coroutines>();
                Catalog.LoadAssetAsync<Material>("apoz123Wand.SpellEffect.Evanesco.Mat", callback => { evanescoDissolveMat = callback; }, "Evanesco");
                Catalog.LoadAssetAsync<Material>("apoz123Wand.SpellEffect.Dissimulo.Mat", callback => { dissimuloDissolveMat = callback; }, "Dissimulo");
                //Catalog.LoadAssetAsync<Material>("apoz123Wand.Selector.Mat", callback => { selectorMat = callback; }, "Selector");
                //Catalog.LoadAssetAsync<GameObject>("apoz123Wand.Incendio.SpellEffect", callback => { incendioEffect = callback; }, "Incendio");
                Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SpellEffect.BubbleHead", callback => { bubbleHeadEffect = callback; Debug.Log(callback);}, "BubbleHead");
                Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SpellEffect.Sparks.Stupefy", callback => { stupefySparks = callback; Debug.Log(callback);}, "StupefySparks");
                Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SpellEffect.Sparks.Expelliarmus", callback => { expelliarmusSparks = callback; }, "ExpelliarmusSparks");
                Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SpellEffect.Sparks.AvadaKedavra", callback => { avadaSparks = callback; }, "AvadaKedavraSparks");
                Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SpellEffect.Sparks.PetrificusTotalus", callback => { petrificusSparks = callback; }, "PetrificusTotalusSparks");
                Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SpellEffect.Sparks.Levicorpus", callback => { levicorpusSparks = callback; }, "LevicorpusSparks");
                Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SpellEffect.Sparks.Tarantallegra", callback => { tarantallegraSparks = callback; }, "TarantallegraSparks");
                Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SpellEffect.Sparks.Flipendo", callback => { flipendoSparks = callback; }, "FlipendoSparks");
                Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SpellEffect.Sparks.Levioso", callback => { leviosoSparks = callback; }, "LeviosoSparks");
                Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SpellEffect.Impedimenta",callback => { impedimentaEffect = callback;}, "ImpedimentaEffect");
                Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SoundEffect.Impedimenta",callback => { impedimentaSoundFX = callback;}, "ImpedimentaSoundEffect");
                Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SpellEffect.Line",callback => { wingardiumLeviosaEffect = callback;}, "LineEffect");
                Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SpellEffect.ImperioHidden",callback => { imperioEffect = callback;}, "ImperioEffect");
                Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SpellEffect.ImperioShown",callback => { imperioShown = callback;}, "ImperioVisibleEffect");
                Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SpellEffect.Sparks.Evertestatum", callback => {evertestatumSparks = callback;}, "EvertestatumSparks");
                Catalog.LoadAssetAsync<GameObject>("apoz123.SpellEffect.Explosion", callback => { explosion = callback;}, "ExplosionVisualEffect");
                Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SpellEffect.Depulso",
                    callback => { depulsoEffect = callback; Debug.Log(callback);}, "DepulsoEffect");
                Catalog.LoadAssetAsync<GameObject>("apoz123.SpellEffect.ProtegoNew",
                    callback => { protegoNew = callback; }, "ProtegoNew");
                    
                Catalog.LoadAssetAsync<GameObject>("apoz123.SpellEffect.AvadaTest",
                    callback => { avadaTest = callback; }, "AvadaTest");
                sdfg = new SDFGenerator();
                Catalog.LoadAssetAsync<ComputeShader>("apoz123.GenerateSDF.Compute", callback => compute = callback,
                    "ComputeShader");
                Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SpellEffect.Crucio", callback => crucioEffect = callback,"CrucioEffect");
                dissimuloActive = false;
                protegoSpawned = false;
                EventManager.onItemEquip += OnItemEquip;
                List<JSONSpell> loadedSpells = Catalog.GetData<SpellListData>("CustomSpells").spellList;

                foreach (JSONSpell spell in loadedSpells)
                {
                    var spellType = Type.GetType("WandSpellss." + spell.classType + "");
                    CustomDebug.Debug("Spell type is: " + spellType.ToString());
                    spellDict.Add(spell.name, spellType);
                    spells.Add(spell.name);
                    CustomDebug.Debug("Choices count: " + spells.ToString());
                }
                
                
                spells.Add("Accio Wand");
                spells.Add("Accio Nimbus");
                List<ItemData> itemDatas = Catalog.GetDataList<ItemData>();
                parseItemWeapons(itemDatas);
                spells.Add("Accio " + "Weapon");
                recognizer = new SpeechRecognitionEngine();

                Grammar servicesGrammar = new Grammar(new GrammarBuilder(spells));
                recognizer.RequestRecognizerUpdate();
                recognizer.LoadGrammarAsync(servicesGrammar);
                recognizer.SetInputToDefaultAudioDevice();
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
                recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
                Application.quitting += () => Process.GetCurrentProcess().Kill();
                foreach (string micName in Microphone.devices)
                {
                    Debug.Log("Default Microphone is: " + micName);   
                }
            });
        }

        private void parseItemWeapons(List<ItemData> itemDatas)
        {
            string accioString = "Accio";
            foreach (ItemData data in itemDatas)
            {
                if (data.type == ItemData.Type.Weapon && data.type != null && data.displayName != null && data.category != null)
                {
                    string displayName = data.displayName.ToLower();
                    string categoryName = "";
                    if (data.category.EndsWith("s"))
                    {
                        categoryName = data.category.Remove(data.category.Length - 1).ToLower();
                    }
                    else categoryName = data.category.ToLower();

                    if (displayName.Contains(categoryName))
                    {
                        spells.Add(accioString + " " + categoryName.ToLower());
                    }
                    else
                    {
                        string[] allNames = displayName.Split(' ');
                        for (int i = 0; i < allNames.Length; i++)
                        {
                            spells.Add(accioString + " " + allNames[i].ToLower());
                        }
                        spells.Add(displayName);
                    }
                }
            }
        }

        private void OnItemEquip(Item item)
        {
            if (item.GetComponent<ItemVoiceModule>() != null)
            {
               currentlyHeldWands.Add(item);
            }
        }
        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            CustomDebug.Debug("Confidence: " + e.Result.Confidence);
            string result = e.Result.Text;
            float confidence = e.Result.Confidence;
            string accioString = "Accio";
            if (result != null && confidence > 0.93f)
            {
                CustomDebug.Debug(result);
                if (spellDict.ContainsKey(result) && currentlyHeldWands.Count > 0)
                {
                    foreach (Item wand in Loader.local.currentlyHeldWands)
                    {
                        wand.gameObject.GetComponent<SpellEntry>().TypeSelection(spellDict[result], result, wand);
                    }
                }

                else if (currentlyHeldWands.Count <= 0 && (result == accioString + " Wand" || result == accioString + " wand"))
                {
                    try
                    {
                        paramItem = Item.allActive.Where(item => item.GetComponent<SpellEntry>() != null && !Player.currentCreature.equipment.GetAllHolsteredItems().Contains(item)).OrderBy(item =>  Vector3.Distance(item.transform.position, Player.currentCreature.handRight.transform.position)).First();

                    }
                    catch (InvalidOperationException) { }

                    if (paramItem) couroutineManager.StartAccio(paramItem, Player.currentCreature.handRight);
                }/*
                else if (currentlyHeldWands.Count > 0 && e.Result.Text == "Accio Nimbus")
                {
                    Catalog.GetData<ItemData>("Nimbus2000BroomVersion").SpawnAsync(callback =>
                    {
                        Item item = currentlyHeldWands[0];
                        callback.transform.position = item.transform.forward * 30f;
                        callback.gameObject.AddComponent<AccioNimbus>().Setup(item.mainHandler.otherHand);
                    });
                }*/

                else if(result.Contains(accioString) && currentlyHeldWands.Count > 0 && result.Length > accioString.Length)
                {
                    foreach (Item wand in currentlyHeldWands)
                    {
                        Type spellType = Type.GetType("WandSpellss." + accioString + "");
                        wand.gameObject.GetComponent<SpellEntry>().TypeSelection(spellType, result, wand, result);
                    }
                }
            }
        }
        public void StartBubbleHeadDestroy(GameObject bubbleUpdate)
        {
            couroutineManager.StartCustomCoroutine(DestroyBubbleHead(bubbleUpdate));
        }

        public IEnumerator DestroyBubbleHead(GameObject bubbleUpdate)
        {

            bubbleUpdate.GetComponentInChildren<VisualEffect>().Stop();

            yield return new WaitForSeconds(3f);

            UnityEngine.GameObject.Destroy(bubbleUpdate);
        }

        public void DestroyLevicorpus()
        {
            foreach (Creature creature in levicorpusedCreatures)
            {
                UnityEngine.Object.Destroy(creature.footLeft.GetComponent<SpringJoint>());
                UnityEngine.Object.Destroy(creature.footRight.GetComponent<SpringJoint>());
            }
            foreach (GameObject go in floaters) UnityEngine.Object.Destroy(go);
        }
        
        
    }
}
