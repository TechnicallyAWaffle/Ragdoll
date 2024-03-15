using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Pair : MonoBehaviour
{
    public List<GameObject> npcs = new List<GameObject>(); // Holds up to two NPCs

    // Method to add an NPC to the pair if it has space
    public bool TryAddNPC(GameObject npc)
    {
        if (npcs.Count < 2)
        {
            npcs.Add(npc);
            return true;
        }
        return false;
    }
}
