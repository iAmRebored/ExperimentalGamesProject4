using UnityEngine;

public class AIEatingBehaviorMods : MonoBehaviour
{
    public HandAI handAI;
    public Player PigStats;
    public RoundEliminationSystem roundEliminationSystem;
    public float fullnessThreshold = 70f; // Above this, start delaying
    public float maxDelay = 5f; // Max delay when at or above max fullness
    public float checkInterval = 0.5f;

    private float checkTimer = 0f;
    private float fullness;
    private float maxFullness;

    private void Awake()
    {
        roundEliminationSystem = FindFirstObjectByType<RoundEliminationSystem>();
    }

    void Update()
    {
        checkTimer -= Time.deltaTime;
        if (checkTimer <= 0f)
        {
            checkTimer = checkInterval;
            AdjustHandBehavior();
        }
    }

    void AdjustHandBehavior()
    {
        if (PigStats == null || handAI == null)
            return;

        // Use reflection to access the stats dynamically
        fullness = PigStats.fullness;
        maxFullness = PigStats.maxFullness;

        float fullnessPercent = fullness / maxFullness;

        // Map fullness percent to delay using a curve or linear mapping
        float delayFactor = Mathf.InverseLerp(fullnessThreshold / maxFullness, 1f, fullnessPercent);
        float adjustedCooldown = Mathf.Lerp(handAI.dropCoolDown, maxDelay, delayFactor);

        handAI.dropCoolDown = adjustedCooldown;

        if (roundEliminationSystem.lowestScorer == this.gameObject)
        {
            // if this AI is the lowest scorer, lower the delay significantly
            handAI.dropCoolDown = Mathf.Max(adjustedCooldown * 0.5f, 0.1f); // Ensure it doesn't go too low
        }
    }
}
