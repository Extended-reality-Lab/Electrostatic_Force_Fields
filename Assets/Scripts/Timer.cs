using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    float timeRemaining = 0;
    bool countdown = false;
    public StageManager stageManager;
    float minutes = 0;
    float seconds = 0;
    public TextMeshProUGUI timeText;

    public void SetTextSource(TextMeshProUGUI t)
    {
        timeText = t;
    }
    void Update()
    {
        if (countdown)
        {
            if (timeRemaining  > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else{
                countdown = false;
                timeRemaining = 0;
                stageManager.Done();
            }
        }
    }

    public void DisableCountdown()
    {
        countdown = false;
    }
    public void startTimer(float tr)
    {
        timeRemaining = tr;
        countdown = true;
    }

    public float getTime()
    {
        return timeRemaining;
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}