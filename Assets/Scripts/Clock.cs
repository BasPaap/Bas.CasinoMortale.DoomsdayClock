using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Clock : MonoBehaviour
{
    [SerializeField] private float numSecondsAtStart;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI fractionText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private float audioStartTime = 7 + 1.2f;
    [SerializeField] private float gameOverStartTime = 0;
    [SerializeField] private float pitchIncreaseTime = 17;
    [SerializeField] private float doubleOhSevenDisplayTime = 2f;

    private float startTime;
    private int lastBeepSecond;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        startTime = Time.time;
        gameOverText.color = Color.clear;
    }
    
    private void Update()
    {
        var numElapsedSeconds = Time.time - startTime;
        float numSecondsRemaining = numSecondsAtStart - numElapsedSeconds;
        var timeToDisplay = TimeSpan.FromSeconds(numSecondsRemaining);

        string timeFormat = "h':'mm':'ss";
        string fractionFormat = "'.'ff";
    
        if (numSecondsRemaining > 7 && (int)numSecondsRemaining != lastBeepSecond)
        {
            lastBeepSecond = (int)numSecondsRemaining;
            if (numSecondsRemaining < pitchIncreaseTime)
            {
                audioSource.pitch = 1 +  (pitchIncreaseTime - numSecondsRemaining) / pitchIncreaseTime;
            }
            audioSource.Play();
        }
        if (numSecondsRemaining < audioStartTime && !audioPlayer.isPlaying)
        {
            audioPlayer.Play();
        }

        if (numSecondsRemaining < gameOverStartTime && gameOverText.color == Color.clear)
        {
            gameOverText.color = Color.red;
        }

        if (numSecondsRemaining > doubleOhSevenDisplayTime && numSecondsRemaining < 7)
        {
            timeToDisplay = TimeSpan.FromSeconds(7);
        }
        if (numSecondsRemaining < doubleOhSevenDisplayTime)
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
