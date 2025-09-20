using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject painelPause;
    public gameManager gameManagerRef;
    public AudioSource musica;
    public ContagemRegressiva contagemRef;

    private bool jogoPausado = false;

    void Update()
    {
        // Escape do teclado
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        // Start do controle (geralmente JoystickButton7)
        if (Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        if (jogoPausado)
            RetomarJogo();
        else
            PausarJogo();
    }

    public void PausarJogo()
    {
        jogoPausado = true;

        if (painelPause != null)
            painelPause.SetActive(true);

        if (musica != null)
            musica.Pause();

        if (gameManagerRef != null)
            gameManagerRef.startPlaying = false;

        Time.timeScale = 0f;
    }

    public void RetomarJogo()
    {
        jogoPausado = false;

        if (painelPause != null)
            painelPause.SetActive(false);

        // Ao sair do pause, chama a contagem que só vai liberar o Time.timeScale = 1 no final
        if (contagemRef != null)
            contagemRef.IniciarContagem();
    }
}
