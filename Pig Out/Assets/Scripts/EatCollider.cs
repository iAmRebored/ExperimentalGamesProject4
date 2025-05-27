using UnityEngine;

public class EatCollider : MonoBehaviour
{
    public Player playerRef;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            playerRef.StartEating(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            // Optional: implement CancelEating(other.gameObject) if you want to interrupt that specific food
            playerRef.CancelEating(other.gameObject); 
        }
    }
}
