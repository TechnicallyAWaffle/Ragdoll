using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class EscortMain : MonoBehaviour
{
    public string destination = "Hub Area"; // Default scene

    public void ExitAnimationEnd()
    {
        // Access GameManager instance directly
        gameObject.GetComponent<Animator>().SetBool("isOpen", false);
        GameManager.Instance.ragdollMain.TogglePlayerVisible(true);
        GameManager.Instance.ragdollMain.gameObject.GetComponent<PlayerInput>().enabled = true;
        InteractionManager.Instance.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void EnterAnimationEnd()
    {
        // Start coroutine through GameManager instance
        StartCoroutine(GameManager.Instance.LoadScene(destination));
    }
}
