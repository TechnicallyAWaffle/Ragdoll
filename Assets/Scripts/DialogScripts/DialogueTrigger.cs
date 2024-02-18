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

    private void Awake()
    {
        visualCue.SetActive(false);
        playerInRange = false;
    }

    //Game Loop
    void Update()
    {
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Debug.Log(inkJSON.text);
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    //Checks if Ragdoll enters NPC's range to talk
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //checks tag of incoming object (aka player)
        if (collider.gameObject.tag == "Ragdoll")
        {
            playerInRange = true;
        }
    }

    //Checks if Ragdoll exits NPC's range to talk
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Ragdoll")
        {
            playerInRange = false;
        }
    }
}
