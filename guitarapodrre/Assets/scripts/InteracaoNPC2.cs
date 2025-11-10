using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteracaoNPC2 : MonoBehaviour
{
    public Dialogo dialogo; // asset do diálogo final

    void Start()
    {
        // Espera um pequeno tempo antes de começar (garante que o DialogoManager já está na cena)
        Invoke(nameof(IniciarDialogoFinal), 0.2f);
    }

    void IniciarDialogoFinal()
    {
        if (DialogoManager.Instance != null && !DialogoManager.Instance.EmDialogo)
        {
            DialogoManager.Instance.IniciarDialogo(dialogo);
        }
        else
        {
            Debug.LogWarning("DialogoManager não encontrado ou diálogo já ativo!");
        }
    }
}
