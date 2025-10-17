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

    [Header("Configurações")]
    [SerializeField] public int maxCharge = 10;
    [SerializeField] public KeyCode activationKey = KeyCode.Space; // tecla que ativa o super
    [SerializeField] public int scoreMultiplierOnSuper = 2;
    [SerializeField] public float superDuration = 5f;

    public int currentCharge = 0;
    public bool isSuperActive = false;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
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

    // Adiciona carga ao super (chame AddCharge(1) ao acertar nota especial)
    public void AddCharge(int amount)
    {
        if (isSuperActive) return;

        currentCharge = Mathf.Clamp(currentCharge + Mathf.Max(0, amount), 0, maxCharge);
        UpdateUI();

        if (currentCharge >= maxCharge)
        {
            Debug.Log("[SuperManager] Barra cheia! Pressione " + activationKey + " para ativar.");
            StartCoroutine(PulseBar()); // animação de pulso quando enche
        }
    }

    // Tenta ativar o super
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
        StartCoroutine(FlashBar()); // flash visual ao ativar

        if (superParticles != null) superParticles.Play();

        if (gameManager.instance != null)
        {
            gameManager.instance.SetMultiplier(scoreMultiplierOnSuper, superDuration);
        }

        StartCoroutine(EndSuperAfter(superDuration));
    }

    private IEnumerator EndSuperAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        isSuperActive = false;

        if (superParticles != null) superParticles.Stop();
        Debug.Log("[SuperManager] Super finalizado.");
    }

    // Atualiza a UI e aplica o gradiente dinâmico
    private void UpdateUI()
    {
        if (uiFill != null)
        {
            float t = (float)currentCharge / (float)maxCharge;
            uiFill.fillAmount = t;

            // Gradiente azul → roxo → rosa
            Color startColor = new Color(0f, 0.8f, 1f); // azul neon
            Color midColor = new Color(0.6f, 0f, 1f);   // roxo
            Color endColor = new Color(1f, 0f, 1f);     // rosa forte

            if (t < 0.5f)
                uiFill.color = Color.Lerp(startColor, midColor, t * 2f);
            else
                uiFill.color = Color.Lerp(midColor, endColor, (t - 0.5f) * 2f);
        }
    }

    // Pulso visual quando a barra enche
    private IEnumerator PulseBar()
    {
        float time = 0f;
        while (time < 0.5f)
        {
            time += Time.deltaTime;
            float scale = 1f + Mathf.Sin(time * 20f) * 0.1f;
            uiFill.transform.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }
        uiFill.transform.localScale = Vector3.one;
    }

    // Flash branco rápido quando o super é ativado
    private IEnumerator FlashBar()
    {
        Color original = uiFill.color;
        uiFill.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        uiFill.color = original;
    }

    // Getters utilitários
    public bool IsSuperReady() => currentCharge >= maxCharge;
    public bool IsSuperActive() => isSuperActive;
    public int GetCurrentCharge() => currentCharge;
    public int GetMaxCharge() => maxCharge;
}
