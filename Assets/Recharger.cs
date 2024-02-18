using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recharger : MonoBehaviour
{
    private GameObject ragdoll;
    private void Start()
    {
        ragdoll = GameObject.FindGameObjectWithTag("Ragdoll");
    }
}
