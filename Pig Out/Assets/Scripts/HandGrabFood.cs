using UnityEngine;

public class HandGrabFood : MonoBehaviour
{
    public Camera playerCamera;
    public float rayDistance = 2.0f;     // How far forward the ray goes
    public Transform heldFoodPosition; // Position where the food will be held
    public LayerMask foodLayer;         // Set this to include only the "Food" layer

    private GameObject heldFood;
    private Player player;

    private Vector3 previousPosition;
    private Vector3 handVelocity;

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

        // Track velocity of the hand
        handVelocity = (heldFoodPosition.position - previousPosition) / Time.deltaTime;
        previousPosition = heldFoodPosition.position;
    }

    void TryGrabFood()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, foodLayer))
        {
            GameObject food = hit.collider.gameObject;

            // Check if it's already being grabbed
            FoodItem foodItem = food.GetComponent<FoodItem>();
            if (foodItem != null && foodItem.isGrabbed)
            {
                return; // Skip if already grabbed
            }

            heldFood = food;

            // Mark as grabbed
            if (foodItem != null)
            {
                foodItem.isGrabbed = true;
            }

            // Optional: disable physics while holding
            if (heldFood.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = true;
            }

            previousPosition = heldFoodPosition.position;
            handVelocity = Vector3.zero;
        }
    }

    void ReleaseFood()
    {
        if (heldFood)
        {
            // Mark as not grabbed
            FoodItem foodItem = heldFood.GetComponent<FoodItem>();
            if (foodItem != null)
            {
                foodItem.isGrabbed = false;
            }

            // Optional: re-enable physics
            if (heldFood.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = false;

                rb.linearVelocity = handVelocity;
            }

            heldFood = null;
        }
    }
}
