using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundEliminationSystem : MonoBehaviour
{
    public GameObject player;
    private Player playerStats;
    public List<GameObject> opponents;
    public AudioSource killPig;

    public GameObject lowestScorer;
    private GameObject previousLowestScorer;
    public int index = -1;


    void Awake()
    {
        playerStats = player.GetComponent<Player>();
    }

    void Update()
    {
        UpdateLowestScorer();
    }

    void UpdateLowestScorer()
    {
        lowestScorer = player;
        float lowestScore = playerStats.points;
        index = -1;

        for (int i = 0; i < opponents.Count; i++)
        {
            Player opponentStats = opponents[i].GetComponent<Player>();
            if (opponentStats.points < lowestScore)
            {
                lowestScore = opponentStats.points;
                lowestScorer = opponents[i];
                index = i;
            }
        }
    }

    public static void StartRound()
    {
        PigAI.roundStarted = true;
        Timer.timeRemaining = 30;
    }

    public static void EndRound()
    {
        PigAI.roundStarted = false;
    }

    public void EliminateCompetitor()
    {
        UpdateLowestScorer(); // Make sure the latest info is used

        if (index != -1)
        {
            opponents.RemoveAt(index);
            Destroy(lowestScorer);
            killPig.Play();

            if (opponents.Count == 0)
            {
                SceneManager.LoadScene("WinScreen");
            }
        }
        else
        {
            // Player is the lowest
            SceneManager.LoadScene("LossScreen");
        }
    }
}
