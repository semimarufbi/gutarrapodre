using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SuperManager : MonoBehaviour
{
    public static SuperManager Instance;

    [Header("Referências")]
    [SerializeField] public ParticleSystem superParticles;
    [SerializeField] public Image uiFill;           // Image Type = Filled (configurar no Inspector)
    [SerializeField] public NoteSpawner noteSpawner; // opcional, se quiser spawnar notas especiais
    [Header("Config")]
    [SerializeField] public int maxCharge = 10;
    [SerializeField] public KeyCode activationKey = KeyCode.Space; // tecla que ativa o super
    [SerializeField] public int scoreMultiplierOnSuper = 2;
    [SerializeField] public float superDuration = 5f; 


  public  int currentCharge = 0;
    public bool isSuperActive = false;

    private void Awake()
    {
        if (Instance == null) { Instance = this; /*DontDestroyOnLoad(gameObject);*/ }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        UpdateUI();
        if (superParticles != null) superParticles.Stop();
    }

    private void Update()
    {
        // Ativação por tecla (configurável pelo inspector)
        if (!isSuperActive && currentCharge >= maxCharge && Input.GetKeyDown(activationKey))
        {
            TryActivateSuper();
        }
    }

    /// <summary>
    /// Adiciona carga ao super (chame AddCharge(1) ao acertar uma nota especial).
    /// Não ativa automaticamente quando cheio — apenas enche a barra.
    /// </summary>
    public void AddCharge(int amount)
    {
        if (isSuperActive) return;

        currentCharge = Mathf.Clamp(currentCharge + Mathf.Max(0, amount), 0, maxCharge);
        UpdateUI();
        if (currentCharge >= maxCharge)
        {
            Debug.Log("[SuperManager] Barra cheia! Pressione " + activationKey + " para ativar.");
            // opcional: acionar algum efeito visual para indicar que está pronto
        }
    }

    /// <summary>
    /// Tenta ativar o super (pode ser chamado por botão UI).
    /// </summary>
    public void TryActivateSuper()
    {
        if (isSuperActive) return;
        if (currentCharge < maxCharge)
        {
            Debug.Log("[SuperManager] Barra não cheia ainda.");
            return;
        }

        ActivateSuper();
    }

    private void ActivateSuper()
    {
        isSuperActive = true;
        currentCharge = 0;
        UpdateUI();

        Debug.Log("[SuperManager] Super ativado!");

        if (superParticles != null) superParticles.Play();

        // Aplica multiplicador no gameManager (se existir)
        if (gameManager.instance != null)
        {
            // Presume que gameManager tem SetMultiplier(int mult, float duration)
            gameManager.instance.SetMultiplier(scoreMultiplierOnSuper, superDuration);
        }

        // opcional: se quiser spawnar notas especiais enquanto o super estiver ativo,
        // chame algo como: noteSpawner.MarkNextNotesAsSuper(...)

        StartCoroutine(EndSuperAfter(superDuration));
    }

    private IEnumerator EndSuperAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        isSuperActive = false;

        if (superParticles != null) superParticles.Stop();
        Debug.Log("[SuperManager] Super finalizado.");
    }

    private void UpdateUI()
    {
        if (uiFill != null)
        {
            uiFill.fillAmount = (float)currentCharge / (float)maxCharge;
        }
    }

    // API utilitária (por exemplo, para UI button)
    public bool IsSuperReady() => currentCharge >= maxCharge;
    public bool IsSuperActive() => isSuperActive;
    public int GetCurrentCharge() => currentCharge;
    public int GetMaxCharge() => maxCharge;
}
