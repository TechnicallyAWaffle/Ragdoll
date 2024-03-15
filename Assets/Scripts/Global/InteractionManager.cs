using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    [SerializeField] private GameObject EToInteract;
    private GameObject thisEToInteract;

    private Pair currentPair;
    private EscortMain currentEscort; // Reference to an active EscortMain component, if any

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        thisEToInteract = Instantiate(EToInteract);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pair"))
        {
            currentPair = collision.GetComponent<Pair>();
            if (currentPair != null && currentPair.npcs.Count > 0)
            {
                ActivateEToInteract(collision.transform.position);
            }
        }
        else if (collision.CompareTag("Escort")) // Assuming "Escort" is the tag for your EscortMain object
        {
            currentEscort = collision.GetComponent<EscortMain>();
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
            if (currentPair != null)
            {
                // Interact with NPCs in the Pair
                foreach (GameObject npc in currentPair.npcs)
                {
                    Debug.Log($"Interacting with NPC: {npc.name}");
                }
                Debug.Log($"Total NPCs in this Pair: {currentPair.npcs.Count}");
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
