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

namespace WandSpellss
{
    class Loader : CustomData
    {
        public static Loader local;


        public List<Creature> levicorpusedCreatures = new List<Creature>();
        public List<GameObject> floaters = new List<GameObject>();
        public Item currentTipper;
        public Item currentWand;
        public Material evanescoDissolveMat;
        public GameObject highlighter;
        public Material selectorMat;
        public GameObject incendioEffect;
        public GameObject bubbleHeadEffect;
        public List<GameObject> sparksEffect = new List<GameObject>();
        public GameObject stupefySparks;
        public GameObject expelliarmusSparks;
        public GameObject petrificusSparks;
        public GameObject avadaSparks;
        public GameObject levicorpusSparks;
        public GameObject tarantallegraSparks;
        public List<Item> currentlyHeldWands = new List<Item>();
        public List<Type> spellsOnPlayer = new List<Type>();
        public List<Type> finiteSpells = new List<Type>();

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

        public override void OnCatalogRefresh()
        {
            //Only want one instance of the loader running
            if (local != null) return;
            local = this;
            // do whatever you want to setup, listen to events etc
            local = this;
            couroutineManager = coroutineManagerGO.AddComponent<Coroutines>();
            Catalog.LoadAssetAsync<Material>("apoz123Wand.SpellEffect.Evanesco.Mat", callback => { evanescoDissolveMat = callback; }, "Evanesco");
            Catalog.LoadAssetAsync<Material>("apoz123Wand.Selector.Mat", callback => { selectorMat = callback; }, "Selector");
            Catalog.LoadAssetAsync<GameObject>("apoz123Wand.Incendio.SpellEffect", callback => { incendioEffect = callback; }, "Incendio");
            Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SpellEffect.BubbleHead", callback => { bubbleHeadEffect = callback; }, "BubbleHead");
            Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SpellEffect.Sparks.Stupefy", callback => { stupefySparks = callback; }, "StupefySparks");
            Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SpellEffect.Sparks.Expelliarmus", callback => { expelliarmusSparks = callback; }, "ExpelliarmusSparks");
            Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SpellEffect.Sparks.AvadaKedavra", callback => { avadaSparks = callback; }, "AvadaKedavraSparks");
            Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SpellEffect.Sparks.PetrificusTotalus", callback => { petrificusSparks = callback; }, "PetrificusTotalusSparks");
            Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SpellEffect.Sparks.Levicorpus", callback => { levicorpusSparks = callback; }, "LevicorpusSparks");
            Catalog.LoadAssetAsync<GameObject>("apoz123Wand.SpellEffect.Sparks.Tarantallegra", callback => { tarantallegraSparks = callback; }, "TarantallegraSparks");

            Choices spells = new Choices();
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



            recognizer = new SpeechRecognitionEngine();
            // Create a SpeechRecognitionEngine object for the default recognizer in the en-US locale.

            // Create a grammar for finding services in different cities.


            Grammar servicesGrammar = new Grammar(new GrammarBuilder(spells));


            recognizer.RequestRecognizerUpdate();
            // Create a Grammar object from the GrammarBuilder and load it to the recognizer.

            recognizer.LoadGrammarAsync(servicesGrammar);

            // Configure the input to the speech recognizer.
            recognizer.SetInputToDefaultAudioDevice();

            // Start asynchronous, continuous speech recognition.
            recognizer.RecognizeAsync(RecognizeMode.Multiple);

            // Add a handler for the speech recognized event.
            recognizer.SpeechRecognized += Recognizer_SpeechRecognized;

            Application.quitting += () => Process.GetCurrentProcess().Kill();

            CustomDebug.debugOn = true;

        }
       



        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            CustomDebug.Debug("Confidence: " + e.Result.Confidence);
            if (e.Result.Text != null && e.Result.Confidence > 0.93f)
            {
                CustomDebug.Debug(e.Result.Text);
                if (spellDict.ContainsKey(e.Result.Text) && currentlyHeldWands.Count > 0)
                {
                    foreach (Item wand in Loader.local.currentlyHeldWands)
                    {
                        wand.gameObject.GetComponent<SpellEntry>().TypeSelection(spellDict[e.Result.Text], e.Result.Text);
                    }
                }

                else if (currentlyHeldWands.Count <= 0 && e.Result.Text == "Accio Wand")
                {

                    try
                    {
                        paramItem = Item.allActive.Where(item => item.GetComponent<SpellEntry>() != null).OrderBy(item => Vector3.Distance(item.transform.position, Player.currentCreature.handRight.transform.position)).First();

                    }
                    catch (InvalidOperationException) { }

                    if (paramItem) couroutineManager.StartAccio(paramItem, Player.currentCreature.handRight);
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
