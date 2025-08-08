using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public float speed = 5f;

    void OnEnable()
    {
        Debug.Log("[NoteObject] Ativada nota: " + gameObject.name);
    }

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }
}
