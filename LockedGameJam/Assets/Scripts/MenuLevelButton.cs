using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuLevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] MenuHighscoreText highscoreText;
    [SerializeField] GameObject highscorePanel;

    public int LevelNumber { get; set; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        highscorePanel.SetActive(true);
        highscoreText.SetHighscore(LevelNumber);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highscorePanel.SetActive(false);
    }
}
