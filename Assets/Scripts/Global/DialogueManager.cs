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
    private GameObject currentDialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    private Story currentStory; //keep track of current Inkfile to display

    [SerializeField] private TextAsset pearceDialogue;
    [SerializeField] private TextAsset embrodyleDialogue;
    [SerializeField] private TextAsset msPrettyDialogue;
    //[SerializeField] private TextAsset janusDialogue;

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
    }

    private void Update()
    {
        //Dont display anything if false
        if (!dialogueIsPlaying)
        {
            return;
        }
    }


    //Grabs dialogue from inkJson file & displays it to screen (used in DialogueTrigger.cs)
    public void EnterDialogueMode(string characterName, string secondaryCharacterName, GameObject dialogueTemplate)
    {
        dialoguePanel = dialogueTemplate;
        dialogueText = dialogueTemplate.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        string speakers = characterName;
        if (secondaryCharacterName != null)
            speakers = characterName + secondaryCharacterName;
        switch (characterName)
        {
            case "Pearce":
                currentStory = new Story(pearceDialogue.text);
                currentStory.variablesState["speakers"] = "SOLO";
                break;
            case "Embrodyle":
                currentStory = new Story(embrodyleDialogue.text);
                currentStory.variablesState["speakers"] = "SOLO";
                break;
            case "Ms. Pretty":
                currentStory = new Story(msPrettyDialogue.text);
                currentStory.variablesState["speakers"] = "SOLO";
                break;
            case "Janus":
                //currentStory = new Story(janusDialogue.text);
                //currentStory.variablesState["speakers"] = speakers;
                break;
        }
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        currentStory.BindExternalFunction("SetSpeaker", (string character) =>
        {
            Debug.Log(character);
            //currentStory = new Story(inkJSON.text);  use character parameter here?
        });

        ContinueStory();
    }

    public void SetSpeaker(string speakerName)
    { 
        
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
    public void ContinueStory()
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
