using UnityEngine;
using UnityEngine.SceneManagement;

public class somConfig : MonoBehaviour
{
    public void CarregarSom()
    {
        AudioListener.volume = 1f; // Liga o som
        Debug.Log("Som ativado");
    }

    public void Removersom()
    {
        AudioListener.volume = 0f; // Desliga o som
        Debug.Log("Som desativado");
    }
}
