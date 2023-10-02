using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceReset : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterMovement character = collision.GetComponent<CharacterMovement>();
        if (character != null)
        {
            character.Die(0);
        }
    }
}
