using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // 🔹 Necessário para trocar de cena

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

    [Header("Cena de Fim")]
    [SerializeField] private string proximaCena = "TelaFinal"; // 🔹 Nome da cena a carregar
    private bool musicaTerminou = false; // 🔹 Evita chamar a troca mais de uma vez

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
        // 🔹 Checa se o super está ativo
        if (superAtivo)
        {
            superTimer -= Time.deltaTime;
            if (superTimer <= 0)
                DesativarSuper();
        }

        // 🔹 Checa se a música terminou
        if (theMusic != null && startPlaying && !theMusic.isPlaying && !musicaTerminou)
        {
            musicaTerminou = true;
            TrocarCenaQuandoMusicaAcabar();
        }
    }

    private void TrocarCenaQuandoMusicaAcabar()
    {
        Debug.Log("Música terminou! Carregando próxima cena...");
        SceneManager.LoadScene(proximaCena);
    }

    public void NoteHit(bool especial = false)
    {
        score += 100 * currentMultiplier;

        if (especial && !superAtivo)
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
        superProgress = 0;
        currentMultiplier = 2;

        if (superParticulas != null)
            superParticulas.Play();
        else
            Debug.LogWarning("superParticulas is not assigned in gameManager!");

        Debug.Log("SUPER ATIVADO!");
    }

    private void DesativarSuper()
    {
        superAtivo = false;
        currentMultiplier = 1;

        if (superParticulas != null)
            superParticulas.Stop();
        else
            Debug.LogWarning("superParticulas is not assigned in gameManager!");

        Debug.Log("SUPER DESATIVADO!");
    }

    public void SetMultiplier(int multiplier, float duration)
    {
        superAtivo = true;
        superTimer = duration;
        superProgress = 0;
        currentMultiplier = multiplier;

        if (superParticulas != null)
            superParticulas.Play();
        else
            Debug.LogWarning("superParticulas is not assigned in gameManager!");

        Debug.Log($"SUPER ATIVADO com multiplicador {multiplier} por {duration} segundos!");
    }
}
