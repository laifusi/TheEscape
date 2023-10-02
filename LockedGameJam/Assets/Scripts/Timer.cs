using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private TMP_Text text;
    private float initialTime;
    private float currentTime;
    private bool timerOff;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        ResetTimer();
        EscapeSensor.OnWin += StopTimer;
    }

    private void ResetTimer()
    {
        initialTime = Time.time;
        timerOff = false;
    }

    private void StopTimer()
    {
        timerOff = true;
    }

    private void Update()
    {
        if (timerOff)
            return;

        currentTime = Time.time - initialTime;
        text.SetText(currentTime.ToString("F2"));
    }
}
