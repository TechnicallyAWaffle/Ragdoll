//DialogueTrigger --> NPC mechanics
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;

    //Init variables & states
    private void Awake()
    {
        visualCue.SetActive(false);
        playerInRange = false;
    }

    //Game Loop
    void Update()
    {
        //Check to see if player wants to talk to NPC
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Grab ink dialogue once 'E' is pressed
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    //Checks if RAGDOLL enters NPC's range to talk
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //checks tag of incoming object (aka player)
        if (collider.gameObject.tag == "Ragdoll")
        {
            playerInRange = true;
        }
    }

    //Checks if RAGDOLL exits NPC's range to talk
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Ragdoll")
        {
            playerInRange = false;
        }
    }
}
