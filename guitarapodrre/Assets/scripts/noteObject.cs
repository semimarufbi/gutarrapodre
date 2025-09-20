using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NoteObject : MonoBehaviour
{
    public float speed = 5f;
    public float limiteY = -5f; // limite inferior da tela para erro

    [HideInInspector] public bool jaAcertada = false;

    void OnEnable()
    {
        Debug.Log("[NoteObject] Nota ativada: " + gameObject.name);
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        if (!jaAcertada && transform.position.y < limiteY)
        {
            gameManager.instance.NoteMissed();
            vibracaomanager.instance.Vibrar(5f, 0.3f); // vibração mais forte ao errar
            Destroy(gameObject);
        }
    }

    // Método chamado quando o jogador acerta a nota
    public void Acertou()
    {
        if (!jaAcertada)
        {
            jaAcertada = true;
            gameManager.instance.NoteHit();
            vibracaomanager.instance.Vibrar(3.5f, 0.2f); // vibração curta ao acertar
            Destroy(gameObject);
        }
    }
}
