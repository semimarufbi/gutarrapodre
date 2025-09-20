using System.Collections;
using UnityEngine;
using TMPro;

public class ContagemRegressiva : MonoBehaviour
{
    public TextMeshProUGUI textoContagem;
    public string[] contagem = { "3", "2", "1", "Já!" };
    public float intervalo = 1f;

    public AudioSource musica;
    public gameManager gameManagerRef;

    private bool contagemAtiva = false;

    private void Start()
    {
        IniciarContagem();
    }

    public void IniciarContagem()
    {
        if (!contagemAtiva)
            StartCoroutine(ContagemCoroutine());
    }

    private IEnumerator ContagemCoroutine()
    {
        contagemAtiva = true;

        // Pausa o jogo e a música
        if (gameManagerRef != null)
            gameManagerRef.startPlaying = false;

        if (musica != null)
            musica.Pause();

        // Pausa o tempo do jogo
        Time.timeScale = 0f;

        // Mostra a contagem usando tempo real
        foreach (string numero in contagem)
        {
            if (textoContagem != null)
                textoContagem.text = numero;

            yield return new WaitForSecondsRealtime(intervalo); // ignora Time.timeScale
        }

        // Limpa texto
        if (textoContagem != null)
            textoContagem.text = "";

        // Libera o jogo e a música
        if (gameManagerRef != null)
            gameManagerRef.startPlaying = true;

        if (musica != null)
            musica.Play();

        // Agora sim retorna Time.timeScale = 1
        Time.timeScale = 1f;

        contagemAtiva = false;
    }
}
