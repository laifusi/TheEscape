using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] AudioClip lockedSound;

    private AudioSource audioSource;
    private Collider2D col2D;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Start()
    {
        CharacterMovement.OnDied += ResetDoor;

        audioSource = GetComponent<AudioSource>();
        col2D = GetComponent<Collider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var inventory = collision.collider.GetComponent<Inventory>();
        if(inventory != null)
        {
            if (inventory.HasKey())
            {
                //Open the door
                audioSource.Play();
                col2D.enabled = false;
                spriteRenderer.enabled = false;
            }
            else
            {
                animator?.SetTrigger("NoKey");
                audioSource.PlayOneShot(lockedSound);
            }
        }
    }

    private void ResetDoor()
    {
        col2D.enabled = true;
        spriteRenderer.enabled = true;
    }

    private void OnDestroy()
    {
        CharacterMovement.OnDied -= ResetDoor;
    }
}
