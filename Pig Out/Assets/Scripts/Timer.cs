using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeRemaining;
    public TMP_Text timeText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (PigAI.roundStarted)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:0} : {1:00}", minutes, seconds);
    }
}
