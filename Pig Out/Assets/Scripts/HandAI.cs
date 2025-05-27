using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Collections;
using UnityEngine;

public class HandAI : MonoBehaviour
{
    [Header("Grabbing")]
    public Transform handTransform;
    public Transform mouthTransform;
    public float grabRange = 3f;
    public float handSpeed = 5f;
    public float pullSpeed = 8f;
    public LayerMask foodLayer;

    private GameObject targetFood;
    private bool isGrabbing = false;
    private bool isMovingToMouth = false;

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
            if (item != null && !item.isGrabbed)
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
            StartCoroutine(GrabAndMoveToMouth(targetFood));
        }
    }

    IEnumerator GrabAndMoveToMouth(GameObject food)
    {
        isGrabbing = true;
        FoodItem foodItem = food.GetComponent<FoodItem>();

        // Move hand to food
        while (Vector3.Distance(handTransform.position, food.transform.position) > 0.1f)
        {
            if (foodItem.isGrabbed)
            {
                ResetGrabbing();
                yield break;
            }

            Vector3 targetPosXZ = new Vector3(food.transform.position.x, handTransform.position.y, food.transform.position.z);
            handTransform.position = Vector3.MoveTowards(handTransform.position, targetPosXZ, handSpeed * Time.deltaTime);
            yield return null;
        }

        // If it's still free, grab it
        if (foodItem.isGrabbed)
        {
            ResetGrabbing();
            yield break;
        }

        foodItem.isGrabbed = true;
        food.transform.SetParent(handTransform);
        food.transform.localPosition = Vector3.zero;
        if (food.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }

        // Move hand to mouth
        isMovingToMouth = true;
        while (Vector3.Distance(handTransform.position, mouthTransform.position) > 0.1f)
        {
            if (!food || !foodItem.isGrabbed)
            {
                ResetGrabbing();
                yield break;
            }

            Vector3 mouthPosXZ = new Vector3(mouthTransform.position.x, handTransform.position.y, mouthTransform.position.z);
            handTransform.position = Vector3.MoveTowards(handTransform.position, mouthPosXZ, pullSpeed * Time.deltaTime);
            yield return null;
        }

        // Drop and destroy food
        food.transform.SetParent(null);
        rb.isKinematic = false;
        ResetGrabbing();
    }

    void ResetGrabbing()
    {
        if (targetFood != null)
        {
            FoodItem item = targetFood.GetComponent<FoodItem>();
            if (item != null)
            {
                item.isGrabbed = false;
            }

            if (targetFood.transform.parent == handTransform)
            {
                targetFood.transform.SetParent(null);
            }
        }

        targetFood = null;
        isGrabbing = false;
        isMovingToMouth = false;
    }
}
