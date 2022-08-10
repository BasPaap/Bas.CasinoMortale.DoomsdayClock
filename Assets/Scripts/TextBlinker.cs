using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextBlinker : MonoBehaviour
{
    [SerializeField] private float blinkOffDuration = 0.5f;
    [SerializeField] private float blinkOnDuration = 1f;

    private TextMeshProUGUI textMeshProUGUI;
    private float timer;

    private void Awake()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        timer = timer + Time.deltaTime;
        if (timer >= blinkOffDuration)
        {
            textMeshProUGUI.enabled = true;
        }
        if (timer >= blinkOffDuration + blinkOnDuration)
        {
            textMeshProUGUI.enabled = false;
            timer = 0;
        }
    }

}
