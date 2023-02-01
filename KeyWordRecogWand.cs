using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;
using UnityEngine.Windows.Speech;

namespace WandSpellss
{
    class KeyWordRecogWand : MonoBehaviour
    {
        internal bool hasRecognizedWord;
        private string[] m_keywords = new string[]{"Stewpify","Expelliarmus","Ahvahduhkuhdahvra","PetrificusTotalus","Levicorpus", "Liberacorpus","Ackio","Protego","Lumos","Engorgio","Evanesco","Geminio","Sectumsempra","Nox","Ascendio", "Vincere mortem", "Morsmordre", "Wingardium leviosa", "Reducio","Arresto Momentum", "Tarantallegra", "Pronunciation", "Deletrius", "Expecto Patronum"};
        Dictionary<string, Type> spellDict = new Dictionary<string, Type>();
        private KeywordRecognizer m_recognizer;
        internal bool isEnabled;

        internal string knownCurrent;
        private bool isListening;
        private Coroutine attentionSpan;


        public void Start() {

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


            hasRecognizedWord = false;
            m_recognizer = new KeywordRecognizer(m_keywords);
            m_recognizer.OnPhraseRecognized += M_recognizer_OnPhraseRecognized;
        }

        void Update() {

            if (!isEnabled)
            {

                m_recognizer.Stop();


            }

            else {

                if (!m_recognizer.IsRunning) {

                    m_recognizer.Start();
                
                }
            }
        }

        private void M_recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            StringBuilder builder = new StringBuilder();
            hasRecognizedWord = true;
            knownCurrent = args.text;

            if (spellDict.ContainsKey(knownCurrent)) {

                this.GetComponentInParent<SpellEntry>().TypeSelection(spellDict[knownCurrent], knownCurrent);

            } 


        }
    }
}


