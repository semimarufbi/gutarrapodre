using UnityEngine;

public class controlebotao : MonoBehaviour
{
    public SpriteRenderer theSR;
    public Sprite defautImage;
    public Sprite imagepressed;
    public KeyCode keyTopress;

    private bool podePressionar = false;
    private Collider2D notaNaArea = null; // guarda referência da nota que está no quadrado

    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();
        theSR.sprite = defautImage;
    }

    void Update()
    {
        if (podePressionar && notaNaArea != null)
        {
            if (Input.GetKeyDown(keyTopress))
            {
                theSR.sprite = imagepressed;

                // Destrói a nota que está na área
                Destroy(notaNaArea.gameObject);

                // Avise o gameManager que a nota foi acertada, se quiser
                // gameManager.instance.NoteHit();

                Debug.Log($"Nota acertada com a tecla {keyTopress}");

                // Limpa a referência para evitar erro
                notaNaArea = null;
                podePressionar = false;
            }
            if (Input.GetKeyUp(keyTopress))
            {
                theSR.sprite = defautImage;
            }
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
            podePressionar = false;
            theSR.sprite = defautImage;

            // Limpa referência se a nota sair da área
            if (notaNaArea == other)
                notaNaArea = null;
        }
    }
}
