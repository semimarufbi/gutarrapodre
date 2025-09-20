using UnityEngine;

public class InteracaoNPC : MonoBehaviour
{
    public Dialogo dialogo; // asset criado
    private bool dentro = false;

    void Update()
    {
        // Só inicia diálogo quando estiver perto e não houver diálogo ativo
        if ((dentro && Input.GetKeyDown(KeyCode.E)) || dentro && Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            if (!DialogoManager.Instance.EmDialogo)
            {
                DialogoManager.Instance.IniciarDialogo(dialogo);
            }
            // não chamamos ProximaLinha() aqui — o DialogoManager já escuta E globalmente enquanto EmDialogo == true
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            dentro = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            dentro = false;
    }
}
