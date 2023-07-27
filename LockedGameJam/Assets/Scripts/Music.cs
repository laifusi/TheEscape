using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    static Music instance;
    private AudioSource audioSource;
    [SerializeField] AudioClip loopSound;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(!audioSource.isPlaying)
        {
            audioSource.clip = loopSound;
            audioSource.loop = true;
            audioSource.Play();
            enabled = false;
        }
    }
}
