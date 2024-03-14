using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private GameObject EToInteract;
    private GameObject thisEToInteract;

    public RagdollMain ragdollMain;
    
    public AudioManager audioManager;
    public HealthSystem ragdollHealth;

    private GameObject currentNPC;

    private void Start()
    {
        thisEToInteract = Instantiate(EToInteract);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "NPC")
        {
            GameObject npc = collision.gameObject;
            currentNPC = npc;
            thisEToInteract.SetActive(true);
            thisEToInteract.transform.position = new Vector3(npc.transform.position.x, -1, -1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (thisEToInteract)
            thisEToInteract.SetActive(false);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("yorb");
            thisEToInteract.SetActive(false);
            switch (currentNPC.name)
            {
                case "Ms. Pretty":
                    audioManager.StartCoroutine(audioManager.ChangeHubCharacterTrack(AudioManager.HubTracks.MSPRETTY));
                    ragdollHealth.heal(1);
                    break;
                case "Pearce":
                    //Call GUI event
                    audioManager.StartCoroutine(audioManager.ChangeHubCharacterTrack(AudioManager.HubTracks.PEARCE));
                    break;
                case "Embrodyle":
                    //Store dropped items in lost items list in SceneLoader. Move one random one into inventory when this is called.
                    //Go on date
                    audioManager.StartCoroutine(audioManager.ChangeHubCharacterTrack(AudioManager.HubTracks.EMBRODYLE));
                    break;
                case "Inkwell":
                    //No inkwell theme yet
                    //Call GUI event
                    break;
                case "Escort":
                    StartCoroutine(ragdollMain.GoToCheckpoint(currentNPC.transform.position, currentNPC.GetComponent<Animator>()));
                    break;
                case "Rudy":
                    //Start Tutorial
                    break;
            }
        }

    }

    
}
