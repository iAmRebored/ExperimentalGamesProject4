using System.Collections.Generic;
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
    public AudioSource throwUpSound;

    [Header("Eating")]
    public ParticleSystem eatingEffect;
    public AudioSource eatingSound;

    private List<GameObject> foodsBeingEaten = new List<GameObject>();
    private Dictionary<GameObject, float> foodTimers = new Dictionary<GameObject, float>();
    private float throwUpTimer = 0f;

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

        if (foodsBeingEaten.Count == 0)
        {
            state = State.Idle;
            return;
        }

        List<GameObject> finishedFoods = new List<GameObject>();

        foreach (var food in foodsBeingEaten)
        {
            foodTimers[food] -= Time.deltaTime;

            if (foodTimers[food] <= 0f)
            {
                finishedFoods.Add(food);
            }
        }

        foreach (var food in finishedFoods)
        {
            ConsumeFood(food);
        }
    }

    void HandleThrowingUp()
    {
        throwUpTimer -= Time.deltaTime;

        fullness -= fullnessRecoveryFromThrowUp * Time.deltaTime;
        if (fullness < 0) fullness = 0;
        if (throwUpTimer <= 0f)
        {
            UpdateScore(-penalty);
            vomitEffect.Stop();
            throwUpTime += 1f; // Increase throw up time for next time
            penalty += 10f * penaltyMultiplier; // Increase penalty over time
            state = State.Idle;
            //Debug.Log("Finished throwing up...");
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
        if (state == State.ThrowingUp || foodsBeingEaten.Contains(food)) return;

        foodsBeingEaten.Add(food);

        FoodItem foodItem = food.GetComponent<FoodItem>();
        float baseEatTime = foodItem != null ? foodItem.eatTime : 3f;
        float timer = baseEatTime * eatingTimeMultiplier;
        foodTimers[food] = timer;

        eatingEffect.startColor = foodItem != null ? foodItem.GetComponent<Renderer>().material.color : Color.white;
        eatingEffect.Play();
        eatingSound.Play();

        state = State.Eating;

        //Debug.Log("Started eating " + food.name);
    }

    private void ConsumeFood(GameObject food)
    {
        if (food != null)
        {
            FoodItem foodItem = food.GetComponent<FoodItem>();

            if (foodItem != null)
            {
                UpdateScore(foodItem.points);
                UpdateFullness(foodItem.fullness);
            }

            Destroy(food);
            foodsBeingEaten.Remove(food);
            foodTimers.Remove(food);

            //Debug.Log("Finished eating " + food.name);
        }

        if (foodsBeingEaten.Count == 0)
        {
            StopEating();
        }
    }

    public void CancelEating(GameObject food)
    {
        if (foodsBeingEaten.Contains(food))
        {
            foodsBeingEaten.Remove(food);

            //Debug.Log("Canceled eating: " + food.name);

            // If that was the last item and state is Eating, return to Idle
            if (foodsBeingEaten.Count == 0 && state == State.Eating)
            {
                StopEating();
            }
        }
    }

    public void StopEating()
    {
        foodsBeingEaten.Clear();
        foodTimers.Clear();
        eatingSound.Stop();
        eatingEffect.Stop();

        if (state == State.Eating)
            state = State.Idle;

        //Debug.Log("Stopped eating.");
    }

    private void StartThrowingUp()
    {
        state = State.ThrowingUp;
        throwUpTimer = throwUpTime;
        vomitEffect.Play();
        throwUpSound.Play();
        foodsBeingEaten.Clear();
        foodTimers.Clear();
        eatingEffect.Stop();

        //Debug.Log("Too full! Throwing up...");
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
