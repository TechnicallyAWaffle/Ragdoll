using UnityEngine;

public class Cage : MonoBehaviour
{
    public NPC containedNPC;
    [SerializeField] private Animator cageAnimator;

    void Awake()
    {
        cageAnimator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("RagdollHead"))
        {
            Debug.Log("Collided with RagdollHead");
            UnlockNPC();
        }
    }

    private void UnlockNPC()
    {
        cageAnimator.SetTrigger("Unlock");
        containedNPC.gameObject.SetActive(true);
        GetComponent<Collider2D>().enabled = false;
        NPCManager.Instance.NPCUnlocked(containedNPC);
    }
}
