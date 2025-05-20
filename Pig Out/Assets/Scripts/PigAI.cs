using UnityEngine;

public class PigAI : MonoBehaviour
{
    private enum State
    {
        Idle,
        Eating,
        ThrowingUp,
    }
    private int fullness = 0;
    [Tooltip("How much they can eat before throwing up")]
    public int maxFullness;
    [Tooltip("How fast should they be getting full")]
    public int fullnessIncrease;
    [Tooltip("Recovery speed while idle")]
    public int fullnessRecovery;
    [Tooltip("Recovery speed while throwing up")]
    public int fullnessRecoveryFromThrowUp;
    [Tooltip("When they should stop eating")]
    public int fullnessStoppingPoint;
    [Tooltip("When they should start eating")]
    public int fullnessStartingPoint;
    private bool updateStats = true;
    public static bool roundStarted = true;
    [Tooltip("How many points should they be getting while eating")]
    public float pointIncrease;
    public float points = 0f;
    public float penaltyMultiplier;
    public float penalty;
    [Tooltip("How long they throw up for")]
    public float throwUpTime;

    private State state;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (updateStats && roundStarted)
        {
            UnityEngine.Debug.Log(state);
            if (state == State.Idle)
            {
                if (fullness > 1)
                {
                    fullness -= fullnessRecovery;
                }
            } else if (state == State.Eating)
            {
                fullness += fullnessIncrease;
                points += pointIncrease;
            } else if (state == State.ThrowingUp)
            {
                fullness -= fullnessRecoveryFromThrowUp;
            }
            //This section determines whether it should change states
            if (fullness > maxFullness)
            {
                OnThrowingUp();
            } else if (fullness > fullnessStoppingPoint)
            {
                OnIdle();
            } else if (fullness < fullnessStartingPoint)
            {
                OnEating();
            }
            //Wait 5 seconds before setting updateStats back to true
            updateStats = false;
            FunctionTimer.Create(() => setUpdateStats(true), 5f);
        }
    }

    public void OnIdle()
    {
        state = State.Idle;
    }

    public void OnEating()
    {
        state = State.Eating;
    }

    public void OnThrowingUp()
    {
        state = State.ThrowingUp;
        points -= (penalty * penaltyMultiplier);
        penaltyMultiplier = penaltyMultiplier * 1.5f;
        //Stop throwing up after the set amount of time has passed
        FunctionTimer.Create(() => OnIdle(), throwUpTime);
    }

    public void setUpdateStats(bool updateS)
    {
        updateStats = updateS;
    }
}
