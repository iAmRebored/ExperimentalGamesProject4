using UnityEngine;


public class Player : MonoBehaviour
{
    public enum State
    {
        Idle,
        Eating,
        ThrowingUp
    }

    public State state = State.Idle;

    [Header("Fullness")]
    public float fullness = 0;
    public float maxFullness = 100;
    public float fullnessRecovery = 1;
    public float fullnessRecoveryFromThrowUp = 10;

    [Header("Modifiers")]
    public float eatingTimeMultiplier = 1.0f;
    public float pointsMultiplier = 1.0f;
    public float penaltyMultiplier = 1.0f;

    [Header("Scoring")]
    public float points = 0;
    public float penalty = 0;

    [Header("Throw Up")]
    public float throwUpTime = 3f;
    public ParticleSystem vomitEffect;

    [Header("Eating")]
    public bool isEating = false;
    public GameObject currentFood;
    private float eatTimer = 0f;
    private float throwUpTimer = 0f;
    public ParticleSystem eatingEffect;

    void Update()
    {
        switch (state)
        {
            case State.Idle:
                RecoverFullness();
                break;
            case State.Eating:
                HandleEating();
                break;
            case State.ThrowingUp:
                HandleThrowingUp();
                break;
        }
    }

    // --------------------- State Handlers ---------------------

    void HandleEating()
    {
        if (fullness >= maxFullness)
        {
            StartThrowingUp();
            return;
        }

        if (isEating)
        {
            eatTimer -= Time.deltaTime;

            if (eatTimer <= 0f)
            {
                ConsumeFood();
            }
        }
        else
        {
            state = State.Idle;
        }
    }

    void HandleThrowingUp()
    {
        throwUpTimer -= Time.deltaTime;

        fullness -= fullnessRecoveryFromThrowUp * Time.deltaTime;
        if (fullness < 0) fullness = 0;

        if (throwUpTimer <= 0f)
        {
            vomitEffect.Stop();
            throwUpTime += 1f; // Increase throw up time for next time
            penalty += 10f * penaltyMultiplier; // Increase penalty over time
            state = State.Idle;
            Debug.Log("Finished throwing up...");
        }
    }

    void RecoverFullness()
    {
        if (fullness >= maxFullness)
        {
            StartThrowingUp();
            return;
        }

        if (fullness > 0)
        {
            fullness -= fullnessRecovery * Time.deltaTime;
            if (fullness < 0) fullness = 0;
        }
    }

    // --------------------- Eating System ---------------------

    public void StartEating(GameObject food)
    {
        if (state == State.ThrowingUp) return;

        isEating = true;
        currentFood = food;

        // Simulate getting eating time and points from the food object
        FoodItem foodItem = food.GetComponent<FoodItem>();
        float baseEatTime = foodItem != null ? foodItem.eatTime : 3f;
        eatTimer = baseEatTime / eatingTimeMultiplier;
        eatingEffect.startColor = foodItem != null ? foodItem.GetComponent<Renderer>().material.color : Color.white;
        eatingEffect.Play();

        state = State.Eating;

        Debug.Log("Started eating " + food.name);
    }

    private void ConsumeFood()
    {
        if (currentFood != null)
        {
            FoodItem foodItem = currentFood.GetComponent<FoodItem>();

            if (foodItem != null)
            {
                UpdateScore(foodItem.points);
                UpdateFullness(foodItem.fullness);
            }

            Destroy(currentFood);
        }

        StopEating();
    }

    public void StopEating()
    {
        isEating = false;
        currentFood = null;
        eatTimer = 0f;
        eatingEffect.Stop();

        if (state == State.Eating)
            state = State.Idle;

        Debug.Log("Stopped eating.");
    }

    private void StartThrowingUp()
    {
        state = State.ThrowingUp;
        throwUpTimer = throwUpTime;
        vomitEffect.Play();
        isEating = false;
        Debug.Log("Too full! Throwing up...");
    }

    // --------------------- Score & Fullness ---------------------

    public void UpdateScore(float amount)
    {
        float adjustedPoints = amount * pointsMultiplier;
        points += adjustedPoints;
        Debug.Log($"Gained {adjustedPoints} points. Total: {points}");
    }

    public void UpdateFullness(int amount)
    {
        fullness += amount;
        if (fullness > maxFullness)
        {
            fullness = maxFullness;
        }
        Debug.Log($"Fullness: {fullness}/{maxFullness}");
    }
}
