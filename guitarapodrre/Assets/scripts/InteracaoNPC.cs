using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteracaoNPC : MonoBehaviour
{
    public GameObject painelTexto;
    
    private bool podeInteragir = false;

    void Update()
    {
        if (podeInteragir && Input.GetKeyDown(KeyCode.E))
        {
            // Troca de cena
            SceneManager.LoadScene("ritmo");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("gordo"))
        {
            painelTexto.SetActive(true);
            podeInteragir = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("gordo"))
        {
            painelTexto.SetActive(false);
            podeInteragir = false;
        }
    }
}
