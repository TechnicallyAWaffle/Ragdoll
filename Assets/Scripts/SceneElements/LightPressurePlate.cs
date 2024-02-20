using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPressurePlate : MonoBehaviour, ITriggerable
{
    [SerializeField] private GameObject[] linkedObjects;

    public void ActivateLinked()
    {
        foreach (GameObject linkedObject in linkedObjects)
            linkedObject.GetComponent<IActivateable>().Activate();
    }

    public void DeactivateLinked()
    {
        foreach (GameObject linkedObject in linkedObjects)
            linkedObject.GetComponent<IActivateable>().Deactivate();
    }
}
