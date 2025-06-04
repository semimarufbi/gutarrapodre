using UnityEngine;

public class beatScroller : MonoBehaviour
{
    public bool hasStarted = false;
    public float noteTravelTime = 2f;
    private Vector3 moveDirection;

    void Start()
    {
        moveDirection = new Vector3(0f, -1f, 0f) / noteTravelTime;
    }

    void Update()
    {
        if (hasStarted)
        {
            transform.position += moveDirection * Time.deltaTime;
        }
    }

    public void SetTravelTime(float travelTime)
    {
        noteTravelTime = travelTime;
        moveDirection = new Vector3(0f, -1f, 0f) / noteTravelTime;

        hasStarted = true; // <- ESSA LINHA ATIVA O MOVIMENTO
    }
}
