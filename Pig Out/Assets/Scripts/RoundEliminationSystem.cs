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
        Timer.timeRemaining = 30;
    }

    public static void EndRound()
    {
        PigAI.roundStarted = false;
    }

    public void EliminateCompetitor()
    {
        GameObject lowestScoreCompetitor = player;
        float lowestScore = player.GetComponent<Player>().points;
        player.GetComponent<Player>().points = 0;
        int index = -1;
        for (int i = 0; i < opponents.Count; i++)
        {
            if (lowestScore > opponents[i].GetComponent<PigAI>().points)
            {
                lowestScore = opponents[i].GetComponent<PigAI>().points;
                opponents[i].GetComponent<PigAI>().points = 0;
                lowestScoreCompetitor = opponents[i];
                index = i;
            }
        }
        if (index != -1)
        {
            opponents.RemoveAt(index);
            lowestScoreCompetitor.gameObject.SetActive(false);
            lowestScoreCompetitor.GetComponent<PigAI>().eliminated = true;
            if (opponents.Count == 0)
            {
                //Player won
            }
        }
        else
        {
            //Player lost go to game over screen
        }
    }
}
