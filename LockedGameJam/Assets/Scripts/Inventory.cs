using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    bool key;
    GameObject keyObject;

    internal void TakeKey(GameObject keyObj)
    {
        key = true;
        keyObject = keyObj;
    }

    internal bool HasKey()
    {
        return key;
    }

    internal void ReturnKey()
    {
        if(keyObject != null)
        {
            keyObject.GetComponent<Collider2D>().enabled = true;
            keyObject.GetComponent<SpriteRenderer>().enabled = true;
            keyObject.GetComponent<Key>().enabled = true;
            keyObject = null;
            key = false;
        }
    }
}
