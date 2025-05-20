using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    //public TMP_Text playerScore;
    public TMP_Text opponentScore;
    //public GameObject player;
    public List<GameObject> opponents;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //playerScore.text = player.GetComponent<>().points;
        string scores = "";
        int opponentNumber = 1;
        foreach (var opponent in opponents)
        {
            scores += "Opponent " + opponentNumber + ": ";
            scores += opponent.GetComponent<PigAI>().points;
            scores += '\n';
        }
        opponentScore.text = scores;
    }
}
