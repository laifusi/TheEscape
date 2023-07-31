using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CamTimer : MonoBehaviour
{
    private TMP_Text timerText;

    private void Start()
    {
        timerText = GetComponent<TMP_Text>();
        CleanTimer();

        transform.rotation = Quaternion.identity;

        CharacterMovement.OnDied += CleanTimer;
    }

    public void UpdateTimer(float time)
    {
        timerText.SetText(time.ToString("F1"));
        if(time < 0)
        {
            CleanTimer();
        }
    }

    private void CleanTimer()
    {
        timerText.SetText("");
    }

    private void OnDestroy()
    {
        CharacterMovement.OnDied -= CleanTimer;
    }
}
