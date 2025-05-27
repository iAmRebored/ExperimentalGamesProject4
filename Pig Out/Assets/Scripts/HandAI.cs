using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandAI : MonoBehaviour
{
    [Header("Grabbing")]
    public Transform handTransform;
    public Transform heldFoodPosition;
    public Transform mouthTransform;
    public float grabRange = 3f;
    public float handSpeed = 5f;
    public float pullSpeed = 8f;
    public float dropCoolDown = 3f;
    public LayerMask foodLayer;

    private GameObject targetFood;
    public bool isGrabbing = false;
    public bool isMovingToMouth = false;

    public bool paused;

    void Update()
    {
        if (paused || isGrabbing || isMovingToMouth)
            return;

        FindAndStartGrabbing();
    }

    void FindAndStartGrabbing()
    {
        Collider[] nearbyFood = Physics.OverlapSphere(transform.position, grabRange, foodLayer);

        GameObject closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider col in nearbyFood)
        {
            FoodItem item = col.GetComponent<FoodItem>();
            if (item != null && !item.isGrabbed && (item.isTargetedBy != this.gameObject))
            {
                float dist = Vector3.Distance(handTransform.position, col.transform.position);
                if (dist < closestDistance)
                {
                    closest = col.gameObject;
                    closestDistance = dist;
                }
            }
        }

        if (closest != null)
        {
            targetFood = closest;
            FoodItem foodItem = targetFood.GetComponent<FoodItem>();
            foodItem.isTargetedBy = this.gameObject;
            StartCoroutine(GrabAndMoveToMouth(targetFood));
        }
    }

    IEnumerator GrabAndMoveToMouth(GameObject food)
    {
        isGrabbing = true;
        FoodItem foodItem = food.GetComponent<FoodItem>();

        // Move hand to food
        while (Vector2.Distance(
                    new Vector2(handTransform.position.x, handTransform.position.z),
                    new Vector2(food.transform.position.x, food.transform.position.z)
                ) > 0.1f)
        {
            if (foodItem.isGrabbed)
            {
                ResetGrabbing();
                yield break;
            }
            Debug.Log("moving to food");
            Vector3 targetPosXZ = new Vector3(food.transform.position.x, handTransform.position.y, food.transform.position.z);
            handTransform.position = Vector3.MoveTowards(handTransform.position, targetPosXZ, handSpeed * Time.deltaTime);
            yield return null;
        }
        Debug.Log("reached food");

        // If it's still free, grab it
        if (foodItem.isGrabbed)
        {
            ResetGrabbing();
            yield break;
        }

        foodItem.isGrabbed = true;
        food.transform.localPosition = Vector3.zero;

        Rigidbody rb = food.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;

        // Move hand to mouth
        isMovingToMouth = true;
        while (Vector2.Distance(
                new Vector2(handTransform.position.x, handTransform.position.z),
                new Vector2(mouthTransform.position.x, mouthTransform.position.z)
                ) > 0.2f)
        {
            if (!food || !foodItem.isGrabbed)
            {
                ResetGrabbing();
                yield break;
            }

            Vector3 mouthPosXZ = new Vector3(mouthTransform.position.x, handTransform.position.y, mouthTransform.position.z);
            handTransform.position = Vector3.MoveTowards(handTransform.position, mouthPosXZ, pullSpeed * Time.deltaTime);
            targetFood.transform.position = heldFoodPosition.position;
            yield return null;
        }

        // Notify food it is ready to be eaten
        foodItem.isGrabbed = false;
        foodItem.isTargetedBy = null;
        if (rb) rb.isKinematic = false;

        ResetGrabbing();
    }

    void ResetGrabbing()
    {
        targetFood = null;
        isGrabbing = false;
        isMovingToMouth = false;
        paused = true;
        float timer = dropCoolDown;
        StartCoroutine(DropCooldown(timer));
    }

    IEnumerator DropCooldown(float duration)
    {
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            yield return null;
        }
        paused = false;
    }
}
