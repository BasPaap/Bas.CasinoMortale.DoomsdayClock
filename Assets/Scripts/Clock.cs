using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private float numSecondsAtStart;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI fractionText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private float audioStartTime = 7 + 1.2f;
    [SerializeField] private float gameOverStartTime = 0;

    private float startTime;

    private void Start()
    {
        startTime = Time.time;
        gameOverText.color = Color.clear;
    }

    private void Update()
    {
        var numElapsedSeconds = Time.time - startTime;
        float numSecondsRemaining = numSecondsAtStart - numElapsedSeconds;
        var timeToDisplay = TimeSpan.FromSeconds(numSecondsRemaining);

        string timeFormat = "h':'mm':'ss";
        string fractionFormat = "'.'fff";
    
        if (numSecondsRemaining < audioStartTime && !audioPlayer.isPlaying)
        {
            audioPlayer.Play();
        }

        if (numSecondsRemaining < gameOverStartTime && gameOverText.color == Color.clear)
        {
            gameOverText.color = Color.red;
        }

        if (numSecondsRemaining > 5f && numSecondsRemaining < 7)
        {
            timeToDisplay = TimeSpan.FromSeconds(7);
        }
        if (numSecondsRemaining < 5f)
        {
            timeText.text = "0:07";
            fractionText.text = string.Empty;
        }
        else
        {
            timeText.text = timeToDisplay.ToString(timeFormat);
            fractionText.text = timeToDisplay.ToString(fractionFormat);
        }        
    }
}
