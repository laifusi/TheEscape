using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuHighscoreText : MonoBehaviour
{
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void SetHighscore(int levelNumber)
    {
        text.SetText(PlayerPrefs.GetFloat("HighScore " + levelNumber).ToString("F2"));
    }
}
