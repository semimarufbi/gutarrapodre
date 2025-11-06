using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;

public class TrocaConfig : MonoBehaviour
{
    public void CarregarCenas(string nomeCena)
    {
        SceneManager.LoadScene(nomeCena);
    }

    public void SairDoJogo()
    {
        Debug.Log("Encerrando...");

#if UNITY_EDITOR
        EditorApplication.isPlaying = false; 
#else
        Application.Quit();
#endif
    }
}
