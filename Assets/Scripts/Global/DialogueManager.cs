using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.InputSystem;
using System.Data;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanelPrefab;
    private GameObject currentDialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    private Story currentStory;

    [SerializeField] private TextAsset pearceDialogue;
    [SerializeField] private TextAsset embrodyleDialogue;
    [SerializeField] private TextAsset msPrettyDialogue;

    private static DialogueManager instance;
    public bool dialogueIsPlaying { get; private set; }

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
        if (!dialogueIsPlaying)
        {
            return;
        }
    }

    public void EnterDialogueMode(NPC npc)
    {
        Vector3 spawnOffset = new Vector3(1, 1, 0); // Adjust this to set the relative position.
        currentDialoguePanel = Instantiate(dialoguePanelPrefab, npc.transform.position + spawnOffset, Quaternion.identity, npc.transform);

        switch (npc.npcName)
        {
            case "Pearce":
                currentStory = new Story(pearceDialogue.text);
                currentStory.variablesState["speakers"] = "SOLO";
                break;
            case "Embrodyle":
                currentStory = new Story(embrodyleDialogue.text);
                currentStory.variablesState["speakers"] = "SOLO";
                break;
            case "Pretty":
                currentStory = new Story(msPrettyDialogue.text);
                currentStory.variablesState["speakers"] = "SOLO";
                break;
            case "Janus":
                //currentStory = new Story(janusDialogue.text);
                //currentStory.variablesState["speakers"] = speakers;
                break;
            case "":
                Debug.Log("test2ran");
                currentStory = new Story(msPrettyDialogue.text);
                currentStory.variablesState["speakers"] = "SOLO";
                break;
        }
        dialogueIsPlaying = true;
        // dialoguePanel.SetActive(true);

        currentStory.BindExternalFunction("SetSpeaker", (string character) =>
        {
            Debug.Log(character);
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
        // dialoguePanel.SetActive(false);
        // dialogueText.text = "";
        currentStory.UnbindExternalFunction("SetSpeaker");
        currentStory = null;
    }
    public void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            //Grab the next line of dialogue
            currentDialoguePanel.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = currentStory.Continue();
        }
        else
        {
            ExitDialogueMode();
        }
    }

}
