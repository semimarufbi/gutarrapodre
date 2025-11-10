using UnityEngine;

public class InteracaoNPC : MonoBehaviour
{
    [Header("Referência ao diálogo")]
    public Dialogo dialogo; // asset criado no projeto

    private bool iniciouDialogo = false;

    void Start()
    {
        // Inicia o diálogo automaticamente ao carregar a cena
        if (dialogo != null)
        {
            DialogoManager.Instance.IniciarDialogo(dialogo);
            iniciouDialogo = true;
        }
    }

    void Update()
    {
        // Se o diálogo já começou, permite avançar com E ou botão do controle
        if (iniciouDialogo && DialogoManager.Instance.EmDialogo)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton2))
            {
                DialogoManager.Instance.ProximaLinha();
            }
        }
    }
}
