using UnityEngine;
using TMPro;

public class DialogoManager : MonoBehaviour
{
    public static DialogoManager Instance;

    [Header("Prefabs de balão por personagem")]
    public GameObject balaoPlayerPrefab;
    public GameObject balaoNPCPrefab;

    [Header("Canvas Pai")]
    public Transform canvasPai; // Canvas em Screen Space - Overlay

    private GameObject balaoAtual;
    private TextMeshProUGUI textoUI;

    private Dialogo dialogoAtual;
    private int indice = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Inicia o diálogo.
    /// </summary>
    public void IniciarDialogo(Dialogo dialogo)
    {
        if (dialogo == null) return;

        dialogoAtual = dialogo;
        indice = 0;

        CriarOuTrocarBalao();

        MostrarLinha();
    }

    /// <summary>
    /// Avança para a próxima linha.
    /// </summary>
    public void ProximaLinha()
    {
        if (dialogoAtual == null) return;

        indice++;

        if (indice < dialogoAtual.falas.Length)
        {
            CriarOuTrocarBalao();
            MostrarLinha();
        }
        else
        {
            EncerrarDialogo();
        }
    }

    /// <summary>
    /// Mostra o texto da linha atual.
    /// </summary>
    private void MostrarLinha()
    {
        if (textoUI != null && dialogoAtual != null)
        {
            textoUI.text = $"{dialogoAtual.falas[indice].nome}: {dialogoAtual.falas[indice].texto}";
        }
    }

    /// <summary>
    /// Cria um novo balão ou troca se o personagem mudar.
    /// </summary>
    private void CriarOuTrocarBalao()
    {
        string nomeFalando = dialogoAtual.falas[indice].nome;

        GameObject prefabEscolhido = nomeFalando == "Player" ? balaoPlayerPrefab : balaoNPCPrefab;

        // Se não existe balão ou mudou de personagem, cria novo
        if (balaoAtual == null || balaoAtual.name != prefabEscolhido.name + "(Clone)")
        {
            if (balaoAtual != null) Destroy(balaoAtual);

            balaoAtual = Instantiate(prefabEscolhido, canvasPai);
            textoUI = balaoAtual.GetComponentInChildren<TextMeshProUGUI>();

            if (textoUI == null)
                Debug.LogError("Texto TextMeshProUGUI não encontrado no prefab do balão!");
        }
    }

    /// <summary>
    /// Encerra o diálogo e destrói o balão.
    /// </summary>
    private void EncerrarDialogo()
    {
        if (balaoAtual != null)
        {
            Destroy(balaoAtual);
        }

        dialogoAtual = null;
        indice = 0;
    }
}
