using System;
using System.Collections;
using UnityEngine;

public class EscapeSensor : MonoBehaviour
{
    [SerializeField] GameObject winCanvas;
    [SerializeField] LevelManager levelManager;
    [SerializeField] bool lastLevel = false;
    [SerializeField] bool minigameLevel = false;

    public static Action OnWin;
    public static Action OnNewHighscore;

    private float initialTime;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    private void Start()
    {
        initialTime = Time.time;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.GetComponent<CharacterMovement>();
        if (character != null)
        {
            float timeToWait = 0;

            PlayerPrefs.SetInt("Level " + levelManager.GetCurrentLevel(), 1);

            float currentHighscore = PlayerPrefs.GetFloat("HighScore " + levelManager.GetCurrentLevel());
            if (Time.time - initialTime < currentHighscore || currentHighscore == 0)
            {
                PlayerPrefs.SetFloat("HighScore " + levelManager.GetCurrentLevel(), Mathf.Round((Time.time - initialTime) * 100f) / 100f);
                OnNewHighscore?.Invoke();
                timeToWait = 1.2f;
            }

            StartCoroutine(EndLevel(character, timeToWait));
        }
    }

    private IEnumerator EndLevel(CharacterMovement character, float timeToWait)
    {
        Rigidbody2D charRB = character.GetComponent<Rigidbody2D>();
        charRB.velocity = Vector2.zero;
        character.enabled = false;

        GetComponent<AudioSource>().Play();
        OnWin?.Invoke();

        yield return new WaitForSeconds(timeToWait);

        if (!lastLevel && !minigameLevel)
            levelManager.NextLevel();

        if (lastLevel || minigameLevel)
            winCanvas.SetActive(true);

    }
}
