using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NoteObject : MonoBehaviour
{
    [Header("Configurações")]
    public float speed = 10f;
    public float limiteY = -5f;

    [Header("Aparência")]
    public Color corNormal = Color.white;
    public Color corEspecial = new Color(1f, 0.84f, 0f); // dourado
    public ParticleSystem efeitoEspecial;
    public GameObject hitEffect;

    [Header("Áudio")]
    public AudioClip somAcerto;
    [Range(0f, 1f)] public float volumeAcerto = 0.6f;
    public AudioClip somErro;
    [Range(0f, 1f)] public float volumeErro = 0.6f;

    [Header("Override de material (opcional)")]
    [Tooltip("Se quiser substituir o sprite por uma cor sólida quando for especial, arraste aqui um material que use o shader 'Sprite/ReplaceWithColor' (veja o shader no final).")]
    public Material specialReplaceMaterial;

    private SpriteRenderer sr;
    private bool especial = false;
    [HideInInspector] public bool jaAcertada = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        if (!jaAcertada && transform.position.y < limiteY)
        {
            gameManager.instance.NoteMissed();
            vibracaomanager.instance.Vibrar(5f, 0.3f);

            if (somErro != null)
                TocarSomPersistente(somErro, volumeErro);

            // instancia efeito de erro se tiver (opcional)
            Destroy(gameObject);
        }
    }

    // chamado pelo spawner
    public void Setup(bool isEspecial)
    {
        especial = isEspecial;

        if (sr == null) sr = GetComponent<SpriteRenderer>();

        // Se houver material especial para substituir, use-o só quando especial
        if (specialReplaceMaterial != null)
        {
            if (especial)
            {
                // aplica material que desenha cor sólida usando alpha do sprite
                sr.material = specialReplaceMaterial;
                // define cor do material (se o shader possuir _Color)
                if (sr.material.HasProperty("_Color"))
                    sr.material.SetColor("_Color", corEspecial);
            }
            else
            {
                // restaura material padrão (é importante ter uma referência de fallback)
                sr.material = new Material(Shader.Find("Sprites/Default"));
                sr.color = corNormal;
            }
        }
        else
        {
            // sem material especial: tente forçar a cor do SpriteRenderer
            if (sr != null)
                sr.color = especial ? corEspecial : corNormal;
        }

        if (efeitoEspecial != null)
        {
            if (especial) efeitoEspecial.Play();
            else efeitoEspecial.Stop();
        }
    }

    public void Acertou()
    {
        if (jaAcertada) return;

        jaAcertada = true;
        gameManager.instance.NoteHit(especial);
        vibracaomanager.instance.Vibrar(3.5f, 0.2f);

        if (somAcerto != null)
            TocarSomPersistente(somAcerto, volumeAcerto);

        if (hitEffect != null)
            Instantiate(hitEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    // Toca som em GameObject temporário que persiste até o fim do clip
    private void TocarSomPersistente(AudioClip clip, float volume)
    {
        if (clip == null) return;

        GameObject go = new GameObject("TempAudio_" + clip.name);
        DontDestroyOnLoad(go); // evita que algo o destrua por cena
        AudioSource src = go.AddComponent<AudioSource>();
        src.clip = clip;
        src.volume = Mathf.Clamp01(volume);
        src.playOnAwake = false;
        src.spatialBlend = 0f; // 2D sound, evita problemas de distância
        src.loop = false;
        src.Play();
        Destroy(go, clip.length + 0.25f); // destrói depois do som terminar
    }
}
