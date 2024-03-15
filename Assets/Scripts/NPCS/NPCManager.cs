using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance { get; private set; }
    public List<StringGameObjectPair> npcPrefabsList = new List<StringGameObjectPair>();
    [HideInInspector]
    public Dictionary<string, GameObject> npcPrefabs = new Dictionary<string, GameObject>();

    private List<string> unlockedNPCs = new List<string>();

    // Use a list instead of a fixed-size array for flexibility
    public List<Pair> pairs = new List<Pair>(); // This will now be populated at runtime

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (StringGameObjectPair pair in npcPrefabsList)
            {
                npcPrefabs.Add(pair.Key, pair.Value);
            }
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
        // Clear the existing list to avoid duplicates
        pairs.Clear();

        // Find all GameObjects with the tag "Pair" and add them to the pairs list
        foreach (GameObject pairObject in GameObject.FindGameObjectsWithTag("Pair"))
        {
            Pair pairComponent = pairObject.GetComponent<Pair>();
            if (pairComponent != null)
            {
                pairs.Add(pairComponent);
            }
        }

        // Use a coroutine to delay the spawning of NPCs to ensure all objects are correctly initialized
        StartCoroutine(DelayedSpawnNPCs());
    }

    IEnumerator DelayedSpawnNPCs()
    {
        // Wait until the end of the frame to ensure all scene objects are fully loaded
        yield return new WaitForEndOfFrame();

        // Optionally, add a slight delay if necessary
        // yield return new WaitForSeconds(0.1f);

        // Now that the scene is fully loaded, proceed with spawning and arranging NPCs
        if (SceneManager.GetActiveScene().name == "Hub Area")
        {
            SpawnAndArrangeNPCsInHubArea();
        }
    }

    public void NPCUnlocked(NPC npc)
    {
        Debug.Log("NPC Unlocked: " + npc.name);
        if (!unlockedNPCs.Contains(npc.name))
        {
            unlockedNPCs.Add(npc.name);
            Debug.Log("NPC Unlocked: " + npc.name);
        }
    }

    public void SetupNPCPrefabs(Dictionary<string, GameObject> prefabs)
    {
        npcPrefabs = prefabs;
    }

    public void SpawnAndArrangeNPCsInHubArea()
    {
        if (SceneManager.GetActiveScene().name == "Hub Area" && pairs.Count > 0)
        {
            foreach (var npcName in unlockedNPCs)
            {
                bool placed = false;
                int tryCount = 0;

                while (!placed && tryCount < 10)
                {
                    int randomIndex = Random.Range(0, pairs.Count);
                    if (npcPrefabs.TryGetValue(npcName, out GameObject prefab))
                    {
                        Debug.Log($"Attempting to spawn NPC: {npcName} at Pair index: {randomIndex}");

                        int npcCount = pairs[randomIndex].npcs.Count;
                        Vector3 positionOffset = npcCount == 0 ? new Vector3(-1, 5, 0) : new Vector3(1, 5, 0);
                        Vector3 initialSpawnPosition = pairs[randomIndex].transform.position + positionOffset;

                        // Adjust Y position for proper floor placement before instantiation
                        Vector3 adjustedSpawnPosition = AdjustHeightForProperFloorPlacement(initialSpawnPosition, prefab);
                        Vector3 positionOffset2 = new Vector3(0, -0.07f, 0);
                        GameObject npcInstance = Instantiate(prefab, adjustedSpawnPosition + positionOffset2, Quaternion.identity);

                        Debug.Log($"Spawned {npcName} at Position: {npcInstance.transform.position}");

                        placed = pairs[randomIndex].TryAddNPC(npcInstance);

                        Animator npcAnimator = npcInstance.GetComponent<Animator>();
                        if (npcAnimator != null)
                        {
                            npcAnimator.SetTrigger("Idle");
                        }

                        if (placed)
                        {
                            Debug.Log($"Successfully spawned and placed NPC: {npcName} at Pair index: {randomIndex}, Spawn Position: {adjustedSpawnPosition}");
                        }
                        else
                        {
                            Debug.Log($"Failed to place NPC: {npcName} at Pair index: {randomIndex}, Spawn Position: {adjustedSpawnPosition}");
                        }
                    }
                    tryCount++;
                }
            }
        }
    }

    private Vector3 AdjustHeightForProperFloorPlacement(Vector3 spawnPosition, GameObject npcPrefab)
    {
        LayerMask terrainLayer = LayerMask.GetMask("Terrain");
        RaycastHit2D hit = Physics2D.Raycast(spawnPosition, Vector2.down, Mathf.Infinity, terrainLayer);
        if (hit.collider != null)
        {
            // Adjust for the NPC's sprite height. Assuming the pivot is at the sprite's center,
            // we need to add half of its height to the y-coordinate of the hit point.
            SpriteRenderer npcSpriteRenderer = npcPrefab.GetComponent<SpriteRenderer>();
            if (npcSpriteRenderer != null)
            {
                // Assuming the NPC prefab has a SpriteRenderer with the sprite set correctly
                float npcSpriteHeight = npcSpriteRenderer.bounds.size.y;
                // Calculate the adjusted Y position, placing the NPC's feet on the ground
                float correctYPosition = hit.point.y + npcSpriteHeight / 2;

                // Return the adjusted spawn position
                return new Vector3(spawnPosition.x, correctYPosition, spawnPosition.z);
            }
            else
            {
                // If the NPC prefab does not have a SpriteRenderer, just log a warning
                Debug.LogWarning("NPC prefab does not have a SpriteRenderer. Cannot adjust height based on sprite size.");
            }
        }
        else
        {
            Debug.LogWarning("Raycast did not hit any terrain. NPC will spawn at original spawn position.");
        }

        // Return the original spawn position if no terrain was found or if there's no SpriteRenderer
        return spawnPosition;
    }



    [System.Serializable]
    public class StringGameObjectPair
    {
        public string Key;
        public GameObject Value;
    }
}
