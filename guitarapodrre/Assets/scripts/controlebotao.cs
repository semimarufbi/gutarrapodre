using UnityEngine;

public class controlebotao : MonoBehaviour
{
    private SpriteRenderer theSR;
    public Sprite defautImage;
    public Sprite imagepressed;
    public KeyCode keyTopress;

    private bool podePressionar = false;
    private Collider2D notaNaArea = null; // refer�ncia da nota que est� no quadrado

    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();
        theSR.sprite = defautImage;
    }

    void Update()
    {
        // Troca o sprite sempre que tecla for pressionada
        if (Input.GetKeyDown(keyTopress))
        {
            theSR.sprite = imagepressed;

            // Se tiver nota na �rea, destr�i ela e contabiliza o hit
            if (podePressionar && notaNaArea != null)
            {
                Destroy(notaNaArea.gameObject);
                gameManager.instance.NoteHit();

                Debug.Log($"Nota acertada com a tecla {keyTopress}");

                notaNaArea = null;
                podePressionar = false;
            }
        }

        // Quando soltar a tecla, volta para sprite padr�o
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
            // Se a nota sair sem ser pressionada, contabiliza perda
            if (notaNaArea == other)
            {
                notaNaArea = null;
                podePressionar = false;

                gameManager.instance.NoteMissed();

                theSR.sprite = defautImage;
            }
        }
    }
}
