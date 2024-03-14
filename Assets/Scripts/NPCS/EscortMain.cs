using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EscortMain : MonoBehaviour, IManageable
{
    private GameManager gameManager;
    private RagdollMain ragdollMain;

    public string destination;

    public void GetGameManager(GameManager gameManager)
    {
        Debug.Log("this ran");
        this.gameManager = gameManager;
    }

    public void ExitAnimationEnd()
    {
        gameObject.GetComponent<Animator>().SetBool("isOpen", false);
        gameManager.ragdollMain.TogglePlayerVisible(true);
        gameManager.ragdollMain.gameObject.GetComponent<PlayerInput>().enabled = true;
        gameManager.ragdollMain.gameObject.transform.Find("InteractionManager").GetComponent<BoxCollider2D>().enabled = true;
    }

    public void EnterAnimationEnd()
    {
        StartCoroutine(gameManager.LoadScene(destination));
    }

}
