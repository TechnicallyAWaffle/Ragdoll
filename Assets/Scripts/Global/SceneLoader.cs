using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    private int sceneIndex = 0;
    [SerializeField] private GameObject[] dontDestroyOnLoad;
    [SerializeField] private AudioManager audioManager;

    // Start is called before the first frame update
    void Awake()
    {
        //FOR TESTING PURPOSES ONLY
        // audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        // DontDestroyOnLoad(this);
        // foreach(GameObject currentObject in dontDestroyOnLoad)
        //     DontDestroyOnLoad(currentObject);

        // audioManager.PlaySceneTrack();
    }

    // Update is called once per frame

    public void LoadScene()
    {
        
        SceneManager.LoadScene(sceneIndex);
    }
}
