using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{

    public AudioSource theMusic;
    public bool startPlaying;
    public beatScroller theBS; // arraste o objeto que tem beatScroller aqui
    public static gameManager instance;

    void Start()
    {
        instance = this;

        if (theBS == null)
        {
            Debug.LogError("BeatScroller não está referenciado no Inspector!");
        }
    }

    void Update()
    {
        if (!startPlaying && Input.anyKeyDown)
        {
            startPlaying = true;
            theBS.hasStarted = true;
            theMusic.Play();
        }
    }

    public void NoteHit()
    {
        Debug.Log("hit on time");
    }
    public void NoteMissed()
    {
        Debug.Log("Missed note");
    }
}
