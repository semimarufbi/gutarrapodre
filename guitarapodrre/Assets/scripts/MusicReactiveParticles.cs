using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class MusicReactiveParticles : MonoBehaviour
{
    public AudioSource audioSource; // Arraste o AudioSource da música aqui
    public int spectrumIndex = 5;   // Qual parte do espectro usar (grave = 0, agudo = valores maiores)
    public float intensity = 10f;   // Intensidade da reação
    public float smoothSpeed = 5f;  // Suavização da resposta

    private ParticleSystem ps;
    private ParticleSystem.MainModule main;
    private ParticleSystem.EmissionModule emission;
    private float[] spectrum = new float[64];
    private float currentValue;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        main = ps.main;
        emission = ps.emission;
    }

    void Update()
    {
        // Pega dados de frequência da música
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        float value = spectrum[spectrumIndex] * intensity;

        // Suaviza a variação
        currentValue = Mathf.Lerp(currentValue, value, Time.deltaTime * smoothSpeed);

        // Aplica nos parâmetros das partículas
        emission.rateOverTime = Mathf.Clamp(currentValue * 100f, 10f, 1000f);
        main.startSize = Mathf.Clamp(0.5f + currentValue, 0.5f, 5f);
    }
}
