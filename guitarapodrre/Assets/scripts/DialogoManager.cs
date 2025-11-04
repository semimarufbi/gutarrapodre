using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogoManager : MonoBehaviour
{
    public static DialogoManager Instance;

    [Header("Prefabs de balão por personagem")]

    [SerializeField] public GameObject balaoPlayerPrefab;
    [SerializeField] public GameObject balaoNPCPrefab;

    [Header("Canvas Pai (Screen Space - Overlay)")]
    public Transform canvasPai;

    private GameObject balaoAtual;
    private TextMeshProUGUI textoUI;

    private Dialogo dialogoAtual;
    private int indice = 0;

    public bool EmDialogo { get; private set; } = false;
    private bool podeAvancar = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (canvasPai == null)
            Debug.LogWarning("[DialogoManager] canvasPai não está configurado no Inspector.");
    }

    void Update()
    {
        if (EmDialogo && podeAvancar && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton2)))
        {
            ProximaLinha();
        }
    }

    public void IniciarDialogo(Dialogo dialogo)
    {
        if (dialogo == null || dialogo.falas == null || dialogo.falas.Length == 0) return;

        dialogoAtual = dialogo;
        indice = 0;
        EmDialogo = true;

        CriarOuTrocarBalao();
        MostrarLinha();

        podeAvancar = false;
        Invoke(nameof(LiberarAvanco), 0.12f);
    }

    public void ProximaLinha()
    {
        if (!EmDialogo || dialogoAtual == null) return;

        indice++;

        if (indice < dialogoAtual.falas.Length)
        {
            CriarOuTrocarBalao();
            MostrarLinha();

            podeAvancar = false;
            Invoke(nameof(LiberarAvanco), 0.08f);
        }
        else
        {
            EncerrarDialogo();
        }
    }

    private void MostrarLinha()
    {
        if (textoUI == null || dialogoAtual == null || dialogoAtual.falas == null) return;
        if (indice < 0 || indice >= dialogoAtual.falas.Length) return;

        textoUI.text = $"{dialogoAtual.falas[indice].nome}: {dialogoAtual.falas[indice].texto}";
    }

    private void CriarOuTrocarBalao()
    {
        if (dialogoAtual == null || dialogoAtual.falas == null || dialogoAtual.falas.Length == 0) return;

        string nomeFalando = dialogoAtual.falas[indice].nome;
        GameObject prefabEscolhido = (nomeFalando == "Player") ? balaoPlayerPrefab : balaoNPCPrefab;

        if (prefabEscolhido == null)
        {
            Debug.LogError($"[DialogoManager] prefab do balão não configurado para '{nomeFalando}'.");
            return;
        }

        if (balaoAtual != null) Destroy(balaoAtual);

        balaoAtual = canvasPai != null
            ? Instantiate(prefabEscolhido, canvasPai)
            : Instantiate(prefabEscolhido);

        textoUI = balaoAtual.GetComponentInChildren<TextMeshProUGUI>();
        if (textoUI == null)
            Debug.LogError("[DialogoManager] TextMeshProUGUI não encontrado dentro do prefab do balão.");
    }

    private void EncerrarDialogo()
    {
        if (balaoAtual != null) Destroy(balaoAtual);

        dialogoAtual = null;
        indice = 0;
        EmDialogo = false;

        // Troca de cena automática: pega a cena atual e carrega a próxima da Build Settings
        int cenaAtualIndex = SceneManager.GetActiveScene().buildIndex;
        int totalCenas = SceneManager.sceneCountInBuildSettings;

        int proximaCenaIndex = (cenaAtualIndex + 1) % totalCenas; // loop caso seja a última
        SceneManager.LoadScene(proximaCenaIndex);
    }

    private void LiberarAvanco()
    {
        podeAvancar = true;
    }
}
