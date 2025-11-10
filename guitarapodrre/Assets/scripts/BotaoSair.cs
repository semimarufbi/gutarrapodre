using UnityEngine;

public class BotaoSair : MonoBehaviour
{
    public void SairDoJogo()
    {
        Debug.Log("Saindo do jogo...");

#if UNITY_EDITOR
        // Para o modo Play se estiver testando no Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Fecha o aplicativo compilado (Windows, Android, etc.)
        Application.Quit();
#endif
    }
}
