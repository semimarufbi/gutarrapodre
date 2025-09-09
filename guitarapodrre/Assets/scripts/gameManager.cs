using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    public bool startPlaying = false;
    public AudioSource theMusic;
    private int score = 0;
    private int misses = 0;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Assim que a cena começa, já inicia o jogo
        startPlaying = true;
        theMusic.Play();
    }

    public void NoteHit()
    {
        score++;
        Debug.Log($"Acertou! Pontos: {score}");
    }

    public void NoteMissed()
    {
        misses++;
        Debug.Log($"Errou! Erros: {misses}");
    }
}
