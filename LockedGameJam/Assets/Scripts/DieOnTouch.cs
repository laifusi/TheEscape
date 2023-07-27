using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieOnTouch : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.GetComponent<CharacterMovement>();
        if (character != null)
        {
            var audioSource = GetComponent<AudioSource>();
            if(audioSource != null)
            {
                audioSource.Play();
            }
            character.Die();
        }
    }
}
