using UnityEngine;

public class EatCollider : MonoBehaviour
{
    public Player playerRef;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is tagged as "Food"
        if (other.CompareTag("Food"))
        {
            playerRef.StartEating(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting object is tagged as "Food"
        if (other.CompareTag("Food"))
        {
            playerRef.StopEating();
        }
    }

}
