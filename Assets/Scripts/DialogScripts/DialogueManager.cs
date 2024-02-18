//using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    //Singleton Class
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    private Story currentStory; //keep track of current Inkfile to display

    private static DialogueManager instance;
    public bool dialogueIsPlaying { get; private set; } //sets outside scripts to read only

    private void Awake()
    {
        if (instance == null)
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


    //Grabs dialogue from inkJson file & displays it to screen
    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    //Exits the dialogue 
    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    //Checks if you can continue dialogue
    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
        }
        else
        {
            ExitDialogueMode();
        }
    }
}
