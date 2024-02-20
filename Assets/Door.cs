using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IActivateable
{

    [SerializeField] private Vector3 unactivePosition;
    [SerializeField] private Vector3 activePosition;
    [SerializeField] private bool isSwingingDoor;
    [SerializeField] private float doorSpeed = 5;
    public void Activate()
    {
        StopAllCoroutines();
        StartCoroutine(MoveDoor(activePosition));
    }

    public void Deactivate()
    {
        StopAllCoroutines();
        StartCoroutine(MoveDoor(unactivePosition));
    }

    private IEnumerator MoveDoor(Vector3 destination)
    {
        while(transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, doorSpeed);
            yield return new WaitForEndOfFrame();
        }
    }
}
