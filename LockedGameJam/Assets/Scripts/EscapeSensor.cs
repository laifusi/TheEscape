using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeSensor : MonoBehaviour
{
    [SerializeField] GameObject winCanvas;

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
            winCanvas.SetActive(true);
            character.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            character.enabled = false;
            GetComponent<AudioSource>().Play();
            OnWin?.Invoke();
            Time.timeScale = 0;
        }
    }
}
