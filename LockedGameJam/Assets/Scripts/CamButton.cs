using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamButton : MonoBehaviour
{
    [SerializeField] GameObject[] cameraVisions;
    [SerializeField] AudioClip press;
    [SerializeField] float totalOffTime;
    [SerializeField] float extraTimeForAndroid = 0.1f;
    [SerializeField] Sprite notPressed;
    [SerializeField] Sprite pressed;
    [SerializeField] CamButton[] otherCamButtons;

    bool cameraOff;
    float cameraTimer;
    SpriteRenderer spriteRenderer;
    AudioSource audioSource;
    List<CamTimer> cameraTimers = new List<CamTimer>();

    public Action<bool> OnCameraButton;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        CharacterMovement.OnDied += ResetCameras;
        EscapeSensor.OnWin += DeactivateCamera;

        if (otherCamButtons == null)
            return;

        foreach(CamButton camButton in otherCamButtons)
        {
            camButton.OnCameraButton += OtherButtonPressed;
        }

        foreach(GameObject camera in cameraVisions)
        {
            cameraTimers.AddRange(camera.transform.parent.GetComponentsInChildren<CamTimer>());
        }

        #if UNITY_ANDROID
        totalOffTime += extraTimeForAndroid;
        #endif
    }

    private void OtherButtonPressed(bool camOff)
    {
        cameraTimer = 0;
        cameraOff = camOff;
    }

    private void TurnCameraOff()
    {
        audioSource.Stop();
        foreach (GameObject cameraVision in cameraVisions)
        {
            cameraVision.SetActive(false);
        }
    }

    private void ResetCameras()
    {
        foreach (GameObject cameraVision in cameraVisions)
            cameraVision.SetActive(true);
        cameraOff = false;
        cameraTimer = 0;
        audioSource.Stop();
        spriteRenderer.sprite = notPressed;
    }

    void Update()
    {
        if(cameraOff)
        {
            cameraTimer += Time.deltaTime;
            if(cameraTimer >= totalOffTime)
            {
                cameraOff = false;
                if(cameraVisions != null)
                    foreach(GameObject cameraVision in cameraVisions)
                        cameraVision.SetActive(true);
                spriteRenderer.sprite = notPressed;
                audioSource.Stop();
            }

            foreach (CamTimer timer in cameraTimers)
            {
                timer.UpdateTimer(totalOffTime - cameraTimer);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.GetComponent<CharacterMovement>();
        if (character != null)
        {
            spriteRenderer.sprite = pressed;
            foreach(GameObject cameraVision in cameraVisions)
                cameraVision.SetActive(false);
            audioSource.Stop();
            cameraOff = false;
            audioSource.PlayOneShot(press);
            cameraTimer = 0;

            OnCameraButton?.Invoke(cameraOff);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        var character = collision.GetComponent<CharacterMovement>();
        if(character != null)
        {
            cameraOff = true;
            cameraTimer = 0;
            audioSource.Play();

            OnCameraButton?.Invoke(cameraOff);
        }
    }

    private void DeactivateCamera()
    {
        audioSource.Stop();
        foreach (GameObject cameraVision in cameraVisions)
        {
            Destroy(cameraVision);
        }
        cameraVisions = null;
    }

    private void OnDestroy()
    {
        CharacterMovement.OnDied -= ResetCameras;
        EscapeSensor.OnWin -= DeactivateCamera;

        if (otherCamButtons == null)
            return;

        foreach (CamButton camButton in otherCamButtons)
        {
            camButton.OnCameraButton -= OtherButtonPressed;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach(GameObject camVis in cameraVisions)
            Gizmos.DrawLine(transform.position, camVis.transform.position);
    }
#endif
}
