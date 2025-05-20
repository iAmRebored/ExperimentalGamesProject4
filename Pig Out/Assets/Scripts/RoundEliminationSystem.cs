using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RoundEliminationSystem : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> opponents;

    public static void StartRound()
    {
        PigAI.roundStarted = true;
        Timer.timeRemaining = 60;
    }

    public static void EndRound()
    {
        PigAI.roundStarted = false;
    }

    private void EliminateCompetitor()
    {
        GameObject lowestScoreCompetitor = player;
        float lowestScore = player.GetComponent<Player>().points;
        int index = -1;
        for (int i = 0; i < opponents.Count; i++)
        {
            if (lowestScore > opponents[i].GetComponent<PigAI>().points)
            {
                lowestScore = opponents[i].GetComponent<PigAI>().points;
                lowestScoreCompetitor = opponents[i];
                index = i;
            }
        }
        if (index != -1)
        {
            opponents.RemoveAt(index);
            lowestScoreCompetitor.gameObject.SetActive(false);
            lowestScoreCompetitor.GetComponent<PigAI>().eliminated = true;
        }
        else
        {
            //Player lost
        }
    }
}
