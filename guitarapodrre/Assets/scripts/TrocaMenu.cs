using UnityEngine;
using UnityEngine.SceneManagement;

public class TrocaDeCena : MonoBehaviour
{
    public void CarregarCena(string nomeCena)
    {
        SceneManager.LoadScene(nomeCena);
    }

    public void SairDoJogo()
    {
        Application.Quit();
        Debug.Log("Jogo encerrado."); // Só aparece no editor
    }
}
