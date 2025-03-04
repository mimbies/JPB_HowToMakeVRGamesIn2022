using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;   //Local eingestellte Sprache

public class VoiceRecognition : MonoBehaviour
{

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    DialogueManager dialogueManager;

    public bool nearbyEmily = false;
    public bool juergenHasBeenGreeted = false;
    public bool sendAmbulance = false;
    public bool emilyStandsUp = false;
    public bool emilyExtendsArms = false;

    public bool emilyOneLeg = false;


    public int fastMethodSteps = 0;

    private void Start()
    {
        actions.Add("Hallo Jürgen", hallojuergen);
        actions.Add("Alles okay", emily_allesokay);     // Ist alles Okay?
        actions.Add("dein Name", emily_deinname);       // Wie ist dein Name? Oder: Wie heißt du? wollen wir mehr optionen haben?
        actions.Add("ihr Name", emily_deinname);
        actions.Add("wie heißt du", emily_deinname);
        actions.Add("aufstehen", emily_aufstehen);      //Kannst du aufstehen?
        actions.Add("Kannst du aufstehen", emily_aufstehen);
        actions.Add("Kannst du lächeln", emily_aufstehen);
        actions.Add("Kannst du aufstehen und lächeln", emily_aufstehen);
        actions.Add("Arme ausstrecken", emily_armeausstrecken); //Kannst du deine Arme ausstrecken?
        actions.Add("Kannst du deine Arme ausstrecken", emily_armeausstrecken); //Kannst du deine Arme ausstrecken?
        actions.Add("Arme", emily_armeausstrecken); //Kannst du deine Arme ausstrecken?
        actions.Add("linkes Bein", emily_linkesbein);   // Kannst du dich auf dein linkes Bein stellen?
        actions.Add("Kannst du dich auf dein linkes Bein stellen", emily_linkesbein);
        actions.Add("Kannst du dich auf ein Bein stellen", emily_linkesbein);
        actions.Add("sonniger Tag", emily_nachsprechen);
        actions.Add("Heute ist Mittwoch, es soll ein sonniger Tag werden", emily_nachsprechen);
        actions.Add("Bushaltestelle", krankenwagen_wo); //ungenau?
        actions.Add("Park", krankenwagen_wo);
        actions.Add("eine Frau", krankenwagen_wer);
        actions.Add("emily", krankenwagen_wer); // Eine Frau mit dem Namen Emily?
        actions.Add("Schlaganfall", krankenwagen_was); //Sie hat vermutlich einen Schlaganfall

        actions.Add("gelehmt", krankenwagen_symptomGelaehmt);
        actions.Add("balance", krankenwagen_symptomeBalance);
        actions.Add("ballons", krankenwagen_symptomeBalance);
        actions.Add("Sprachschwierigkeiten", krankenwagen_symptomeSprachschwierigkeiten); //Anders?
        actions.Add("Probleme beim Sprechen", krankenwagen_symptomeSprachschwierigkeiten);
        actions.Add("Ja", krankenwagen_bestaetigung);
        actions.Add("verstanden", krankenwagen_bestaetigung);
        actions.Add("Habe ich", krankenwagen_bestaetigung);
        actions.Add("Ich habe verstanden", krankenwagen_bestaetigung);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += speechDistributer;
        keywordRecognizer.Start(); //nur Starten und stoppen bei Gebrauch um Laufzeit zu verbessern

        dialogueManager = FindObjectOfType<DialogueManager>();


    }

    private void speechDistributer(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }


    private void hello()
    {
        if (testForDialogueManager())
        {
            dialogueManager.QueueAndPlayDialogueById(0);    //L�sst vorherige Audios und Untertitel auslaufen
        }

    }

    private void coding()
    {
        if (testForDialogueManager())
        {
            dialogueManager.QueueAndPlayDialogueById(1);    //L�sst vorherige Audios und Untertitel auslaufen
        }
    }

    private void interrupt()
    {
        if (testForDialogueManager())
        {
            dialogueManager.InterruptAndPlayDialogueById(2);    //Unterbricht aktuelle und darauffolgende Audios + Untertitel und spielt nur diesen aus
        }
    }

    private void hallojuergen()
    {
        juergenHasBeenGreeted = true;       //sets transition variable to true
                                            //sitting animation transitions to waving animation
        Debug.Log("Jürgen heard you");

    }

    public void changeNearbyEmily()
    {

        nearbyEmily = true;
    }

    private void emily_allesokay()
    {
        if (nearbyEmily)
        {
            if (fastMethodSteps == 0)
            {
                fastMethodSteps = 1;

                Debug.Log("asked emily if she's okay");

                dialogueManager.QueueAndPlayDialogueById(8); //Emily says shes okay

                dialogueManager.QueueAndPlayDialogueById(9); //Narrator prompts you to ask for her name
            }
        }
    }

