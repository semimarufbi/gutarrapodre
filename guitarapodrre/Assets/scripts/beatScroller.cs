using UnityEngine;

public class beatScroller : MonoBehaviour
{
    public float beatTempo;
    public bool hasStarted;

    void Update()
    {
        if (hasStarted)
        {
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
        }
    }
}
