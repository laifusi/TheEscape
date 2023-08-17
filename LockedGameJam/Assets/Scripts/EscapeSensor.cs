using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeSensor : MonoBehaviour
{
    [SerializeField] GameObject winCanvas;
    [SerializeField] LevelManager levelManager;
    [SerializeField] bool lastLevel = false;
    [SerializeField] bool minigameLevel = false;

    public static Action OnWin;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.GetComponent<CharacterMovement>();
        if (character != null)
        {
            PlayerPrefs.SetInt("Level " + SceneManager.GetActiveScene().buildIndex, 1);
            if(lastLevel || minigameLevel)
                winCanvas.SetActive(true);
            Rigidbody2D charRB = character.GetComponent<Rigidbody2D>();
            charRB.velocity = Vector2.zero;
            character.enabled = false;
            GetComponent<AudioSource>().Play();
            OnWin?.Invoke();
            Time.timeScale = 0;
            if(!lastLevel && !minigameLevel)
                levelManager.NextLevel();
        }
    }
}
