using UnityEngine;

public class HandGrabFood : MonoBehaviour
{
    public Camera playerCamera;
    public float rayDistance = 2.0f;     // How far forward the ray goes
    public Transform heldFoodPosition; // Position where the food will be held
    public LayerMask foodLayer;         // Set this to include only the "Food" layer

    private GameObject heldFood;

    void Update()
    {
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
            heldFood.transform.position = transform.position;
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
