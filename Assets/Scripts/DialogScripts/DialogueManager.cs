//using Ink.Parsed;
//DialogueManager --> Ink file(actual dialogue) mechanics
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;   //allows for use of "Story" type in inkfiles
using UnityEngine.InputSystem;
using System.Data;

public class DialogueManager : MonoBehaviour
{
    //Singleton Class - can only have 1 object at a time
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    private Story currentStory; //keep track of current Inkfile to display

    private static DialogueManager instance;
    public bool dialogueIsPlaying { get; private set; } //sets outside scripts to read only

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        //Dont display anything if false
        if (!dialogueIsPlaying)
        {
            return;
        }
        
        //continue to next line of dialogue everytime [E] is pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            ContinueStory();
        }
    }


    //Grabs dialogue from inkJson file & displays it to screen (used in DialogueTrigger.cs)
    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        currentStory.BindExternalFunction("SetSpeaker", (string character) =>
        {
            Debug.Log(character);
            //currentStory = new Story(inkJSON.text);  use character parameter here?
        });

        ContinueStory();
    }

    //Exits the dialogue if we've gone through the whole InkJSON file
    private void ExitDialogueMode()
    {
        Debug.Log("Exit Dialogue Function Called");
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        currentStory.UnbindExternalFunction("SetSpeaker");
        currentStory = null;
    }

    //Checks if you can continue dialogue
    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            //Grab the next line of dialogue
            dialogueText.text = currentStory.Continue();
        }
        else
        {
            ExitDialogueMode();
        }
    }

}


/* bubble showing --> prefab
 * end convo fix (exitdialoguemode not being called?)
 */