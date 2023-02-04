using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using UnityEngine;
using ThunderRoad;

namespace WandSpellss
{
    class SpeechRecogWands : MonoBehaviour
    {

        GrammarBuilder findServices;
        SpeechRecognitionEngine recognizer;
        public string knownCurrent;
        Dictionary<string, Type> spellDict = new Dictionary<string, System.Type>();
        SpellEntry itemStuff;

        public void Start()
        {
            Debug.Log("Started Speech Recognizer");

            spellDict.Add("Stewpify", typeof(Stupefy));
            spellDict.Add("Wingardium leviosa", typeof(WingardiumLeviosa));
            spellDict.Add("Expelliarmus", typeof(Expelliarmus));
            spellDict.Add("Ahvahduhkuhdahvra", typeof(AvadaKedavra));
            spellDict.Add("PetrificusTotalus", typeof(PetrificusTotalus));
            spellDict.Add("Protego", typeof(Protego));
            spellDict.Add("Arresto Momentum", typeof(ArrestoMomentum));
            spellDict.Add("Ascendio", typeof(Ascendio));
            spellDict.Add("Ackio", typeof(Accio));
            spellDict.Add("Engorgio", typeof(Engorgio));
            spellDict.Add("Evanesco", typeof(Evanesco));
            spellDict.Add("Reducio", typeof(Reducio));
            spellDict.Add("Levicorpus", typeof(Levicorpus));
            spellDict.Add("Morsmordre", typeof(Morsmordre));
            spellDict.Add("Geminio", typeof(Geminio));
            spellDict.Add("Liberacorpus", typeof(Liberacorpus));
            spellDict.Add("Lumos", typeof(Lumos));
            spellDict.Add("Nox", typeof(Nox));
            spellDict.Add("Tarantallegra", typeof(Tarantallegra));
            spellDict.Add("Sectumsempra", typeof(Stupefy));

            recognizer = new SpeechRecognitionEngine();
            // Create a SpeechRecognitionEngine object for the default recognizer in the en-US locale.

            // Create a grammar for finding services in different cities.
            Choices spells = new Choices(new string[] { "Stewpify", "Expelliarmus", "Ahvahduhkuhdahvra", "PetrificusTotalus", "Levicorpus", "Liberacorpus", "Ackio", "Protego", "Lumos", "Engorgio", "Evanesco", "Geminio", "Sectumsempra", "Nox", "Ascendio", "Vincere mortem", "Morsmordre", "Wingardium leviosa", "Reducio", "Arresto Momentum", "Tarantallegra", "Pronunciation", "Deletrius", "Expecto Patronum" });

            
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
        }

        

        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text != null)
            {
                Debug.Log(e.Result.Text);
                if (spellDict.ContainsKey(e.Result.Text))
                {
                    foreach (Item wand in Loader.local.currentlyHeldWands)
                    {
                        wand.gameObject.GetComponent<SpellEntry>().TypeSelection(spellDict[e.Result.Text], e.Result.Text);
                    }
                }
            }
        }


    }
}
