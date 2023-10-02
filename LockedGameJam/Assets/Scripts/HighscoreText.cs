using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighscoreText : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    [SerializeField] Animator newHighscoreAnimator;

    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        SetHighscore();

        EscapeSensor.OnNewHighscore += NewHighscore;
    }

    private void SetHighscore()
    {
        float highscore = PlayerPrefs.GetFloat("HighScore " + levelManager.GetCurrentLevel());
        if (highscore != 0)
            text.SetText(highscore.ToString("F2"));
        else
            text.SetText("---");
    }

    private void NewHighscore()
    {
        SetHighscore();
        newHighscoreAnimator.SetTrigger("NewHighscore");
    }

    private void OnDestroy()
    {
        EscapeSensor.OnNewHighscore -= NewHighscore;
    }
}
