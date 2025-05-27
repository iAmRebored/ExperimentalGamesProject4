using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public static float timeRemaining;
    public TMP_Text timeText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RoundEliminationSystem.StartRound();
    }

    // Update is called once per frame
    void Update()
    {
        if (PigAI.roundStarted && timeRemaining >= 0)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        } else if (timeRemaining <= 0)
        {
            DisplayTime(0);
            RoundEliminationSystem.EndRound();
            timeRemaining = 30;
            GetComponent<RoundEliminationSystem>().EliminateCompetitor();
            FunctionTimer.Create(() => RoundEliminationSystem.StartRound(), 5f);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        //float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}", seconds);
    }
}
