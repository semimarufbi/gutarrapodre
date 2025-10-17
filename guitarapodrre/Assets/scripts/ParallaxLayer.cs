using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Header("Configurações")]
    public float speed = 0.05f; // velocidade de movimento
    public bool autoWidth = true; // detecta automaticamente a largura do sprite
    public float manualWidth = 20f; // caso autoWidth = false

    private Vector3 startPos;
    private float spriteWidth;

    void Start()
    {
        startPos = transform.position;

        if (autoWidth)
        {
            // tenta pegar a largura real do sprite
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
                spriteWidth = sr.bounds.size.x;
            else
                spriteWidth = manualWidth;
        }
        else
        {
            spriteWidth = manualWidth;
        }
    }

    void Update()
    {
        // Move para a esquerda
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Reposiciona suavemente quando sai da tela
        if (transform.position.x <= startPos.x - spriteWidth)
        {
            transform.position += new Vector3(spriteWidth, 0, 0);
        }
    }
}