    private void emily_deinname()
    {
        if (fastMethodSteps == 1)
        {

            Debug.Log("asked for emilys name");

            fastMethodSteps = 2;

            dialogueManager.QueueAndPlayDialogueById(10); //Emily replies with her name

            dialogueManager.QueueAndPlayDialogueById(11); // Narrator prompts you to tell emily to stand up
        }
    }
    private void emily_aufstehen()
    {
        if (fastMethodSteps == 2)
        {

            Debug.Log("asked emily to stand up (and smile)");
            //she smiles a (only one side of her face moves up)

            emilyStandsUp = true;

            fastMethodSteps = 3;



            dialogueManager.QueueAndPlayDialogueById(12); // Narrator explains that Emily seems unwell

            dialogueManager.QueueAndPlayDialogueById(13); //N prompts you to ask emily to extend her arms
        }




    }

    private void emily_armeausstrecken()
    {
        if (fastMethodSteps == 3)
        {

            Debug.Log("asked emily to extend her arms");
            //emily standing animation transitions to her raising her arms, one more than the other

            emilyExtendsArms = true;

            fastMethodSteps = 4;

            dialogueManager.QueueAndPlayDialogueById(14);

        }
    }

    private void emily_linkesbein()
    {
        if (fastMethodSteps == 4)
        {

            Debug.Log("asked emily to stand on her left leg and bend the other one");
            //emily is struggling to follow your instructions, not able to raise right leg, losing balance
            //how to animate?

            emilyOneLeg = true;


            fastMethodSteps = 5;

            dialogueManager.QueueAndPlayDialogueById(15); //N prompts you to ask emily to speak after you
        }


    }

    private void emily_nachsprechen()
    {


        if (fastMethodSteps == 5)
        {

            Debug.Log("asked emily to speak after you");

            fastMethodSteps = 6;

            dialogueManager.QueueAndPlayDialogueById(16); //Emily speaks after you, slow and broken sentences

            dialogueManager.QueueAndPlayDialogueById(17); //N explains that EMily might suffer from a stroke.
                                                          // Prompts you to pick up her phone

            dialogueManager.QueueAndPlayDialogueById(18); // prompt to call 911 - implement somewhere with phone maybe..

        }


    }

    private void krankenwagen_wo()
    {
        //Before: 112 has been called, player has been asked where they are

        if (fastMethodSteps == 6)
        {

            Debug.Log("told paramedics location");

            fastMethodSteps = 7;

            dialogueManager.QueueAndPlayDialogueById(20); //Paramedics ask whos hurt


        }



    }

    private void krankenwagen_wer()
    {

        if (fastMethodSteps == 7)
        {

            Debug.Log("told paramedics whos hurt");

            fastMethodSteps = 8;

            dialogueManager.QueueAndPlayDialogueById(21); //paramedics ask what happened
        }



    }

    private void krankenwagen_was()
    {

        if (fastMethodSteps == 8)
        {

            Debug.Log("told paramedics what happened");

            fastMethodSteps = 9;

            dialogueManager.QueueAndPlayDialogueById(23);

        }



    }

    private void krankenwagen_symptomeBalance()
    {

        if (fastMethodSteps > 8 && fastMethodSteps < 12) //9
        {

            Debug.Log("told paramedics about balance problems");

            fastMethodSteps += 1; // 10 or 11 or 12

            if (fastMethodSteps == 12)
            {
                dialogueManager.QueueAndPlayDialogueById(22); //paramedics ask you confirm you understood
            }

        }



    }

    private void krankenwagen_symptomGelaehmt()
    {

        if (fastMethodSteps > 8 && fastMethodSteps < 12)
        { //10

            Debug.Log("told paramedics about partial paralysis");

            fastMethodSteps += 1; // 10 or 11 or 12

            if (fastMethodSteps == 12)
            {
                dialogueManager.QueueAndPlayDialogueById(22); //paramedics ask you confirm you understood
            }

        }



    }

    private void krankenwagen_symptomeSprachschwierigkeiten()
    {

        if (fastMethodSteps > 8 && fastMethodSteps < 12)
        { //11

            Debug.Log("told paramedics about pproblems with speaking");
            ;

            fastMethodSteps += 1; // 10 or 11 or 12

            if (fastMethodSteps == 12)
            {
                dialogueManager.QueueAndPlayDialogueById(22); //paramedics ask you confirm you understood
            }
        }



    }



    private void krankenwagen_bestaetigung()
    {
        if (fastMethodSteps == 12)
        {

            Debug.Log("confirmed that you understood paramedics");

            sendAmbulance = true;

            dialogueManager.QueueAndPlayDialogueById(24);
            dialogueManager.QueueAndPlayDialogueById(25);

        }

    }


    //Still missing: Steps after calling the ambulance

    private bool testForDialogueManager()
    {
        if (dialogueManager != null)
        {
            return true;
        }
        else
        {
            Debug.LogWarning("Kein DialogueManager in der Szene gefunden!");
            return false;
        }
    }

}
