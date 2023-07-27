using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        var inventory = collision.GetComponent<Inventory>();

        if (inventory != null)
        {
            inventory.TakeKey(gameObject);
            GetComponent<AudioSource>().Play();
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            enabled = false;
        }
    }
}
