using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float speed = 0.05f; // Ajusta no Inspector
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // loop infinito
        if (transform.position.x < -20f)
            transform.position = startPos;
    }
}
