using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteracaoNPC : MonoBehaviour
{
    public Dialogo dialogo; // asset criado
    private bool dentro = false;

    void Update()
    {
        if (dentro && Input.GetKeyDown(KeyCode.E))
        {
            DialogoManager.Instance.IniciarDialogo(dialogo);
        }

        if (dentro && Input.GetKeyDown(KeyCode.Space))
        {
            DialogoManager.Instance.ProximaLinha();
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
