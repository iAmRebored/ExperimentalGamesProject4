using UnityEngine;

public class HandGrabFood : MonoBehaviour
{
    public Camera playerCamera;
    public float rayDistance = 2.0f;     // How far forward the ray goes
    public Transform heldFoodPosition; // Position where the food will be held
    public LayerMask foodLayer;         // Set this to include only the "Food" layer

    private GameObject heldFood;
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    void Update()
    {
        if (player.state == Player.State.ThrowingUp)
        {
            return; // Don't allow grabbing food while throwing up
        }

        if (Input.GetMouseButtonDown(0))
        {
            TryGrabFood();
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseFood();
        }

        if (heldFood)
        {
            heldFood.transform.position = heldFoodPosition.position;
        }
    }

    void TryGrabFood()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, foodLayer))
        {
            GameObject food = hit.collider.gameObject;
            heldFood = food;

            // Optional: disable physics while holding
            if (heldFood.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = true;
            }
        }
    }

    void ReleaseFood()
    {
        if (heldFood)
        {
            // Optional: re-enable physics
            if (heldFood.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = false;
            }

            heldFood = null;
        }
    }
}
