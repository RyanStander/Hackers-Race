using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private Collider doorCollider;
    [SerializeField] private float openSpeed = 1f;
    [SerializeField] private float totalYMovement = 0f;
    
    //When the player is inside the collider, turn off the collider and open the door
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorCollider.enabled = false; // Disable the collider
            Open();
        }
    }
    
    private void Open()
    {
        StartCoroutine(OpenDoorCoroutine());
    }
    
    private IEnumerator OpenDoorCoroutine()
    {
        Vector3 startPosition = door.transform.position;
        Vector3 endPosition = new Vector3(startPosition.x, totalYMovement + startPosition.y, startPosition.z);
        
        float elapsedTime = 0f;
        while (elapsedTime < openSpeed)
        {
            door.transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / openSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        door.transform.position = endPosition; // Ensure the door ends at the exact position
    }
}
