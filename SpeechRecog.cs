using System;
using System.Speech.Recognition;

namespace WandSpellss
{
    class SpeechRecog
    {
        static void Main(string[] args)
        {

            // Create a SpeechRecognitionEngine object for the default recognizer in the en-US locale.
            using (
            SpeechRecognitionEngine recognizer =
              new SpeechRecognitionEngine(
                new System.Globalization.CultureInfo("en-US")))
            {

                // Create a grammar for finding services in different cities.
                Choices spells = new Choices(new string[] { "Expelliarmus", "AvadaKedavra", "Stupefy"});

                GrammarBuilder findServices = new GrammarBuilder("Find");
                

                // Create a Grammar object from the GrammarBuilder and load it to the recognizer.
                Grammar servicesGrammar = new Grammar(spells);
                recognizer.LoadGrammarAsync(servicesGrammar);

                // Add a handler for the speech recognized event.
                recognizer.SpeechRecognized +=
                  new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);

                // Configure the input to the speech recognizer.
                recognizer.SetInputToDefaultAudioDevice();

                // Start asynchronous, continuous speech recognition.
                recognizer.RecognizeAsync(RecognizeMode.Multiple);

                // Keep the console window open.
                while (true)
                {
                    Console.ReadLine();
                }
            }
        }

        // Handle the SpeechRecognized event.
        static void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            switch (e.Result.Text){

                case 
            }
        }
    }
}