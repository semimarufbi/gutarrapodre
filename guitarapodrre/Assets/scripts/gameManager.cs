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
    private int currentMultiplier = 1; // Multiplicador atual

    [Header("Super System")]
    public int superProgress = 0;       // Progresso atual
    public int superMax = 10;           // Quantas notas especiais precisa
    public bool superAtivo = false;
    public float superDuracao = 5f;     // Duração do super em segundos
    private float superTimer = 0f;
    [SerializeField] public SuperManager SuperManager;

    public ParticleSystem superParticulas; // Partículas do super

    void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        startPlaying = true;
        if (theMusic != null)
            theMusic.Play();
        else
            Debug.LogWarning("theMusic is not assigned in gameManager!");

        AtualizarUI();
    }

    void Update()
    {
        if (superAtivo)
        {
            superTimer -= Time.deltaTime;
            if (superTimer <= 0)
            {
                DesativarSuper();
            }
        }
    }

    public void NoteHit(bool especial = false)
    {
        score += 100 * currentMultiplier; // Aplica o multiplicador

        if (especial && !superAtivo) // Só acumula progresso se super não está ativo
        {
            SuperManager.AddCharge(1);
            superProgress++;
            if (superProgress >= superMax)
            {
                //AtivarSuper();
            }
        }

        AtualizarUI();
        Debug.Log("Acertou! Pontos: " + score);
    }

    public void NoteMissed()
    {
        misses += 1;
        AtualizarUI();
        Debug.Log("Errou! Total de erros: " + misses);
    }

    private void AtualizarUI()
    {
        if (textoScore != null)
            textoScore.text = $"Score: {score}";
        else
            Debug.LogWarning("textoScore is not assigned in gameManager!");

        if (textoErros != null)
            textoErros.text = $"Erros: {misses}";
        else
            Debug.LogWarning("textoErros is not assigned in gameManager!");
    }

    private void AtivarSuper()
    {
        superAtivo = true;
        superTimer = superDuracao;
        superProgress = 0; // Reseta a barra
        currentMultiplier = 2; // Define multiplicador padrão (pode ser sobrescrito por SetMultiplier)

        if (superParticulas != null)
            superParticulas.Play();
        else
            Debug.LogWarning("superParticulas is not assigned in gameManager!");

        Debug.Log("SUPER ATIVADO!");
    }

    private void DesativarSuper()
    {
        superAtivo = false;
        currentMultiplier = 1; // Reseta o multiplicador

        if (superParticulas != null)
            superParticulas.Stop();
        else
            Debug.LogWarning("superParticulas is not assigned in gameManager!");

        Debug.Log("SUPER DESATIVADO!");
    }

    // Método chamado por SuperManager para ativar super com multiplicador
    public void SetMultiplier(int multiplier, float duration)
    {
        superAtivo = true;
        superTimer = duration;
        superProgress = 0; // Reseta a barra
        currentMultiplier = multiplier;

        if (superParticulas != null)
            superParticulas.Play();
        else
            Debug.LogWarning("superParticulas is not assigned in gameManager!");

        Debug.Log($"SUPER ATIVADO com multiplicador {multiplier} por {duration} segundos!");
    }
}