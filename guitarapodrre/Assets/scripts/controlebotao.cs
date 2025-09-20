using UnityEngine;

public class ControleBotao : MonoBehaviour
{
    private SpriteRenderer theSR;
    public Sprite defautImage;
    public Sprite imagePressed;
    public KeyCode keyToPress; // teclado

    private NoteObject notaNaArea = null;

    [Header("Gamepad")]
    public bool usarGamepad = true;

    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();
        theSR.sprite = defautImage;
    }

    void Update()
    {
        bool pressionou = false;

        // Teclado
        if (Input.GetKeyDown(keyToPress))
            pressionou = true;

        // Gamepad
        if (usarGamepad)
        {
            string lane = gameObject.name.ToLower();

            // LT = Blue
            if (lane.Contains("blue") && Input.GetAxis("LT") > 0.5f) pressionou = true;
            

            // LB = Red
            if (lane.Contains("red") && Input.GetKeyDown(KeyCode.JoystickButton4))
                pressionou = true;

            // RB = Yellow
            if (lane.Contains("yellow") && Input.GetKeyDown(KeyCode.JoystickButton5))
                pressionou = true;

            // RT = Green
            if (lane.Contains("green") && Input.GetAxis("RT") > 0.5f) pressionou = true;
            
        }

        // Se apertou
        if (pressionou)
        {
            theSR.sprite = imagePressed;

            if (notaNaArea != null)
            {
                notaNaArea.Acertou(); // método do NoteObject para registrar acerto
                notaNaArea = null;
            }
        }

        // Soltou (teclado + bumpers)
        if (Input.GetKeyUp(keyToPress) ||
            (usarGamepad &&
             (Input.GetKeyUp(KeyCode.JoystickButton4) || Input.GetKeyUp(KeyCode.JoystickButton5))))
        {
            theSR.sprite = defautImage;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        NoteObject nota = other.GetComponent<NoteObject>();
        if (nota != null)
        {
            notaNaArea = nota;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        NoteObject nota = other.GetComponent<NoteObject>();
        if (nota != null && nota == notaNaArea)
        {
            notaNaArea = null;
        }
    }
}
