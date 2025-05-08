using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlebotao : MonoBehaviour
{
    public SpriteRenderer theSR;
    public Sprite defautImage;
    public Sprite imagepressed;
    public KeyCode keyTopress;
    // Start is called before the first frame update
    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frameas
    void Update()
    {
        if (Input.GetKeyDown(keyTopress))
        {
            theSR.sprite = imagepressed;
        }
        if (Input.GetKeyUp(keyTopress))
        {
            theSR.sprite = defautImage;
        }
    }
}
