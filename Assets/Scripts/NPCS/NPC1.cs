using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public string[] dialogue;
    private int index;

    public GameObject contButton;
    public float wordSpeed;
    public bool playerIsClose;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose)
        {

            //If Panel is active, reset & deactivate it
            if (dialoguePanel.activeInHierarchy)
            {
                resetDialoguePanel();
            }
            //If Panel is inactive, activate Panel to sshow up on screen
            else
            {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }

        //If we finish loading in text to screen, enable continue button
        if (dialogueText.text == dialogue[index])
        {
            contButton.SetActive(true);
        }
    }

    public void resetDialoguePanel()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    //Add our text to the screen
    IEnumerator Typing()
    {
        dialogueText.text = "";
        foreach(char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    //Every button click on dialogue, it loads in next dialogue text
    public void NextLine()
    {
        contButton.SetActive(false);

        if(index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            resetDialoguePanel();
        }
    }

    //Check when player enters NPC's talking range
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ragdoll"))
        {
            playerIsClose = true;
        }
    }

    //Check when player exits NPC's talking range
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ragdoll"))
        {
            playerIsClose = false;
            resetDialoguePanel();
        }
    }
}
