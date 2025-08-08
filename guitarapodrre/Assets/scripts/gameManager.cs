using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    public bool startPlaying = false;
    public AudioSource theMusic; // 🎵

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        // Exemplo: quando o jogador aperta espaço, começa
        if (!startPlaying && Input.GetKeyDown(KeyCode.Space))
        {
            startPlaying = true;
            theMusic.Play();
        }
    }
}
