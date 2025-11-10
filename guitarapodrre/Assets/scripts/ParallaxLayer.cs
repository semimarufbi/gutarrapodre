using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Header("Configurações")]
    public float parallaxMultiplier = 0.5f; // intensidade do movimento horizontal
    public float verticalMultiplier = 0.2f; // intensidade do movimento vertical (pulo)
    public bool autoWidth = true;
    public float manualWidth = 20f;

    [Header("Referências")]
    public Transform player; // referência ao jogador

    private Vector3 lastPlayerPos;
    private Vector3 startPos;
    private float spriteWidth;

    void Start()
    {
        if (player == null)
        {
            Debug.LogWarning("⚠️ Nenhum jogador atribuído ao ParallaxLayer!");
            enabled = false;
            return;
        }

        startPos = transform.position;
        lastPlayerPos = player.position;

        // Detecta largura do sprite
        if (autoWidth)
        {
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

    void LateUpdate()
    {
        // Diferença de movimento do jogador
        Vector3 deltaMovement = player.position - lastPlayerPos;

        // Movimento horizontal: parallax reage ao deslocamento do jogador
        transform.position += new Vector3(deltaMovement.x * parallaxMultiplier, 0f, 0f);

        // Movimento vertical: pequena resposta ao pulo (profundidade)
        transform.position += new Vector3(0f, deltaMovement.y * verticalMultiplier, 0f);

        // Loop infinito: reposiciona quando sai da tela
        float distanceFromStart = player.position.x - startPos.x;
        if (distanceFromStart > spriteWidth)
        {
            startPos.x += spriteWidth;
        }

        lastPlayerPos = player.position;
    }
}
