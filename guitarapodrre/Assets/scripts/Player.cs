using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float velocidade = 5f;
    [SerializeField] private float velocidadeCorrendo = 8f;
    [SerializeField] private Animator animacao; // 🔸 Animação desativada

    [Header("Pulo")]
    public bool isGrounded;
    private bool podeDuploPulo;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Corrida")]
    private bool correndo;

    private float inputHorizontal;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb.gravityScale < 3f)
            rb.gravityScale = 5f;
    }

    private void Update()
    {
        inputHorizontal = Input.GetAxis("Horizontal");

        // Flip do personagem
        spriteFlip(inputHorizontal);

        // Corrida
        correndo = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton4);

         
        animacao.SetFloat("Velocidade", Mathf.Abs(inputHorizontal));
        animacao.SetBool("NoChao", isGrounded);
        animacao.SetBool("Correndo", correndo);
        

       

            // animacao.SetTrigger("Pulo"); // 🔸 Removido

            if (!isGrounded)
                podeDuploPulo = false;
        }

        
        if (isGrounded)
        {
            if (Mathf.Abs(inputHorizontal) > 0.01f)
            {
                animacao.SetBool("parado", false);
                animacao.SetBool("andando", !correndo);
                animacao.SetBool("Correndo", correndo);
            }
            else
            {
                animacao.SetBool("parado", true);
                animacao.SetBool("andando", false);
                animacao.SetBool("Correndo", false);
            }
        }
        else
        {
            animacao.SetBool("parado", false);
            animacao.SetBool("andando", false);
        }
        
    }

    private void FixedUpdate()
    {
        float velocidadeAtual = correndo ? velocidadeCorrendo : velocidade;
        rb.velocity = new Vector2(inputHorizontal * velocidadeAtual, rb.velocity.y);
    }

    private void spriteFlip(float horizontal)
    {
        if (horizontal < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (horizontal > 0)
            transform.localScale = new Vector3(1, 1, 1);
    }

    
}
