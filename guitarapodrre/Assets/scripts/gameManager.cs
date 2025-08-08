using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    public bool startPlaying = false;
    public AudioSource theMusic; // áudio da música

    private int score = 0;
    private int missed = 0;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        // Começa a tocar e o jogo quando aperta espaço
        if (!startPlaying && Input.GetKeyDown(KeyCode.Space))
        {
            startPlaying = true;
            theMusic.Play();
            Debug.Log("Jogo iniciado!");
        }
    }

    // Chamar quando a nota for acertada
    public void NoteHit()
    {
        score++;
        Debug.Log($"Nota acertada! Pontos: {score}");
    }

    // Chamar quando a nota for perdida
    public void NoteMissed()
    {
        missed++;
        Debug.Log($"Nota perdida! Total perdido: {missed}");
    }

    // Se quiser, métodos para pegar a pontuação atual, reiniciar, etc
    public int GetScore()
    {
        return score;
    }

    public int GetMissed()
    {
        return missed;
    }
}
