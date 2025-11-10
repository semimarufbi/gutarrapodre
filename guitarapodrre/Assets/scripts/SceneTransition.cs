using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    [Header("Fade Config")]
    public Image fadeImage;
    public float fadeDuration = 1f;
    public Color fadeColor = Color.black;

    [Header("Cena de destino")]
    public string proximaCena;

    private static SceneTransition instance;

    void Awake()
    {
        // Garante que só exista 1 (singleton)
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Garante que o fade comece visível
        if (fadeImage != null)
        {
            fadeImage.color = fadeColor;
        }

        // Escuta quando uma nova cena carrega
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        if (fadeImage != null)
            StartCoroutine(FadeIn());
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void TrocarCena()
    {
        if (!string.IsNullOrEmpty(proximaCena))
            StartCoroutine(FadeAndLoad(proximaCena));
        else
            Debug.LogWarning("⚠️ Nenhuma cena foi definida em 'proximaCena'!");
    }

    IEnumerator FadeIn()
    {
        float t = 1f;
        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;

        while (t > 0)
        {
            t -= Time.deltaTime / fadeDuration;
            c.a = Mathf.Clamp01(t);
            fadeImage.color = c;
            yield return null;
        }
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < 1)
        {
            t += Time.deltaTime / fadeDuration;
            c.a = Mathf.Clamp01(t);
            fadeImage.color = c;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Assim que a nova cena carregar → faz o FadeIn de novo
        if (fadeImage != null)
            StartCoroutine(FadeIn());
    }
}
