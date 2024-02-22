using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private RagdollMain ragdollMain;
    [SerializeField] private float interactRadius;

    public void Interact()
    {
        Collider[] colliders = Physics.OverlapSphere(ragdollMain.transform.position, interactRadius);
        foreach (Collider collider in colliders)
        {
            switch (collider.tag)
            {
                case "Ms.Pretty":
                    //Access health
                    break;
                case "Pearce":
                    //Call GUI event
                    break;
                case "Embrodyle":
                    //Store dropped items in lost items list in SceneLoader. Move one random one into inventory when this is called.
                    //Go on date
                    break;
                case "Inkwell":
                    //Call GUI event
                    break;
            }
        }
    }
}
