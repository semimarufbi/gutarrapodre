using UnityEngine;
using UnityEngine.SceneManagement;

public class ritmotroca : MonoBehaviour
{
    public void CarregaCena(string nomeCena)
    {
        SceneManager.LoadScene(nomeCena);
    }

    public void SaiDoJogo()
    {
        Application.Quit();
        Debug.Log("Jogo encerrado."); // Só aparece no editor
    }
}
