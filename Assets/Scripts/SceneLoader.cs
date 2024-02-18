using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    private int sceneIndex = 0;
    [SerializeField] private GameObject[] dontDestroyOnLoad;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this);
        foreach(GameObject currentObject in dontDestroyOnLoad)
            DontDestroyOnLoad(currentObject);
    }

    // Update is called once per frame

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
