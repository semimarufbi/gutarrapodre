using UnityEngine;

public class controlebotao : MonoBehaviour
{
    private SpriteRenderer theSR;
    public Sprite defautImage;
    public Sprite imagepressed;
    public KeyCode keyTopress;

    private bool podePressionar = false;
    private Collider2D notaNaArea = null; // referência da nota na área

    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();
        theSR.sprite = defautImage;
    }

    void Update()
    {
        // Sempre mostra a animação quando a tecla for pressionada (com ou sem nota)
        if (Input.GetKeyDown(keyTopress))
        {
            theSR.sprite = imagepressed;

            // Se tem nota, acerta e destrói
            if (podePressionar && notaNaArea != null)
            {
                Destroy(notaNaArea.gameObject);
                gameManager.instance.NoteHit(); // Conta o acerto
                Debug.Log($"Nota acertada na lane {keyTopress}");
                notaNaArea = null;
                podePressionar = false;
            }
        }

        // Volta para sprite padrão quando soltar a tecla
        if (Input.GetKeyUp(keyTopress))
        {
            theSR.sprite = defautImage;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Note"))
        {
            podePressionar = true;
            notaNaArea = other;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Note"))
        {
            if (notaNaArea == other)
            {
                notaNaArea = null;
                podePressionar = false;
                gameManager.instance.NoteMissed(); // Conta como erro
            }
        }
    }
}
