using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundEliminationSystem : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> opponents;
    public AudioSource killPig;
    public GameObject lowestScorer;

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
        lowestScorer = player;
        float lowestScore = player.GetComponent<Player>().points;
        player.GetComponent<Player>().points = 0;
        int index = -1;
        for (int i = 0; i < opponents.Count; i++)
        {
            if (lowestScore > opponents[i].GetComponent<Player>().points)
            {
                lowestScore = opponents[i].GetComponent<Player>().points;
                opponents[i].GetComponent<Player>().points = 0;
                lowestScorer = opponents[i];
                index = i;
            }
        }
        if (index != -1)
        {
            opponents.RemoveAt(index);
            //lowestScoreCompetitor.gameObject.SetActive(false);
            //lowestScoreCompetitor.GetComponent<PigAI>().eliminated = true;
            Destroy(lowestScorer);
            killPig.Play();
            if (opponents.Count == 0)
            {
                SceneManager.LoadScene("WinScreen");
                //Player won
            }
        }
        else
        {
            SceneManager.LoadScene("WinScreen");
            //Player lost go to game over screen
        }
    }
}
