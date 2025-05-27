using UnityEngine;

public class EatCollider : MonoBehaviour
{
    public Player playerRef;
    public PigAI pigRef;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            if (playerRef != null)
            {
                playerRef.StartEating(other.gameObject);
            }
            else if (pigRef != null)
            {
                //pigRef.StartEating(other.gameObject);
            }
            else
            {
                Debug.LogWarning("No player or pig reference set for EatCollider.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            if (playerRef != null)
            {
                playerRef.CancelEating(other.gameObject);
            }
            else if (pigRef != null)
            {
                //pigRef.CancelEating(other.gameObject);
            }
        }
    }
}
