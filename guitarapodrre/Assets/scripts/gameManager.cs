using UnityEngine;
using TMPro;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("Áudio e gameplay")]
    public bool startPlaying = false;
    public AudioSource theMusic;

    [Header("Score UI")]
    public TextMeshProUGUI textoScore;
    public TextMeshProUGUI textoErros;

    private int score = 0;
    private int misses = 0;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        startPlaying = true;
        if (theMusic != null) theMusic.Play();
        AtualizarUI();
    }

    public void NoteHit()
    {
        score += 100; // cada acerto vale 100 pontos
        AtualizarUI();
        Debug.Log("Acertou! Pontos: " + score);
    }

    public void NoteMissed()
    {
        misses += 1; // cada erro +1
        AtualizarUI();
        Debug.Log("Errou! Total de erros: " + misses);
    }

    private void AtualizarUI()
    {
        if (textoScore != null)
            textoScore.text = $"Score: {score}";

        if (textoErros != null)
            textoErros.text = $"Erros: {misses}";
    }
}
