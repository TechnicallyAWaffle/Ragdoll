using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance { get; private set; }
    public Dictionary<string, GameObject> npcPrefabs; // Map NPC identifiers to their prefabs
    private List<string> unlockedNPCs = new List<string>(); // Stores unlocked NPC identifiers

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

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DeserializeNPCs();
    }

    public void NPCUnlocked(NPC npc)
    {
        if (!unlockedNPCs.Contains(npc.npcIdentifier))
        {
            unlockedNPCs.Add(npc.npcIdentifier);
            // Consider saving unlockedNPCs list here if persistent saving is desired
        }
    }

    public void DeserializeNPCs()
    {
        // Instantiate NPCs based on the unlockedNPCs list
        foreach (var npcIdentifier in unlockedNPCs)
        {
            if (npcPrefabs.TryGetValue(npcIdentifier, out GameObject prefab))
            {
                Instantiate(prefab, Vector3.zero, Quaternion.identity);
                // Additional setup can be done here if necessary
            }
        }
    }

    // Add a method to setup npcPrefabs mapping, this could be called in Awake or manually in the editor
    public void SetupNPCPrefabs(Dictionary<string, GameObject> prefabs)
    {
        npcPrefabs = prefabs;
    }
}
