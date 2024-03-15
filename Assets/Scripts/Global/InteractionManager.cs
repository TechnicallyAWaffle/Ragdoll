using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InteractionManager : MonoBehaviour
{
    //public static InteractionManager Instance { get; private set; }

    [SerializeField] private GameObject EToInteract;
    //For testing
    private GameObject thisEToInteract;

    private Pair currentPair;
    private EscortMain currentEscort; // Reference to an active EscortMain component, if any

    void Awake()
    {
        //if (Instance == null)
       // {
      //      Instance = this;
     //       DontDestroyOnLoad(gameObject);
     //   }
     //   else
     //   {
     //       Destroy(gameObject);
     //   }
    }
    private void Start()
    {
        thisEToInteract = Instantiate(EToInteract);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pair"))
        {
            currentPair = collision.gameObject.GetComponent<Pair>();
            if (currentPair != null && currentPair.npcs.Count > 0)
            {
                ActivateEToInteract(collision.transform.position);
            }
        }
        else if (collision.CompareTag("Escort")) // Assuming "Escort" is the tag for your EscortMain object
        {
            currentEscort = collision.gameObject.GetComponent<EscortMain>();
            if (currentEscort != null)
            {
                ActivateEToInteract(collision.transform.position);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Pair") || collision.CompareTag("Escort"))
        {
            if (thisEToInteract) thisEToInteract.SetActive(false);
            currentPair = null;
            currentEscort = null;
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            string speakers = "";
            if (currentPair != null)
            {
                // Interact with NPCs in the Pair
                foreach (GameObject npc in currentPair.npcs)
                {
                    Debug.Log($"Interacting with NPC: {npc.name}");
                }
                Debug.Log($"Total NPCs in this Pair: {currentPair.npcs.Count}");

                if (DialogueManager.GetInstance().dialogueIsPlaying)
                    DialogueManager.GetInstance().ContinueStory();
                thisEToInteract.SetActive(false);
                switch (speakers)
                {
                    case "Ms. Pretty":
                        //audioManager.StartCoroutine(audioManager.ChangeHubCharacterTrack(AudioManager.HubTracks.MSPRETTY));
                        DialogueManager.GetInstance().EnterDialogueMode(speakers);
                        break;
                    case "Pearce":
                        //Call GUI event
                        //audioManager.StartCoroutine(audioManager.ChangeHubCharacterTrack(AudioManager.HubTracks.PEARCE));
                        DialogueManager.GetInstance().EnterDialogueMode(speakers);
                        break;
                    case "Embrodyle":
                        //Store dropped items in lost items list in SceneLoader. Move one random one into inventory when this is called.
                        //Go on date
                        //audioManager.StartCoroutine(audioManager.ChangeHubCharacterTrack(AudioManager.HubTracks.EMBRODYLE));
                        DialogueManager.GetInstance().EnterDialogueMode(speakers);
                        break;
                    case "Inkwell":
                        //No inkwell theme yet
                        //Call GUI event
                        break;
                    case "Escort":
                        //StartCoroutine(ragdollMain.GoToCheckpoint(currentNPC.transform.position, currentNPC.GetComponent<Animator>()));
                        DialogueManager.GetInstance().EnterDialogueMode(speakers);
                        break;
                    case "":
                        Debug.Log("test1ran");
                        DialogueManager.GetInstance().EnterDialogueMode(speakers);
                        break;
                }
            }
            else if (currentEscort != null)
            {
                // Trigger scene transition via EscortMain
                Debug.Log($"Activating Escort to {currentEscort.destination}");
                currentEscort.GetComponent<Animator>().SetTrigger("CheckpointEnter");

                GameManager.Instance.ragdollMain.TogglePlayerVisible(false);
                GameManager.Instance.ragdollMain.gameObject.GetComponent<PlayerInput>().enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    private void ActivateEToInteract(Vector3 position)
    {
        thisEToInteract.SetActive(true);
        thisEToInteract.transform.position = new Vector3(position.x, -1, -1); // Adjust as necessary
    }

}
