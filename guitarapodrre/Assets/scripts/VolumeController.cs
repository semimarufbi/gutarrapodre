using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [Header("Referências")]
    public AudioSource musicaSource;   // áudio da música de fundo
    public Slider musicaSlider;        // slider da música

    public Slider efeitosSlider;       // slider dos efeitos

    public static float volumeMusica = 1f;
    public static float volumeEfeitos = 1f;

    void Start()
    {
        // Carrega volumes salvos
        volumeMusica = PlayerPrefs.GetFloat("VolumeMusica", 1f);
        volumeEfeitos = PlayerPrefs.GetFloat("VolumeEfeitos", 1f);

        // Inicializa sliders
        if (musicaSlider != null)
        {
            musicaSlider.value = volumeMusica;
            musicaSlider.onValueChanged.AddListener(SetVolumeMusica);
        }

        if (efeitosSlider != null)
        {
            efeitosSlider.value = volumeEfeitos;
            efeitosSlider.onValueChanged.AddListener(SetVolumeEfeitos);
        }

        // Aplica volume inicial
        if (musicaSource != null)
            musicaSource.volume = volumeMusica;
    }

    public void SetVolumeMusica(float valor)
    {
        volumeMusica = valor;
        PlayerPrefs.SetFloat("VolumeMusica", valor);

        if (musicaSource != null)
            musicaSource.volume = valor;
    }

    public void SetVolumeEfeitos(float valor)
    {
        volumeEfeitos = valor;
        PlayerPrefs.SetFloat("VolumeEfeitos", valor);
    }
}
