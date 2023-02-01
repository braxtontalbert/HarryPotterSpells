using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

namespace WandSpellss
{
    class DictationRecognizer : MonoBehaviour
    {

        private Text m_Hypotheses;

        private Text m_Recognitions;

        private DictationRecognizer m_DictationRecognizer;
        private Action<object> DictationComplete;
        private Action<object, object> DictationResult;
        private Action<object> DictationHypothesis;
        private Action<object, object> DictationError;

        void Start()
        {
            m_DictationRecognizer = new DictationRecognizer();

            m_DictationRecognizer.DictationResult += (text, confidence) =>
            {
                Debug.LogFormat("Dictation result: {0}", text);
                m_Recognitions.text += text + "\n";
            };

            m_DictationRecognizer.DictationHypothesis += (text) =>
            {
                Debug.LogFormat("Dictation hypothesis: {0}", text);
                m_Hypotheses.text += text;
            };

            m_DictationRecognizer.DictationComplete += (completionCause) =>
            {
                if ((DictationCompletionCause)completionCause != DictationCompletionCause.Complete)
                    Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", completionCause);
            };

            m_DictationRecognizer.DictationError += (error, hresult) =>
            {
                Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
            };

            m_DictationRecognizer.Start();
        }


        void Update() {



            Debug.Log(m_Hypotheses);
        
        }
    }
}
