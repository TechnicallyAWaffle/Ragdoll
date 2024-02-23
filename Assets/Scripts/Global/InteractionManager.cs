using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private RagdollMain ragdollMain;
    [SerializeField] private GameObject EToInteract;

    private GameObject currentNPC;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("yippeee");
        if (collision.gameObject.tag == "NPC")
        {
            GameObject npc = collision.gameObject;
            currentNPC = npc;
            EToInteract.SetActive(true);
            EToInteract.transform.position = new Vector3(npc.transform.position.x, -1, -1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EToInteract.SetActive(false);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("yorb");
            EToInteract.SetActive(false);
            switch (currentNPC.name)
            {
                case "Ms.Pretty":
                    //Access health
                    break;
                case "Pearce":
                    //Call GUI event
                    break;
                case "Embrodyle":
                    //Store dropped items in lost items list in SceneLoader. Move one random one into inventory when this is called.
                    //Go on date
                    break;
                case "Inkwell":
                    //Call GUI event
                    break;
            }
        }
       
    }
}
