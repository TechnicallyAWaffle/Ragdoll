using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneConfig : MonoBehaviour
{
    public List<GameObject> sceneElements = new List<GameObject>();
    public List<GameObject> needReference = new List<GameObject>();
    public GameObject ragdoll;
    public GameObject escort;
    public Vector3 respawnPoint;
}
