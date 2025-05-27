using UnityEngine;

public class SweatParticlePlayer : MonoBehaviour
{
    public ParticleSystem sweatParticleSystem;
    private RoundEliminationSystem roundEliminationSystem;

    private bool isPlaying = false;

    private void Awake()
    {
        roundEliminationSystem = FindObjectOfType<RoundEliminationSystem>();
        if (sweatParticleSystem == null)
        {
            Debug.LogWarning("Sweat Particle System not assigned on " + gameObject.name);
        }
    }

    void LateUpdate()
    {
        if (roundEliminationSystem == null || sweatParticleSystem == null)
            return;

        bool shouldPlay = roundEliminationSystem.lowestScorer == this.gameObject;

        if (shouldPlay && !isPlaying)
        {
            sweatParticleSystem.Play();
            isPlaying = true;
        }
        else if (!shouldPlay && isPlaying)
        {
            sweatParticleSystem.Stop();
            isPlaying = false;
        }
    }
}
