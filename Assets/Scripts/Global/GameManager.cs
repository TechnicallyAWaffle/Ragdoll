using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] private GameObject fadeToBlack;

    private SceneConfig sceneConfig;
    public RagdollMain ragdollMain;
    public AudioManager audioManager;
    public ItemManager itemManager;
    public NPCManager npcManager;    //Global variables
    public int currentLevel = 0;
    public int currentRegion = 1;
    public string nextLevel;
    public int ragdollHealth;

    // Start is called before the first frame update
    void Awake()
    {
        //FOR TESTING PURPOSES ONLY
        //audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(fadeToBlack);
        DontDestroyOnLoad(audioManager);
        DontDestroyOnLoad(npcManager);
        //audioManager.PlaySceneTrack();
    }

    public void LoadSceneFromButton(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    private void UpdateSceneData()
    {

        if (GameObject.FindGameObjectWithTag("SceneConfig"))
        {
            sceneConfig = GameObject.FindGameObjectWithTag("SceneConfig").GetComponent<SceneConfig>();
            foreach (GameObject element in sceneConfig.needReference)
                element.GetComponent<IManageable>().GetGameManager(this);

            //Get ragdoll reference
            this.ragdollMain = sceneConfig.ragdoll.GetComponent<RagdollMain>();

            //Set Ragdoll's new respawn point and position
            ragdollMain.respawnPoint = sceneConfig.respawnPoint;
            ragdollMain.gameObject.transform.position = ragdollMain.respawnPoint;

            //Fade out screen
            fadeToBlack.GetComponent<Animator>().SetTrigger("FadeOut");

            //Set escort destination and animation state
            sceneConfig.escort.GetComponent<EscortMain>().destination = nextLevel;
            sceneConfig.escort.GetComponent<Animator>().SetBool("isOpen", true);
        }
        else
            Debug.Log("No SceneConfig found!");
    }

    public void TogglePlayerVisible(bool toggle)
    {
        ragdollMain.ragdollBody.SetActive(toggle);
        ragdollMain.ragdollHead.SetActive(toggle);
        ragdollMain.ragdollTieLeft.SetActive(toggle);
        ragdollMain.ragdollTieRight.SetActive(toggle);
    }

    public IEnumerator LoadScene(string sceneName)
    {
            fadeToBlack.SetActive(true);
            yield return new WaitForSeconds(2f);
            var waitUntilLoaded = SceneManager.LoadSceneAsync(sceneName);
            while (!waitUntilLoaded.isDone)
                yield return null;
            UpdateSceneData();
            yield return new WaitForSeconds(1f);
            fadeToBlack.SetActive(false);
            sceneConfig.escort.GetComponent<Animator>().SetTrigger("CheckpointExit");
            if (sceneName == "Hub Area")
            {
                nextLevel = currentRegion + "-" + currentLevel;
                sceneConfig.escort.GetComponent<EscortMain>().destination = nextLevel;
            }
            //If the destination is not the hub, then it's a transition from one level to another
            else
                currentLevel++;
    }
}
