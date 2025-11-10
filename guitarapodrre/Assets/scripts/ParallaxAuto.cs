using UnityEngine;

public class ParallaxAuto : MonoBehaviour
{
    public float speed = 0.1f;
    private Vector3 startPos;
    private float length;

    void Start()
    {
        startPos = transform.position;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            length = sr.bounds.size.x;
    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x <= startPos.x - length)
            transform.position = startPos;
    }
}
