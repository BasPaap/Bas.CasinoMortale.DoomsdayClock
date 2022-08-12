using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Video;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Clock : MonoBehaviour
{
    [SerializeField] private float numSecondsAtStart;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI fractionText;
    [SerializeField] private TextMeshProUGUI doubleOhSevenText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private VideoPlayer bloodDrip;
    [SerializeField] private VideoPlayer cards;
    [SerializeField] private float audioStartTime = 7 + 1.2f;
    [SerializeField] private float gameOverStartTime = 0;
    [SerializeField] private float pitchIncreaseTime = 17;
    [SerializeField] private float doubleOhSevenDisplayTime = 2f;
    [SerializeField] private float bloodDripDisplayTime;
    [SerializeField] private float titleDisplayTime;
    [SerializeField] private float cardsDisplayTime = -15;

    private float startTime;
    private int lastBeepSecond;
    private AudioSource audioSource;
    private bool isTimeVisible = true;
    private bool hasBloodDripPlayed= false;
    private RawImage bloodDripImage;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        startTime = Time.time;
        gameOverText.DOFade(0, 0);
        titleText.DOFade(0, 0);
        doubleOhSevenText.color = Color.clear;
        bloodDripImage = bloodDrip.GetComponent<RawImage>();
        cards.Prepare();
        
        ClearRenderTexture(bloodDrip.targetTexture);
        ClearRenderTexture(cards.targetTexture);

        gameOverText.outlineWidth = 0.1f;
        gameOverText.outlineColor = Color.black;
    }

    private void ClearRenderTexture(RenderTexture renderTexture)
    {
        RenderTexture rt = UnityEngine.RenderTexture.active;
        UnityEngine.RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        UnityEngine.RenderTexture.active = rt;
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
                audioSource.pitch = 1 + (pitchIncreaseTime - numSecondsRemaining) / pitchIncreaseTime;
            }
            audioSource.Play();
        }
        if (numSecondsRemaining < audioStartTime && !audioPlayer.isPlaying)
        {
            audioPlayer.Play();
        }

        if (numSecondsRemaining < gameOverStartTime && gameOverText.color.a == 0)
        {
            gameOverText.DOFade(1, 0);
        }

        if (numSecondsRemaining > doubleOhSevenDisplayTime && numSecondsRemaining < 7)
        {
            timeToDisplay = TimeSpan.FromSeconds(7);
        }
        if (numSecondsRemaining < doubleOhSevenDisplayTime)
        {
            const float timeFadeDuration = 1.0f;

            timeText.text = "0:00:07";
            fractionText.text = ".00";

            if (isTimeVisible)
            {
                doubleOhSevenText.color = Color.red;
                timeText.DOKill();
                timeText.DOFade(0, timeFadeDuration);
                fractionText.DOKill();
                fractionText.DOFade(0, timeFadeDuration);
                isTimeVisible = false;
            }
        }
        else
        {
            timeText.text = timeToDisplay.ToString(timeFormat);
            fractionText.text = timeToDisplay.ToString(fractionFormat);
        }

        if (numSecondsRemaining < titleDisplayTime && titleText.color.a == 0)
        {
            titleText.DOFade(1, 0);
            doubleOhSevenText.DOFade(0, 0);
        }

        if (numSecondsRemaining < bloodDripDisplayTime && !bloodDrip.isPlaying && !hasBloodDripPlayed)
        {
            hasBloodDripPlayed = true;
            bloodDrip.Play();
        }

        if (numSecondsRemaining < cardsDisplayTime && !cards.isPlaying)
        {
            cards.Play();
            bloodDripImage.DOFade(0, 5);
        }
    }
}
