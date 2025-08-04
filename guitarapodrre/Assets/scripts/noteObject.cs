using UnityEngine;

public class noteObject : MonoBehaviour
{
    public KeyCode keyToPress;
    public bool canBePressed;

    [HideInInspector] public int laneIndex;
    [HideInInspector] public noteSpa spawner;

    void Update()
    {
        if (Input.GetKeyDown(keyToPress) && canBePressed)
        {
            gameManager.instance.NoteHit();
            NotifyDestroyed();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
        {
            canBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
        {
            if (canBePressed)
            {
                canBePressed = false;
                gameManager.instance.NoteMissed();
                NotifyDestroyed();
                Destroy(gameObject);
            }
        }
    }

    void OnDestroy()
    {
        NotifyDestroyed();
    }

    void NotifyDestroyed()
    {
        if (spawner != null)
        {
            spawner.OnNoteDestroyed(laneIndex);
        }
    }
}
