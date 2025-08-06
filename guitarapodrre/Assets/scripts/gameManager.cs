using UnityEngine;

public class gameManager : MonoBehaviour
{
    public AudioSource theMusic;
    public bool startPlaying;
    public static gameManager instance;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (!startPlaying && Input.anyKeyDown)
        {
            startPlaying = true;
            theMusic.Play();
        }
    }

    public void NoteHit()
    {
        Debug.Log("Acertou!");
    }

    public void NoteMissed()
    {
        Debug.Log("Errou!");
    }
}
