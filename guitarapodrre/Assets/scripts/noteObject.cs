using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NoteObject : MonoBehaviour
{
    [Header("Configurações")]
    public float speed = 5f;
    public float limiteY = -5f;

    [Header("Aparência")]
    public Color corNormal = Color.white;
    public Color corEspecial = new Color(1f, 0.84f, 0f); // dourado
    public ParticleSystem efeitoEspecial; // brilho constante
    public GameObject hitEffect;          // explosãozinha no acerto

    [Header("Áudio (opcional)")]
    public AudioClip somAcerto;
    public AudioClip somErro;
    private AudioSource audioSource;

    // Estado interno
    [HideInInspector] public bool jaAcertada = false;
    private bool especial = false;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void OnEnable()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    void Update()
    {
        // Movimento da nota
        transform.position += Vector3.down * speed * Time.deltaTime;

        // Se passou do limite e não foi acertada → erro
        if (!jaAcertada && transform.position.y < limiteY)
        {
            gameManager.instance.NoteMissed();
            vibracaomanager.instance.Vibrar(5f, 0.3f);

            if (somErro != null) audioSource.PlayOneShot(somErro);

            Destroy(gameObject);
        }
    }

    // Setup chamado pelo Spawner
    public void Setup(bool isEspecial)
    {
        especial = isEspecial;

        if (sr != null)
            sr.color = especial ? corEspecial : corNormal;

        if (efeitoEspecial != null)
        {
            if (especial) efeitoEspecial.Play();
            else efeitoEspecial.Stop();
        }
    }

    // Quando o jogador acerta a nota
    public void Acertou()
    {
        if (jaAcertada) return;

        jaAcertada = true;
        gameManager.instance.NoteHit(especial);
        vibracaomanager.instance.Vibrar(3.5f, 0.2f);

        if (somAcerto != null) audioSource.PlayOneShot(somAcerto);

        if (hitEffect != null)
            Instantiate(hitEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
