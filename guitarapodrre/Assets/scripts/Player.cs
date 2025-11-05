using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float velocidade = 5f;
    [SerializeField] private float velocidadeCorrendo = 8f;
    [SerializeField] private Animator animacao;

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

        // Detecta o chão
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Corrida
        correndo = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton4);

        // Atualiza parâmetros de animação
        animacao.SetFloat("Velocidade", Mathf.Abs(inputHorizontal));
        animacao.SetBool("NoChao", isGrounded);
        animacao.SetBool("Correndo", correndo);

        // Se está no chão, reseta o duplo pulo
        if (isGrounded)
            podeDuploPulo = true;

        // --- SISTEMA DE PULO ---
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                // Pulo normal
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animacao.SetTrigger("Pulo");
            }
            else if (podeDuploPulo)
            {
                // Duplo pulo
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                podeDuploPulo = false;
                animacao.SetTrigger("Pulo");
            }
        }

        // Animações de andar/parado
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

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
