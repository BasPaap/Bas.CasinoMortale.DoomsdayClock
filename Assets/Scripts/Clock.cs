using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource))]
public class Clock : MonoBehaviour
{
    [SerializeField] private float numSecondsAtStart;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI fractionText;
    [SerializeField] private TextMeshProUGUI doubleOhSevenText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private AudioSource musicAudioSource;
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
    private AudioSource beepAudioSource;
    private bool isTimeVisible = true;
    private bool hasBloodDripPlayed = false;
    private bool hasMusicPlayed = false;
    private RawImage bloodDripImage;
    private RawImage cardsImage;

    private void Awake()
    {
        beepAudioSource = GetComponent<AudioSource>();
        startTime = Time.time;

        gameOverText.DOKill();
        gameOverText.DOFade(0, 0);

        titleText.DOKill();
        titleText.DOFade(0, 0);

        doubleOhSevenText.color = Color.clear;
        bloodDripImage = bloodDrip.GetComponent<RawImage>();
        
        cardsImage = cards.GetComponent<RawImage>();
        cardsImage.DOKill();
        cardsImage.DOFade(0, 0);
        
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
            PlayBeep(numSecondsRemaining);
        }

        if (numSecondsRemaining < audioStartTime && !musicAudioSource.isPlaying && !hasMusicPlayed)
        {
            PlayMusic();
        }

        if (numSecondsRemaining < gameOverStartTime && gameOverText.color.a == 0)
        {
            ShowGameOverText();
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
                FadeTimeOut(timeFadeDuration);
            }
        }
        else
        {
            UpdateText(timeToDisplay, timeFormat, fractionFormat);
        }

        if (numSecondsRemaining < titleDisplayTime && titleText.color.a == 0 && !hasBloodDripPlayed)
        {
            ShowTitleText();
        }

        if (numSecondsRemaining < bloodDripDisplayTime && !bloodDrip.isPlaying && !hasBloodDripPlayed)
        {
            PlayBloodDrip();
        }

        if (numSecondsRemaining < cardsDisplayTime && !cards.isPlaying)
        {
            PlayCards();
        }
    }

    private void PlayCards()
    {
        cardsImage.DOKill();
        cardsImage.DOFade(1, 2);
        cards.Play();
        
        bloodDrip.DOKill();
        bloodDripImage.DOFade(0, 5);

        titleText.DOKill();
        titleText.DOFade(0, 2);
    }

    private void PlayBloodDrip()
    {
        hasBloodDripPlayed = true;
        bloodDrip.Play();
    }

    private void ShowTitleText()
    {
        titleText.DOFade(1, 0);
        doubleOhSevenText.DOFade(0, 0);
    }

    private void UpdateText(TimeSpan timeToDisplay, string timeFormat, string fractionFormat)
    {
        timeText.text = timeToDisplay.ToString(timeFormat);
        fractionText.text = timeToDisplay.ToString(fractionFormat);
    }

    private void FadeTimeOut(float timeFadeDuration)
    {
        doubleOhSevenText.color = Color.red;
        timeText.DOKill();
        timeText.DOFade(0, timeFadeDuration);
        fractionText.DOKill();
        fractionText.DOFade(0, timeFadeDuration);
        isTimeVisible = false;
    }

    private void ShowGameOverText()
    {
        gameOverText.DOKill();
        gameOverText.DOFade(1, 0);
    }

    private void PlayMusic()
    {
        musicAudioSource.Play();
        hasMusicPlayed = true;
    }

    private void PlayBeep(float numSecondsRemaining)
    {
        lastBeepSecond = (int)numSecondsRemaining;
        if (numSecondsRemaining < pitchIncreaseTime)
        {
            beepAudioSource.pitch = 1 + (pitchIncreaseTime - numSecondsRemaining) / pitchIncreaseTime;
        }
        beepAudioSource.Play();
    }
}
