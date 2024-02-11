using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInteractive : MonoBehaviour
{
    protected bool isActive = false; // Tracks the state of the object

    public virtual void Activate()
    {
        if (!isActive)
        {
            Debug.Log($"{this.gameObject.name} Activated!");
            isActive = true;
            ToggleSubObjects(true);
        }
    }

    public virtual void Deactivate()
    {
        if (isActive)
        {
            Debug.Log($"{this.gameObject.name} Deactivated!");
            isActive = false;
            ToggleSubObjects(false);
        }
    }

    private void ToggleSubObjects(bool activate)
    {
        foreach (Transform child in transform)
        {
            var panel = child.GetComponent<SlidingPanel>();
            if (panel != null)
            {
                if (activate)
                    panel.Activate();
                else
                    panel.Deactivate();
            }
            // Add additional component checks here as needed for other types of interactable sub-objects
        }
    }
}
